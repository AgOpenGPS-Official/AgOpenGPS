using System.Collections.Generic;

namespace AgOpenGPS
{
    /// <summary>
    /// Defines a template for task notes based on work type.
    /// Each work type can have specific fields (e.g., Product, Rate, Unit for Spraying).
    /// </summary>
    public class WorkTypeTemplate
    {
        /// <summary>
        /// The work type this template applies to (e.g., "Spraying", "Seeding")
        /// </summary>
        public string WorkType { get; set; }

        /// <summary>
        /// List of fields that should be displayed for this work type
        /// </summary>
        public List<TemplateField> Fields { get; set; }

        public WorkTypeTemplate()
        {
            Fields = new List<TemplateField>();
        }

        public WorkTypeTemplate(string workType)
        {
            WorkType = workType;
            Fields = new List<TemplateField>();
        }
    }

    /// <summary>
    /// Defines a single field in a work type template
    /// </summary>
    public class TemplateField
    {
        /// <summary>
        /// Internal name of the field (e.g., "Product", "Rate", "Unit")
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display label for the field (translatable)
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Type of input control: "text", "dropdown"
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// For dropdown fields: comma-separated list of options
        /// </summary>
        public string Options { get; set; }

        /// <summary>
        /// Default value for this field
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Whether this field is required
        /// </summary>
        public bool Required { get; set; }

        public TemplateField()
        {
            FieldType = "text";
            Required = false;
        }

        public TemplateField(string name, string label, string fieldType = "text")
        {
            Name = name;
            Label = label;
            FieldType = fieldType;
            Required = false;
        }
    }
}
