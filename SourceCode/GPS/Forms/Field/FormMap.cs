using AgLibrary.Logging;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Forms;
using AgOpenGPS.Helpers;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormMap : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private bool isClosing;
        private GMapPolygon polygon;
        private GMapOverlay overlay = new GMapOverlay();
        private Point lastMouseLocation;
        private bool isColorMap = true;

        public FormMap(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            //translate all the controls
            this.Text = gStr.gsMapForBackground;
            labelNewBoundary.Text = gStr.gsNew + " " + gStr.gsBoundary;
            labelBoundary.Text = gStr.gsBoundary;
            lblPoints.Text = gStr.gsPoints + ":";
            labelBackground.Text = gStr.gsBackground;

            gMapControl.MapProvider = GMapProviders.BingHybridMap;

            polygon = new GMapPolygon(new List<PointLatLng>(), "bingLine")
            {
                Fill = Brushes.Transparent,
                Stroke = new Pen(Color.White, 4f) { LineJoin = LineJoin.Round }
            };
            overlay.Polygons.Add(polygon);
            gMapControl.Overlays.Add(overlay);
        }

        private void FormMap_Load(object sender, EventArgs e)
        {
            Size = Properties.Settings.Default.setWindow_BingMapSize;

            gMapControl.Zoom = Properties.Settings.Default.setWindow_BingZoom;
            gMapControl.Position = new PointLatLng(
                mf.AppModel.CurrentLatLon.Latitude,
                mf.AppModel.CurrentLatLon.Longitude);

            cboxDrawMap.Checked = mf.worldGrid.isGeoMap;

            if (mf.worldGrid.isGeoMap) cboxDrawMap.Image = Properties.Resources.MappingOn;
            else cboxDrawMap.Image = Properties.Resources.MappingOff;

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }

            btnDeleteAll.Enabled = true;
        }

        private void FormMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                e.Cancel = true;
                return;
            }
            Properties.Settings.Default.setWindow_BingMapSize = Size;
            Properties.Settings.Default.setWindow_BingZoom = (int)gMapControl.Zoom;
            Properties.Settings.Default.Save();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            isClosing = true;
            Close();
        }

        private void UpdateWindowTitle()
        {
            PointLatLng pos = gMapControl.FromLocalToLatLng(lastMouseLocation.X, lastMouseLocation.Y);
            Text = $"Mouse = {PointLatLngToString(pos)} / Zoom = {gMapControl.Zoom} ";
        }

        private void gMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            lastMouseLocation = e.Location;
            UpdateWindowTitle();
        }

        private void gMapControl_MouseWheel(object sender, MouseEventArgs e)
        {
            UpdateWindowTitle();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            gMapControl.Position = new PointLatLng(
                mf.AppModel.CurrentLatLon.Latitude,
                mf.AppModel.CurrentLatLon.Longitude);
            if (polygon.Points.Count == 0)
            {
                overlay.Markers.Clear();

                // Create marker's location point
                var point = new PointLatLng(
                    mf.AppModel.CurrentLatLon.Latitude,
                    mf.AppModel.CurrentLatLon.Longitude);

                // Create marker instance: specify location on the map and radius
                var marker = new GMapMarkerCircle(point, 5f);

                // Add marker to the map
                overlay.Markers.Add(marker);
            }
            UpdateWindowTitle();
        }

        private void gMapControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (!cboxEnableLineDraw.Checked)
                return;

            if (polygon.Points.Count == 0)
                overlay.Markers.Clear();

            var coord = gMapControl.FromLocalToLatLng(e.Location.X, e.Location.Y);
            polygon.Points.Add(coord);
            gMapControl.UpdatePolygonLocalPosition(polygon);

            // Create marker instance: specify location on the map, radius and label
            var marker = new GMapMarkerCircle(coord, 4f, polygon.Points.Count.ToString());

            // Add marker to the map
            overlay.Markers.Add(marker);
        }

        private void btnDeletePoint_Click(object sender, EventArgs e)
        {
            if (polygon.Points.Count == 0)
                return;

            string sNum = polygon.Points.Count.ToString();

            polygon.Points.RemoveAt(polygon.Points.Count - 1);
            gMapControl.UpdatePolygonLocalPosition(polygon);

            foreach (var marker in overlay.Markers.OfType<GMapMarkerCircle>())
            {
                if (marker.Label == sNum)
                {
                    overlay.Markers.Remove(marker);
                    break;
                }
            }
        }

        private void btnAddFence_Click(object sender, EventArgs e)
        {
            if (polygon.Points.Count > 2)
            {
                CBoundaryList New = new CBoundaryList();
                foreach (var point in polygon.Points)
                {
                    GeoCoord geoCoord = mf.AppModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84(point.Lat, point.Lng));
                    New.fenceLine.Add(new vec3(geoCoord));
                }

                New.CalculateFenceArea(mf.bnd.bndList.Count);
                New.FixFenceLine(mf.bnd.bndList.Count);

                mf.bnd.bndList.Add(New);
                mf.fd.UpdateFieldBoundaryGUIAreas();

                //turn lines made from boundaries
                mf.CalculateMinMax();
                mf.FileSaveBoundary();
                mf.bnd.BuildTurnLines();
                mf.btnABDraw.Visible = true;
            }

            cboxEnableLineDraw.Checked = false;

            //clean up line
            ResetPolygonAndMarkers();

            btnAddFence.Enabled = false;
            btnDeletePoint.Enabled = false;
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (polygon.Points.Count > 0)
            {
                ResetPolygonAndMarkers();
                return;
            }

            if (mf.bnd.bndList == null || mf.bnd.bndList.Count == 0)
            {
                mf.TimedMessageBox(2000, gStr.gsBoundary, gStr.gsNoBoundary);
                return;
            }

            DialogResult result3 = FormDialog.Show(
                gStr.gsDeleteForSure,
                "Delete Last Field Boundary Made?",
                MessageBoxButtons.YesNo);


            if (result3 == DialogResult.OK)
            {
                int cnt = mf.bnd.bndList.Count;
                mf.bnd.bndList[cnt - 1].hdLine?.Clear();
                mf.bnd.bndList.RemoveAt(cnt - 1);

                mf.FileSaveBoundary();
                mf.bnd.BuildTurnLines();
                mf.fd.UpdateFieldBoundaryGUIAreas();
                mf.btnABDraw.Visible = false;
            }
            else
            {
                mf.TimedMessageBox(1500, gStr.gsNothingDeleted, gStr.gsActionHasBeenCancelled);
            }
            cboxEnableLineDraw.Checked = false;

            //clean up line
            ResetPolygonAndMarkers();

            btnAddFence.Enabled = false;
            btnDeletePoint.Enabled = false;
        }

        private void cboxEnableLineDraw_Click(object sender, EventArgs e)
        {
            if (cboxEnableLineDraw.Checked)
            {
                mf.TimedMessageBox(3000, "Boundary Create Mode", "Touch Map to Create The Boundary");
                btnAddFence.Enabled = true;
                btnDeletePoint.Enabled = true;
                Log.EventWriter("Bing Touch Boundary started");
            }
            else
            {
                btnAddFence.Enabled = false;
                btnDeletePoint.Enabled = false;
            }

            ResetPolygonAndMarkers();
        }

        private void SaveBackgroundImage()
        {
            if (polygon.Points.Count > 0)
            {
                mf.TimedMessageBox(3000, gStr.gsBoundary, "Finish Making Boundary or Delete");
                return;
            }

            PointLatLng geoRef = gMapControl.ViewArea.LocationTopLeft;
            GeoCoord topLeftGeoCoord = mf.AppModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84(geoRef.Lat, geoRef.Lng));
            mf.worldGrid.northingMaxGeo = topLeftGeoCoord.Northing;
            mf.worldGrid.eastingMinGeo = topLeftGeoCoord.Easting;

            geoRef = gMapControl.ViewArea.LocationRightBottom;
            GeoCoord bottomRightGeoCoord = mf.AppModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84(geoRef.Lat, geoRef.Lng));
            mf.worldGrid.northingMinGeo = bottomRightGeoCoord.Northing;
            mf.worldGrid.eastingMaxGeo = bottomRightGeoCoord.Easting;

            mf.worldGrid.isGeoMap =
                Math.Abs(mf.worldGrid.northingMaxGeo) <= 4000 &&
                Math.Abs(mf.worldGrid.eastingMinGeo) <= 4000 &&
                Math.Abs(mf.worldGrid.northingMinGeo) <= 4000 &&
                Math.Abs(mf.worldGrid.eastingMaxGeo) <= 4000;

            if (!mf.worldGrid.isGeoMap)
            {
                mf.TimedMessageBox(2000, "Map Error", "Map Too Large");
                Log.EventWriter("GeoMap, Map Too Large");
                ResetMapGrid();
                return;
            }

            Bitmap bitmap = new Bitmap(gMapControl.Width, gMapControl.Height);
            gMapControl.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

            if (!isColorMap)
            {
                bitmap = glm.MakeGrayscale3(bitmap);
            }

            string fileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "BackPic.png");
            try
            {
                if (File.Exists(fileAndDirectory))
                    File.Delete(fileAndDirectory);
                bitmap.Save(fileAndDirectory, ImageFormat.Png);
                mf.worldGrid.BingBitmap = bitmap;
            }
            catch
            {
                mf.TimedMessageBox(2000, "File in Use", "Try loading again");
                Log.EventWriter("GeoMap File in Use, Try Reload");
                return;
            }
            SaveBackgroundGeoFile();
        }

        private void SaveBackgroundGeoFile()
        {
            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //write out the file
            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "BackPic.Txt")))
            {
                writer.WriteLine("$BackPic");
                //outer track of outer boundary tram
                if (mf.worldGrid.isGeoMap)
                {
                    writer.WriteLine(true);
                    writer.WriteLine(mf.worldGrid.eastingMaxGeo.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(mf.worldGrid.eastingMinGeo.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(mf.worldGrid.northingMaxGeo.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(mf.worldGrid.northingMinGeo.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    writer.WriteLine(false);
                    writer.WriteLine(300);
                    writer.WriteLine(-300);
                    writer.WriteLine(300);
                    writer.WriteLine(-300);
                }
            }
        }

        private void cboxDrawMap_Click(object sender, EventArgs e)
        {
            if (polygon.Points.Count > 0)
            {
                mf.TimedMessageBox(2000, gStr.gsBoundary, "Finish Making Boundary");
                cboxDrawMap.Checked = !cboxDrawMap.Checked;
                return;
            }

            if (cboxDrawMap.Checked)
            {
                cboxDrawMap.Image = Properties.Resources.MappingOn;
                SaveBackgroundImage();
            }
            else
            {
                cboxDrawMap.Image = Properties.Resources.MappingOff;
                ResetMapGrid();
                mf.worldGrid.isGeoMap = false;
                SaveBackgroundGeoFile();
            }

        }

        private void ResetMapGrid()
        {
            mf.worldGrid.BingBitmap = Properties.Resources.z_bingMap;
            string fileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "BackPic.png");
            try
            {
                if (File.Exists(fileAndDirectory))
                    File.Delete(fileAndDirectory);
            }
            catch { }

            mf.worldGrid.isGeoMap = false;
            ResetPolygonAndMarkers();
        }

        private void ResetPolygonAndMarkers()
        {
            polygon.Points.Clear();
            gMapControl.UpdatePolygonLocalPosition(polygon);
            overlay.Markers.Clear();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            int zoom = (int)gMapControl.Zoom;
            zoom--;
            if (zoom < 12) zoom = 12;
            gMapControl.Zoom = zoom;//mapControl
            UpdateWindowTitle();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            int zoom = (int)gMapControl.Zoom;
            zoom++;
            if (zoom > 19) zoom = 19;
            gMapControl.Zoom = zoom;//mapControl
            UpdateWindowTitle();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblBnds.Text = mf.bnd.bndList.Count.ToString();
            if (polygon.Points.Count > 0)
                lblPoints.Text = "Pts: " + polygon.Points.Count.ToString();
            else
                lblPoints.Text = "";

            if (mf.bnd.bndList.Count > 1)
            {
                lblBnds.Text = "1 " + gStr.gsOuter + "\r\n" + (mf.bnd.bndList.Count - 1).ToString() + " " + gStr.gsInner;
            }
            else if (mf.bnd.bndList.Count == 1)
            {
                lblBnds.Text = "1 Outer\r\n";
            }
            else
            {
                lblBnds.Text = gStr.gsNone;
            }
        }

        private static string PointLatLngToString(PointLatLng point)
        {
            return $"{DegreeToString(point.Lng, "W", "E")}, {DegreeToString(point.Lat, "S", "N")}";
        }

        private static string DegreeToString(double coordinate, string negativeSym, string positiveSym)
        {
            string sym = (coordinate < 0.0) ? negativeSym : positiveSym;
            coordinate = Math.Abs(coordinate);
            double d = Math.Floor(coordinate);
            coordinate -= d;
            coordinate *= 60;
            double m = Math.Floor(coordinate);
            coordinate -= m;
            coordinate *= 60;
            double s = coordinate;
            string dd = d.ToString();
            string mm = m.ToString().PadLeft(2, '0');
            string ss = s.ToString("00.00", CultureInfo.InvariantCulture);
            return $"{dd}° {mm}' {ss}\" {sym}";
        }

        private class GMapMarkerCircle : GMapMarker
        {
            private readonly float _radius;
            private readonly Brush _brush = Brushes.Red;
            private readonly Font _font = SystemFonts.DefaultFont;
            private readonly Brush _labelBrush = Brushes.Black;

            public GMapMarkerCircle(PointLatLng pos, float radius, string label = null)
                : base(pos)
            {
                _radius = radius;
                Label = label;
            }

            public string Label { get; }

            public override void OnRender(Graphics g)
            {
                g.FillEllipse(_brush, LocalPosition.X - _radius, LocalPosition.Y - _radius, 2 * _radius, 2 * _radius);

                if (Label != null)
                {
                    g.DrawString(Label, _font, _labelBrush, LocalPosition.X + _radius * 0.7f, LocalPosition.Y + _radius * 0.7f);
                }
            }
        }
    }
}