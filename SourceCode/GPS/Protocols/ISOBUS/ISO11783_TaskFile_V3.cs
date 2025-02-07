using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace AgOpenGPS.Protocols.ISOBUS
{
    public class ISO11783_TaskFile_V3
    {
        public static void Export(string fileName, string designator, int area, List<CBoundaryList> bndList, CNMEA pn, CTrack trk)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            XmlWriter xml = XmlWriter.Create(fileName, settings);

            xml.WriteStartElement("ISO11783_TaskData");//Settings
            xml.WriteAttributeString("DataTransferOrigin", "1");
            xml.WriteAttributeString("ManagementSoftwareManufacturer", "AgOpenGPS");
            xml.WriteAttributeString("ManagementSoftwareVersion", "1.4.0");
            xml.WriteAttributeString("VersionMajor", "3");
            xml.WriteAttributeString("VersionMinor", "3");

            {
                //PFD A = "Field ID" B = "Code" C = "Name" D = "Area sq m" E = "Customer Ref" F = "Farm Ref" >
                xml.WriteStartElement("PFD");//Field
                xml.WriteAttributeString("A", "PFD-1");
                xml.WriteAttributeString("C", designator);
                xml.WriteAttributeString("D", area.ToString(CultureInfo.InvariantCulture));

                double lat = 0;
                double lon = 0;

                {
                    //all the boundaries
                    /*
                    < PLN A = "1" C="Area in Sq M like 12568" >
                        < LSG A = "1" >
                            < PNT A = "2" C = "51.61918340" D = "4.51054560" />
                            < PNT A = "2" C = "51.61915460" D = "4.51056120" />
                        </ LSG >
                    </ PLN >
                    */
                    for (int i = 0; i < bndList.Count; i++)
                    {
                        xml.WriteStartElement("PLN");//BND

                        if (i == 0) xml.WriteAttributeString("A", "1"); //outerBnd
                        else xml.WriteAttributeString("A", "6");  //innerBnd

                        xml.WriteStartElement("LSG");//Polygon
                        xml.WriteAttributeString("A", "1");

                        for (int j = 0; j < bndList[i].fenceLineEar.Count; j++)
                        {

                            pn.ConvertLocalToWGS84(bndList[i].fenceLineEar[j].northing, bndList[i].fenceLineEar[j].easting, out lat, out lon);
                            xml.WriteStartElement("PNT");//Boundary Points
                            xml.WriteAttributeString("A", "2");
                            xml.WriteAttributeString("C", lat.ToString(CultureInfo.InvariantCulture));
                            xml.WriteAttributeString("D", lon.ToString(CultureInfo.InvariantCulture));
                            xml.WriteEndElement(); //Boundary Points                   
                        }

                        xml.WriteEndElement();//Polygon
                        xml.WriteEndElement();//BND
                    }

                    //all the headlands A=10
                    if (bndList.Count > 0)
                    {
                        for (int i = 0; i < bndList.Count; i++)
                        {
                            if (bndList[i].hdLine.Count < 1) continue;

                            xml.WriteStartElement("PLN");//BND

                            xml.WriteAttributeString("A", "10"); //headland

                            xml.WriteStartElement("LSG");//Polygon
                            xml.WriteAttributeString("A", "1");

                            for (int j = 0; j < bndList[i].hdLine.Count; j++)
                            {
                                pn.ConvertLocalToWGS84(bndList[i].hdLine[j].northing, bndList[i].hdLine[j].easting, out lat, out lon);
                                xml.WriteStartElement("PNT");//Boundary Points
                                xml.WriteAttributeString("A", "2");
                                xml.WriteAttributeString("C", lat.ToString(CultureInfo.InvariantCulture));
                                xml.WriteAttributeString("D", lon.ToString(CultureInfo.InvariantCulture));
                                xml.WriteEndElement(); //Boundary Points                   
                            }

                            xml.WriteEndElement();//Polygon
                            xml.WriteEndElement();//BND
                        }
                    }

                    //AB Lines
                    /*
                    LSG A = "5" B = "Line Name" >
                        < PNT A = "2" C = "51.61851540" D = "4.51137030" />
                        < PNT A = "2" C = "51.61912230" D = "4.51056060" />
                    </ LSG >
                    */

                    if (trk.gArr != null && trk.gArr.Count > 0)
                    {
                        for (int i = 0; i < trk.gArr.Count; i++)
                        {
                            xml.WriteStartElement("LSG");//Line
                            xml.WriteAttributeString("A", "5");
                            xml.WriteAttributeString("B", trk.gArr[i].name);
                            ///xml.WriteAttributeString("C", (tool.width).ToString(CultureInfo.InvariantCulture));
                            {
                                xml.WriteStartElement("PNT");//A

                                pn.ConvertLocalToWGS84(trk.gArr[i].ptA.northing - Math.Cos(trk.gArr[i].heading) * 1000,
                                    trk.gArr[i].ptA.easting - Math.Sin(trk.gArr[i].heading) * 1000, out lat, out lon);

                                xml.WriteAttributeString("A", "2");
                                xml.WriteAttributeString("C", lat.ToString(CultureInfo.InvariantCulture));
                                xml.WriteAttributeString("D", lon.ToString(CultureInfo.InvariantCulture));

                                xml.WriteEndElement();//A
                                xml.WriteStartElement("PNT");//B

                                pn.ConvertLocalToWGS84(trk.gArr[i].ptA.northing + Math.Cos(trk.gArr[i].heading) * 1000,
                                    trk.gArr[i].ptA.easting + Math.Sin(trk.gArr[i].heading) * 1000, out lat, out lon);

                                xml.WriteAttributeString("A", "2");

                                xml.WriteAttributeString("C", lat.ToString(CultureInfo.InvariantCulture));
                                xml.WriteAttributeString("D", lon.ToString(CultureInfo.InvariantCulture));
                            }
                            xml.WriteEndElement();//B
                            xml.WriteEndElement();//Line
                        }
                    }

                    //curves
                    /*
                    LSG A = "5" B = "Name Here" >
                        < PNT A = "2" C = "51.61851540" D = "4.51137030" />
                        < PNT A = "2" C = "51.61912230" D = "4.51056060" />
                        < PNT A = "2" C = "51.61962230" D = "4.51056760" />
                    </ LSG >
                    */
                    if (trk.gArr != null && trk.gArr.Count > 0)
                    {
                        for (int i = 0; i < trk.gArr.Count; i++)
                        {
                            xml.WriteStartElement("LSG");//Curve
                            xml.WriteAttributeString("A", "5"); //denotes guidance
                            xml.WriteAttributeString("B", trk.gArr[i].name);
                            //xml.WriteAttributeString("C", (tool.width).ToString(CultureInfo.InvariantCulture));

                            for (int j = 0; j < trk.gArr[i].curvePts.Count; j++)
                            {
                                xml.WriteStartElement("PNT");//point
                                pn.ConvertLocalToWGS84(trk.gArr[i].curvePts[j].northing,
                                    trk.gArr[i].curvePts[j].easting, out lat, out lon);

                                xml.WriteAttributeString("A", "2");
                                xml.WriteAttributeString("C", lat.ToString(CultureInfo.InvariantCulture));
                                xml.WriteAttributeString("D", lon.ToString(CultureInfo.InvariantCulture));

                                xml.WriteEndElement();//point
                            }
                            xml.WriteEndElement(); //Curve   
                        }
                    }
                }

                //Last
                xml.WriteEndElement();//End Field
            }

            xml.WriteEndElement();//ISO11783_TaskData Settings

            xml.Flush();

            //Write the XML to file and close the kml
            xml.Close();
        }
    }
}
