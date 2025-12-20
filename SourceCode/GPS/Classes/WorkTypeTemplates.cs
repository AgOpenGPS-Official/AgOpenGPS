using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace AgOpenGPS
{
    /// <summary>
    /// Manages work type templates and stores last used values per work type.
    /// Templates define which fields to show for task notes based on work type.
    /// </summary>
    public static class WorkTypeTemplates
    {
        private static Dictionary<string, Dictionary<string, string>> lastUsedValues;
        private static string storageFilePath;

        /// <summary>
        /// Initialize templates and load last used values
        /// </summary>
        public static void Initialize(string fieldsDirectory)
        {
            storageFilePath = Path.Combine(fieldsDirectory, "note_templates.json");
            LoadLastUsedValues();
        }

        /// <summary>
        /// Gets the template for a specific work type
        /// </summary>
        public static WorkTypeTemplate GetTemplate(string workType)
        {
            var template = new WorkTypeTemplate(workType);

            // Normalize to lowercase for case-insensitive comparison
            string normalizedType = workType?.ToLowerInvariant() ?? "";

            switch (normalizedType)
            {
                case "spraying":
                    template.Fields.Add(new TemplateField("Product", "Product", "text"));
                    template.Fields.Add(new TemplateField("Rate", "Rate", "text"));
                    template.Fields.Add(new TemplateField("Unit", "Unit", "dropdown")
                    {
                        Options = "L/ha,kg/ha,mL/ha"
                    });
                    break;

                case "seeding":
                case "planting":
                    template.Fields.Add(new TemplateField("Crop", "Crop", "text"));
                    template.Fields.Add(new TemplateField("Variety", "Variety", "text"));
                    template.Fields.Add(new TemplateField("SeedRate", "Seed Rate", "text"));
                    template.Fields.Add(new TemplateField("Unit", "Unit", "dropdown")
                    {
                        Options = "kg/ha,seeds/ha,lbs/ac"
                    });
                    break;

                case "fertilizing":
                    template.Fields.Add(new TemplateField("Product", "Product", "text"));
                    template.Fields.Add(new TemplateField("Rate", "Rate", "text"));
                    template.Fields.Add(new TemplateField("Unit", "Unit", "dropdown")
                    {
                        Options = "kg/ha,L/ha,lbs/ac"
                    });
                    break;

                case "harvesting":
                    template.Fields.Add(new TemplateField("Crop", "Crop", "text"));
                    template.Fields.Add(new TemplateField("Yield", "Yield (optional)", "text"));
                    break;

                case "mowing":
                case "cultivating":
                case "other":
                default:
                    template.Fields.Add(new TemplateField("Notes", "Notes", "text"));
                    break;
            }

            return template;
        }

        /// <summary>
        /// Gets the last used values for a specific work type
        /// </summary>
        public static Dictionary<string, string> GetLastUsedValues(string workType)
        {
            if (lastUsedValues == null)
                LoadLastUsedValues();

            if (lastUsedValues.ContainsKey(workType))
                return new Dictionary<string, string>(lastUsedValues[workType]);

            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Saves the values for a specific work type as "last used"
        /// </summary>
        public static void SaveLastUsedValues(string workType, Dictionary<string, string> values)
        {
            if (lastUsedValues == null)
                lastUsedValues = new Dictionary<string, Dictionary<string, string>>();

            lastUsedValues[workType] = new Dictionary<string, string>(values);
            SaveToFile();
        }

        /// <summary>
        /// Loads last used values from JSON file
        /// </summary>
        private static void LoadLastUsedValues()
        {
            lastUsedValues = new Dictionary<string, Dictionary<string, string>>();

            if (string.IsNullOrEmpty(storageFilePath) || !File.Exists(storageFilePath))
                return;

            try
            {
                string json = File.ReadAllText(storageFilePath);
                lastUsedValues = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json)
                    ?? new Dictionary<string, Dictionary<string, string>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading note templates: {ex.Message}");
                lastUsedValues = new Dictionary<string, Dictionary<string, string>>();
            }
        }

        /// <summary>
        /// Saves last used values to JSON file
        /// </summary>
        private static void SaveToFile()
        {
            if (string.IsNullOrEmpty(storageFilePath))
                return;

            try
            {
                // Ensure directory exists
                string directory = Path.GetDirectoryName(storageFilePath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                string json = JsonConvert.SerializeObject(lastUsedValues, Formatting.Indented);
                File.WriteAllText(storageFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving note templates: {ex.Message}");
            }
        }
    }
}
