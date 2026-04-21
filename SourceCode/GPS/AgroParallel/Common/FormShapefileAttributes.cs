// ============================================================================
// FormShapefileAttributes.cs - Popup de atributos DBF del poligono clickeado
// Ubicación: SourceCode/GPS/AgroParallel/Common/FormShapefileAttributes.cs
// Target: net48 (C# 7.3)
//
// Dialogo modal que muestra los atributos DBF del poligono seleccionado en un
// ListView de dos columnas (Campo, Valor). Destaca en negrita el campo de
// estilo activo. Usado por el modo "inspeccion" (paso 12): el usuario activa
// el modo, clickea un poligono en el mapa, y se abre este popup.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AgroParallel.Common
{
    public class FormShapefileAttributes : Form
    {
        public FormShapefileAttributes(int polyIndex, IReadOnlyList<string> fieldOrder,
            IReadOnlyDictionary<string, object> attrs, string highlightField,
            double? currentValue)
        {
            Text = "Atributos del poligono #" + polyIndex;
            Size = new Size(520, 420);
            FormBorderStyle = FormBorderStyle.Sizable;
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(360, 260);
            MaximizeBox = true;
            ShowInTaskbar = false;
            Font = new Font("Segoe UI", 10f);

            var lv = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable
            };
            lv.Columns.Add("Campo", 180);
            lv.Columns.Add("Valor", 300);

            if (fieldOrder != null)
            {
                foreach (var field in fieldOrder)
                {
                    if (string.IsNullOrEmpty(field)) continue;
                    object raw;
                    string val = (attrs != null && attrs.TryGetValue(field, out raw))
                        ? FormatValue(raw)
                        : "";
                    var item = new ListViewItem(field);
                    item.SubItems.Add(val);
                    if (string.Equals(field, highlightField, StringComparison.Ordinal))
                    {
                        item.Font = new Font(Font, FontStyle.Bold);
                        item.BackColor = Color.FromArgb(255, 248, 210);
                    }
                    lv.Items.Add(item);
                }
            }

            Controls.Add(lv);

            var footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            if (!string.IsNullOrEmpty(highlightField) && currentValue.HasValue)
            {
                var lbl = new Label
                {
                    Text = "Dosis muestreada: "
                        + currentValue.Value.ToString("G6", CultureInfo.InvariantCulture),
                    Location = new Point(14, 12),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(30, 100, 30),
                    BackColor = Color.Transparent
                };
                footer.Controls.Add(lbl);
            }

            var btnClose = new Button
            {
                Text = "Cerrar",
                Size = new Size(100, 30),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                DialogResult = DialogResult.OK
            };
            btnClose.Location = new Point(footer.Width - btnClose.Width - 12, 7);
            footer.Controls.Add(btnClose);
            AcceptButton = btnClose;

            Controls.Add(footer);
        }

        private static string FormatValue(object raw)
        {
            if (raw == null) return "";
            if (raw is double d) return d.ToString("G15", CultureInfo.InvariantCulture);
            if (raw is float f) return f.ToString("G7", CultureInfo.InvariantCulture);
            if (raw is decimal m) return m.ToString(CultureInfo.InvariantCulture);
            if (raw is DateTime dt) return dt.ToString("u", CultureInfo.InvariantCulture);
            return Convert.ToString(raw, CultureInfo.InvariantCulture) ?? "";
        }
    }
}
