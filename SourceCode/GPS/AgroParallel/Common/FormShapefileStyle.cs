// ============================================================================
// FormShapefileStyle.cs - Dialogo para elegir campo DBF que pinta el layer
// Ubicación: SourceCode/GPS/AgroParallel/Common/FormShapefileStyle.cs
// Target: net48 (C# 7.3)
//
// Se abre al cargar un shapefile y tambien desde el menu para re-aplicar
// el estilo. Muestra la lista de campos DBF (los numericos se marcan) y
// al elegir uno calcula un gradiente verde→amarillo→rojo en ShapefileLayer.
// ============================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AgroParallel.Common
{
    public class FormShapefileStyle : Form
    {
        private readonly ShapefileLayer _layer;
        private ComboBox _cmbField;
        private Label _lblInfo;
        private Button _btnOk;
        private Button _btnCancel;
        private Button _btnClear;
        private CheckBox _chkShowFill;
        private CheckBox _chkShowOutline;

        // Precompute de stats por campo: "[#] nombre  (min..max, N)" para numericos.
        private readonly List<string> _fieldLabels = new List<string>();
        private readonly List<string> _fieldNamesByIndex = new List<string>();

        public FormShapefileStyle(ShapefileLayer layer)
        {
            _layer = layer ?? throw new ArgumentNullException("layer");
            BuildUI();
            PopulateFields();
            SelectCurrentField();
        }

        private void BuildUI()
        {
            Text = "Estilo Shapefile";
            Size = new Size(520, 280);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            Font = new Font("Segoe UI", 10f);

            var lblTitle = new Label
            {
                Text = "Colorear poligonos por campo DBF:",
                Location = new Point(18, 16),
                AutoSize = true
            };
            Controls.Add(lblTitle);

            _cmbField = new ComboBox
            {
                Location = new Point(18, 42),
                Size = new Size(470, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _cmbField.SelectedIndexChanged += (s, e) => UpdateInfoLabel();
            Controls.Add(_cmbField);

            _lblInfo = new Label
            {
                Location = new Point(18, 78),
                Size = new Size(470, 40),
                ForeColor = Color.DimGray,
                Text = ""
            };
            Controls.Add(_lblInfo);

            _chkShowFill = new CheckBox
            {
                Text = "Mostrar relleno",
                Location = new Point(18, 126),
                AutoSize = true,
                Checked = _layer.ShowFill
            };
            Controls.Add(_chkShowFill);

            _chkShowOutline = new CheckBox
            {
                Text = "Mostrar contorno",
                Location = new Point(180, 126),
                AutoSize = true,
                Checked = _layer.ShowOutline
            };
            Controls.Add(_chkShowOutline);

            _btnClear = new Button
            {
                Text = "Sin estilo",
                Location = new Point(18, 195),
                Size = new Size(110, 32)
            };
            _btnClear.Click += (s, e) =>
            {
                _cmbField.SelectedIndex = 0; // primera entrada = "(Sin colorear)"
            };
            Controls.Add(_btnClear);

            _btnCancel = new Button
            {
                Text = "Cancelar",
                Location = new Point(290, 195),
                Size = new Size(100, 32),
                DialogResult = DialogResult.Cancel
            };
            Controls.Add(_btnCancel);

            _btnOk = new Button
            {
                Text = "Aplicar",
                Location = new Point(396, 195),
                Size = new Size(100, 32),
                DialogResult = DialogResult.OK
            };
            _btnOk.Click += OnApply;
            Controls.Add(_btnOk);

            AcceptButton = _btnOk;
            CancelButton = _btnCancel;
        }

        private void PopulateFields()
        {
            _fieldLabels.Clear();
            _fieldNamesByIndex.Clear();

            _fieldLabels.Add("(Sin colorear)");
            _fieldNamesByIndex.Add(null);

            var numeric = new List<string>();
            var other = new List<string>();
            foreach (var name in _layer.FieldNames)
            {
                if (string.IsNullOrEmpty(name)) continue;
                double min, max;
                int count;
                if (_layer.TryGetFieldStats(name, out min, out max, out count))
                    numeric.Add(name);
                else
                    other.Add(name);
            }

            foreach (var name in numeric)
            {
                double min, max;
                int count;
                _layer.TryGetFieldStats(name, out min, out max, out count);
                _fieldLabels.Add(string.Format(CultureInfo.InvariantCulture,
                    "[#] {0}   ({1:G6} .. {2:G6}, N={3})", name, min, max, count));
                _fieldNamesByIndex.Add(name);
            }

            foreach (var name in other)
            {
                _fieldLabels.Add(name + "   (no numerico)");
                _fieldNamesByIndex.Add(name);
            }

            _cmbField.Items.Clear();
            foreach (var lbl in _fieldLabels)
                _cmbField.Items.Add(lbl);

            _cmbField.SelectedIndex = 0;
        }

        private void SelectCurrentField()
        {
            if (string.IsNullOrEmpty(_layer.StyleField)) return;
            for (int i = 0; i < _fieldNamesByIndex.Count; i++)
            {
                if (string.Equals(_fieldNamesByIndex[i], _layer.StyleField, StringComparison.Ordinal))
                {
                    _cmbField.SelectedIndex = i;
                    return;
                }
            }
        }

        private void UpdateInfoLabel()
        {
            int idx = _cmbField.SelectedIndex;
            if (idx <= 0)
            {
                _lblInfo.Text = "El fill usa color uniforme (ver LineColor / FillColor del layer).";
                return;
            }

            string name = _fieldNamesByIndex[idx];
            double min, max;
            int count;
            if (_layer.TryGetFieldStats(name, out min, out max, out count))
            {
                _lblInfo.Text = string.Format(CultureInfo.InvariantCulture,
                    "Gradiente verde→amarillo→rojo sobre {0} valores de {1}.\n"
                    + "Min = {2:G6}   Max = {3:G6}",
                    count, name, min, max);
            }
            else
            {
                _lblInfo.Text = "Campo no numerico: el gradiente no se puede calcular.";
            }
        }

        private void OnApply(object sender, EventArgs e)
        {
            _layer.ShowFill = _chkShowFill.Checked;
            _layer.ShowOutline = _chkShowOutline.Checked;

            int idx = _cmbField.SelectedIndex;
            string fieldName = (idx >= 0 && idx < _fieldNamesByIndex.Count)
                ? _fieldNamesByIndex[idx] : null;

            if (fieldName == null)
            {
                _layer.ApplyColorByField(null);
                return;
            }

            if (!_layer.ApplyColorByField(fieldName))
            {
                MessageBox.Show(this,
                    "El campo '" + fieldName + "' no tiene suficientes valores numericos.\n"
                    + "Se mantiene el estilo anterior.",
                    "Estilo Shapefile",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }
    }
}
