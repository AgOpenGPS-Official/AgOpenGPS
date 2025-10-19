using AgLibrary.Logging;
using AgOpenGPS.Controls;
using AgOpenGPS.Core;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Forms;
using AgOpenGPS.Helpers;
using AgOpenGPS.Protocols.ISOBUS;
using AgOpenGPS.Core.Models.Guidance;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace AgOpenGPS
{
    /// <summary>
    /// Form for creating a new field based on an ISO XML "Taskdata.xml".
    /// Tested with ISO XML from Agleader, AGCO Valtra and FendtOne
    /// </summary>
    public partial class FormFieldIsoXml : Form
    {
        private readonly FormGPS mf;
        private XmlDocument iso;
        private string xmlFilename;
        private XmlNodeList pfd;
        private int idxFieldSelected;
        private Wgs84 _origin;

        public FormFieldIsoXml(FormGPS callingForm)
        {
            mf = callingForm;
            InitializeComponent();
        }

        private void FormFieldIsoXml_Load(object sender, EventArgs e)
        {
            tboxFieldName.Text = "";
            btnBuildFields.Enabled = false;
            labelFieldname.Text = gStr.gsEditFieldName;
            this.Text = gStr.gsCreateNewFromIsoXML;
            labelField.Text = gStr.gsBasedOnField;
            tree.Nodes?.Clear();

            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "XML files (*.XML)|*.XML",
                InitialDirectory = RegistrySettings.fieldsDirectory
            };

            if (ofd.ShowDialog() == DialogResult.Cancel)
            {
                Close();
                return;
            }

            xmlFilename = ofd.FileName;
            iso = new XmlDocument { PreserveWhitespace = false };
            iso.Load(xmlFilename);

            pfd = iso.GetElementsByTagName("PFD");
            int index = 0;

            try
            {
                const double SqmToHectares = 0.0001;

                foreach (XmlNode nodePFD in pfd)
                {
                    // 1) Get area in m² (either from attribute D or fallback estimate)
                    double areaSqm;
                    if (double.TryParse(nodePFD.Attributes["D"]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double areaRaw) && areaRaw >= 1)
                    {
                        areaSqm = areaRaw; // still in m²
                    }
                    else
                    {
                        areaSqm = EstimateAreaFromPln(nodePFD, mf.AppModel); // still in m²
                    }

                    // 2) Convert to hectares and round to 2 decimals
                    double areaHa = Math.Round(areaSqm * SqmToHectares, 2, MidpointRounding.AwayFromZero);

                    // 3) Build label (always show 2 decimals)
                    string fieldName = nodePFD.Attributes["C"]?.Value ?? "Unnamed";
                    string fieldLabel = $"{fieldName} Area: {areaHa:0.00} Ha";

                    // 4) Add to tree
                    TreeNode fieldNode = new TreeNode(fieldLabel) { Tag = index++ };
                    tree.Nodes.Add(fieldNode);

                    XmlNodeList fieldParts = nodePFD.ChildNodes;

                    //Parse GGP → GPN → LSG structure (v3-style)
                    foreach (XmlNode nodeGgp in nodePFD.SelectNodes("GGP"))
                    {
                        XmlNode gpn = nodeGgp.SelectSingleNode("GPN");
                        if (gpn == null) continue;

                        string name = nodeGgp.Attributes["B"]?.Value ?? "Unnamed";
                        string type = gpn.Attributes["C"]?.Value ?? "";
                        TreeNode node = null;

                        if (type == "1") // AB
                        {
                            node = new TreeNode("AB: " + name);
                        }
                        else if (type == "2") // A+
                        {
                            node = new TreeNode("A+: " + name);
                        }
                        else if (type == "3") // Curve
                        {
                            node = new TreeNode("Curve: " + name);
                        }

                        if (node != null)
                        {
                            fieldNode.Nodes.Add(node);
                        }
                    }


                    // Parse PLN nodes (v2-style)
                    foreach (XmlNode nodePart in fieldParts)
                    {
                        if (nodePart.Name != "PLN") continue;

                        string name = nodePart.Attributes?["B"]?.Value ?? "Unnamed";
                        string type = nodePart.Attributes?["C"]?.Value ?? "";
                        XmlNodeList pnts = nodePart.SelectNodes("PNT");
                        int pointCount = pnts != null ? pnts.Count : 0;

                        TreeNode lineNode = null;

                        if (type == "1" && pointCount == 2)
                        {
                            lineNode = new TreeNode("AB: " + name);
                        }
                        else if (type == "2" && pointCount == 1)
                        {
                            lineNode = new TreeNode("A+: " + name);
                        }
                        else if (type == "3" && pointCount > 2)
                        {
                            lineNode = new TreeNode("Curve: " + name);
                        }
                        else if (type == "4" && pointCount > 0)
                        {
                            lineNode = new TreeNode("Pivot: " + name);
                        }
                        else if (type == "5" && pointCount > 0)
                        {
                            lineNode = new TreeNode("Spiral: " + name);
                        }

                        if (lineNode != null)
                        {
                            fieldNode.Nodes.Add(lineNode);
                        }
                    }


                    //Parse direct LSG nodes (v3 standalone guidance)
                    foreach (XmlNode nodePart in fieldParts)
                    {
                        if (nodePart.Name != "LSG" || nodePart.Attributes["A"]?.Value != "5") continue;

                        string name = nodePart.Attributes?["B"]?.Value ?? "Unnamed";
                        string type = nodePart.Attributes?["C"]?.Value ?? "";
                        XmlNodeList pnts = nodePart.SelectNodes("PNT");
                        int pointCount = pnts != null ? pnts.Count : 0;

                        TreeNode lineNode = null;

                        if (type == "1" && pointCount == 2)
                        {
                            lineNode = new TreeNode("AB: " + name);
                        }
                        else if (type == "2" && pointCount == 1)
                        {
                            lineNode = new TreeNode("A+: " + name);
                        }
                        else if (type == "3" && pointCount > 2)
                        {
                            lineNode = new TreeNode("Curve: " + name);
                        }
                        else if (type == "4" && pointCount > 0)
                        {
                            lineNode = new TreeNode("Pivot: " + name);
                        }
                        else if (type == "5" && pointCount > 0)
                        {
                            lineNode = new TreeNode("Spiral: " + name);
                        }

                        if (lineNode != null)
                        {
                            fieldNode.Nodes.Add(lineNode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Failed to create new field: " + ex);
                FormDialog.Show(gStr.gsError, ex.ToString(), MessageBoxButtons.OK);
                return;
            }

            btnBuildFields.Enabled = tree.Nodes.Count > 0;
            tree.Sort();

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tree.SelectedNode?.Parent == null)
            {
                idxFieldSelected = (int)tree.SelectedNode.Tag;
            }
            else
            {
                idxFieldSelected = (int)tree.SelectedNode.Parent.Tag;
            }

            bool enabled = (idxFieldSelected >= 0);

            if (enabled)
            {
                string fieldName = pfd[idxFieldSelected].Attributes["C"].Value;
                labelField.Text = $"{idxFieldSelected} {fieldName}";
                tboxFieldName.Text = fieldName;
            }

            btnBuildFields.Enabled = enabled;
            btnAddDate.Enabled = enabled;
            btnAddTime.Enabled = enabled;
            tboxFieldName.Enabled = enabled;

        }

        private void btnBuildFields_Click(object sender, EventArgs e)
        {
            mf.currentFieldDirectory = tboxFieldName.Text.Trim();
            string directoryPath = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);

            if (Directory.Exists(directoryPath))
            {
                FormDialog.Show(gStr.gsDirectoryExists, gStr.gsChooseADifferentName, MessageBoxButtons.OK);
                mf.currentFieldDirectory = "";
                return;
            }

            var fieldParts = pfd[idxFieldSelected].ChildNodes;
            var importer = new IsoXmlFieldImporter(fieldParts, mf.AppModel);

            if (!importer.TryGetOrigin(out _origin))
            {
                mf.YesMessageBox("Can't calculate center of field. Missing Outer Boundary or AB line.");
                return;
            }

            mf.JobNew();
            mf.pn.DefineLocalPlane(_origin, true);

            List<CBoundaryList> boundaries = importer.GetBoundaries();
            foreach (var bnd in boundaries)
            {
                mf.bnd.bndList.Add(bnd);
                int idx = mf.bnd.bndList.Count - 1;
                bnd.CalculateFenceArea(idx);
                bnd.FixFenceLine(idx);
            }

            List<vec3> headland = importer.GetHeadland();
            if (headland.Count > 0 && mf.bnd.bndList.Count > 0 && mf.bnd.bndList[0].hdLine.Count == 0)
            {
                mf.bnd.bndList[0].hdLine.AddRange(headland);
            }

            List<Track> guidanceLines = importer.GetGuidanceLines();
            // Add tracks directly via _trackService (already Track type from importer)
            foreach (var track in guidanceLines)
            {
                mf._trackService.AddTrack(track);
            }

            SaveFieldFiles(directoryPath);
            FinalizeField();

            if (mf.bnd.bndList.Count > 0)
                mf.btnABDraw.Visible = true;

            mf.FieldMenuButtonEnableDisable(
                mf.bnd.bndList.Count > 0 && mf.bnd.bndList[0].hdLine.Count > 0
            );

            DialogResult = DialogResult.OK;
            Close();
        }



        private void tboxFieldName_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxSender = (TextBox)sender;
            int cursorPosition = textBoxSender.SelectionStart;
            textBoxSender.Text = Regex.Replace(textBoxSender.Text, glm.fileRegex, "");
            textBoxSender.SelectionStart = cursorPosition;
        }

        private void tboxFieldName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                ((TextBox)sender).ShowKeyboard(this);
                btnSerialCancel.Focus();
            }
        }

        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddDate_Click(object sender, EventArgs e)
        {
            tboxFieldName.Text += " " + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            tboxFieldName.Text += " " + DateTime.Now.ToString("HH-mm", CultureInfo.InvariantCulture);
        }

        // Save field files after job is started
        public void SaveFieldFiles(string directoryPath)
        {
            string fieldFile = Path.Combine(directoryPath, "Field.txt");

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            using (StreamWriter writer = new StreamWriter(fieldFile))
            {
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));
                writer.WriteLine("$FieldDir");
                writer.WriteLine("XML Derived");
                writer.WriteLine("$Offsets");
                writer.WriteLine("0,0");
                writer.WriteLine("Convergence");
                writer.WriteLine("0");
                writer.WriteLine("StartFix");
                writer.WriteLine(_origin.Latitude.ToString(CultureInfo.InvariantCulture) + "," +
                                 _origin.Longitude.ToString(CultureInfo.InvariantCulture));
            }

            mf.FileCreateSections();
            mf.FileCreateRecPath();
            mf.FileCreateContour();
            mf.FileCreateElevation();
            mf.FileSaveFlags();
        }


        public void FinalizeField()
        {
            mf.FileSaveBoundary();
            mf.bnd.BuildTurnLines();
            mf.fd.UpdateFieldBoundaryGUIAreas();
            mf.CalculateMinMax();
            mf.FileSaveHeadland();
            mf.FileSaveTracks();
        }
        private static double EstimateAreaFromPln(XmlNode nodePfd, ApplicationModel appModel)
        {
            // Find PLN with type "1" (outer boundary)
            foreach (XmlNode nodePln in nodePfd.SelectNodes("PLN"))
            {
                if (nodePln.Attributes["A"]?.Value != "1") continue;

                XmlNode lsg = nodePln.SelectSingleNode("LSG[@A='1']");
                if (lsg == null) continue;

                var pts = lsg.SelectNodes("PNT");
                if (pts.Count < 3) continue;

                var vecs = new vec2[pts.Count];
                for (int i = 0; i < pts.Count; i++)
                {
                    double lat, lon;
                    if (!double.TryParse(pts[i].Attributes["C"]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lat)) continue;
                    if (!double.TryParse(pts[i].Attributes["D"]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lon)) continue;

                    GeoCoord geo = appModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84(lat, lon));
                    vecs[i] = new vec2(geo.Easting, geo.Northing);
                }

                // Shoelace formula to compute area (in m²)
                double area = 0;
                for (int i = 0, j = vecs.Length - 1; i < vecs.Length; j = i++)
                {
                    area += (vecs[j].easting + vecs[i].easting) * (vecs[j].northing - vecs[i].northing);
                }
                return Math.Abs(area / 2.0); // m²
            }

            return 0.0;
        }
    }
}
