using AgLibrary.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AgLibrary.Settings
{
    public enum LoadResult { Ok, MissingFile, Failed };

    public static class XmlSettingsHandler
    {
        public static LoadResult LoadXMLFile(string filePath, object obj)
        {
            bool Errors = false;
            try
            {
                if (!File.Exists(filePath))
                {
                    return LoadResult.MissingFile;
                }

                using (XmlTextReader reader = new XmlTextReader(filePath))
                {
                    string name = "";
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name == "setting")
                                {
                                    name = reader.GetAttribute("name");
                                }
                                else if (reader.Name == "value")
                                {
                                    if (!string.IsNullOrEmpty(name))
                                    {
                                        var pinfo = obj.GetType().GetField(name);
                                        if (pinfo != null)
                                        {
                                            try
                                            {
                                                SetFieldValue(pinfo, reader, obj);
                                            }
                                            catch (Exception)
                                            {
                                                if (Debugger.IsAttached)
                                                    throw;// Re-throws the original exception
                                                Errors = true;
                                            }
                                        }
                                    }
                                }
                                break;

                            case XmlNodeType.EndElement:
                                break;
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                if (Debugger.IsAttached)
                    throw;// Re-throws the original exception
                Errors = true;
            }
            return Errors ? LoadResult.Failed : LoadResult.Ok;
        }

        private static bool SetFieldValue(FieldInfo pinfo, XmlTextReader reader, object obj)
        {
            Type fieldType = pinfo.FieldType;
            if (typeof(IEnumerable).IsAssignableFrom(fieldType) && (fieldType.IsGenericType || fieldType.IsArray))
            {
                Type itemType;

                if (fieldType.IsGenericType) // For generic collections like List<T>
                    itemType = fieldType.GetGenericArguments()[0];
                else if (fieldType.IsArray) // For arrays like T[]
                    itemType = fieldType.GetElementType();
                else
                {
                    throw new NotSupportedException($"Unsupported collection type: {fieldType}");
                }

                var innerXml = reader.ReadInnerXml().Trim();
                if (string.IsNullOrWhiteSpace(innerXml))
                {
                    pinfo.SetValue(obj, null);
                    return true;
                }

                // Deserialize XML into the custom object
                var serializer = new XmlSerializer(typeof(List<>).MakeGenericType(itemType));
                using (var stringReader = new StringReader(innerXml))
                {
                    var list = serializer.Deserialize(stringReader);

                    if (fieldType.IsArray) // Convert List<T> to T[] for arrays
                    {
                        var toArrayMethod = typeof(Enumerable).GetMethod(nameof(Enumerable.ToArray))?.MakeGenericMethod(itemType);
                        if (toArrayMethod == null)
                        {
                            throw new InvalidOperationException($"Failed to create array conversion method for {itemType}.");
                        }

                        var typedArray = toArrayMethod.Invoke(null, new[] { list });
                        pinfo.SetValue(obj, typedArray);
                    }
                    else // Directly assign the List<T>
                    {
                        pinfo.SetValue(obj, list);
                    }
                }
                return true;
            }

            if (fieldType.IsClass && fieldType != typeof(string))
            {
                try
                {
                    var innerXml = reader.ReadInnerXml().Trim();
                    if (string.IsNullOrWhiteSpace(innerXml))
                    {
                        pinfo.SetValue(obj, null);
                        return true;
                    }

                    innerXml = innerXml.Replace(">True<", ">true<").Replace(">False<", ">false<");
                    using (StringReader stringReader = new StringReader(innerXml))
                    {
                        var serializer = new XmlSerializer(fieldType);
                        object nestedObj = serializer.Deserialize(stringReader);
                        pinfo.SetValue(obj, nestedObj);
                    }
                }
                catch (Exception)
                {
                }
                return true;
            }

            // Read string values
            string value = reader.ReadString();

            if (fieldType == typeof(string))
            {
                pinfo.SetValue(obj, value);
            }
            else if (fieldType.IsEnum) // Handle Enums
            {
                var enumValue = Enum.Parse(fieldType, value, ignoreCase: true);
                pinfo.SetValue(obj, enumValue);
            }
            else if (fieldType.IsPrimitive || fieldType == typeof(decimal))
            {
                object parsedValue = Convert.ChangeType(value, fieldType, CultureInfo.InvariantCulture);
                pinfo.SetValue(obj, parsedValue);
            }
            else if (fieldType == typeof(Color))
            {
                var parts = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 3 && parseInvariantCulture(parts[0], out int r) && parseInvariantCulture(parts[1], out int g) && parseInvariantCulture(parts[2], out int b))
                {
                    pinfo.SetValue(obj, Color.FromArgb(r, g, b));
                }
            }
            else if (fieldType == typeof(Point))
            {
                var parts = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && parseInvariantCulture(parts[0], out int x) && parseInvariantCulture(parts[1], out int y))
                {
                    pinfo.SetValue(obj, new Point(x, y));
                }
            }
            else if (fieldType == typeof(Size))
            {
                var parts = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && parseInvariantCulture(parts[0], out int width) && parseInvariantCulture(parts[1], out int height))
                {
                    pinfo.SetValue(obj, new Size(width, height));
                }
            }
            else
            {
                if (Debugger.IsAttached)
                    throw new ArgumentException("type not found");
                return false;
            }
            return true;
        }

        private static bool parseInvariantCulture(string value, out int outValue)
        {
            return int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out outValue);
        }

        public static void SaveXMLFile(string filePath, object obj)
        {
            try
            {
                var dirName = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(dirName) && !Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }

                using (XmlTextWriter xml = new XmlTextWriter(filePath + ".tmp", Encoding.UTF8)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4
                })
                {
                    xml.WriteStartDocument();

                    // Start the root element
                    xml.WriteStartElement("configuration");
                    xml.WriteStartElement("userSettings");
                    xml.WriteStartElement(obj.ToString());

                    foreach (var fld in obj.GetType().GetFields())
                    {
                        var value = fld.GetValue(obj);
                        var fieldType = fld.FieldType;

                        // Start a "setting" element
                        xml.WriteStartElement("setting");

                        // Add attributes to the "setting" element
                        xml.WriteAttributeString("name", fld.Name);

                        if (value == null)
                        {
                            xml.WriteAttributeString("serializeAs", "String");
                            xml.WriteElementString("value", string.Empty);
                        }
                        else if ((fieldType.IsClass && fieldType != typeof(string)) || (typeof(IEnumerable).IsAssignableFrom(fieldType) && (fieldType.IsGenericType || fieldType.IsArray)))
                        {
                            //classes, arrays and lists
                            xml.WriteAttributeString("serializeAs", "Xml");

                            // Write the serialized object to a nested "value" element
                            xml.WriteStartElement("value");

                            var serializer = new XmlSerializer(fieldType);
                            serializer.Serialize(xml, value);

                            xml.WriteEndElement(); // value
                        }
                        else
                        {
                            xml.WriteAttributeString("serializeAs", "String");

                            if (value is Point pointValue)
                            {
                                xml.WriteElementString("value", $"{pointValue.X.ToString(CultureInfo.InvariantCulture)}, {pointValue.Y.ToString(CultureInfo.InvariantCulture)}");
                            }
                            else if (value is Size sizeValue)
                            {
                                xml.WriteElementString("value", $"{sizeValue.Width.ToString(CultureInfo.InvariantCulture)}, {sizeValue.Height.ToString(CultureInfo.InvariantCulture)}");
                            }
                            else if (value is Color dd)
                            {
                                xml.WriteElementString("value", $"{dd.R.ToString(CultureInfo.InvariantCulture)}, {dd.G.ToString(CultureInfo.InvariantCulture)}, {dd.B.ToString(CultureInfo.InvariantCulture)}");
                            }
                            else
                            {
                                // Write primitive types or strings
                                string stringValue = Convert.ToString(value, CultureInfo.InvariantCulture);
                                xml.WriteElementString("value", stringValue);
                            }
                        }

                        xml.WriteEndElement(); // setting
                    }

                    // Close all open elements
                    xml.WriteEndElement(); // AgOpenGPS.Properties.Settings
                    xml.WriteEndElement(); // userSettings
                    xml.WriteEndElement(); // configuration

                    // End the document
                    xml.WriteEndDocument();
                    xml.Flush();
                }

                if (File.Exists(filePath))
                    File.Delete(filePath);

                if (File.Exists(filePath + ".tmp"))
                    File.Move(filePath + ".tmp", filePath);
            }
            catch (Exception ex)
            {
                Log.EventWriter("Exception saving XML file" + ex.ToString());
            }
        }
    }
}
