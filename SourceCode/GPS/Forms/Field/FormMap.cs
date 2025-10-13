using AgLibrary.Forms;
using AgLibrary.Logging;
using AgOpenGPS.Core;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Streamers;
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
            gMapControl.ShowCenter = false;
            gMapControl.DragButton = MouseButtons.Left;

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

            cboxDrawMap.Checked = mf.worldGrid.HasBingMap;

            if (mf.worldGrid.HasBingMap) cboxDrawMap.Image = Properties.Resources.MappingOn;
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

        private void gMapControl_OnMapClick(PointLatLng pointClick, MouseEventArgs e)
        {
            if (!cboxEnableLineDraw.Checked)
                return;

            if (polygon.Points.Count == 0)
                overlay.Markers.Clear();

            polygon.Points.Add(pointClick);
            gMapControl.UpdatePolygonLocalPosition(polygon);

            // Create marker instance: specify location on the map, radius and label
            var marker = new GMapMarkerCircle(pointClick, 4f, polygon.Points.Count.ToString());

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

            DialogResult result3 = FormYes.Show(
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

        private GeoCoord GeoCoordFormMapPoint(PointLatLng mapPoint)
        {
            return mf.AppModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84(mapPoint.Lat, mapPoint.Lng));
        }

        // Returns null if bingMap is too big
        private BingMap CreateBingMap()
        {
            BingMap bingMap = null;
            PointLatLng topLeft = gMapControl.ViewArea.LocationTopLeft;
            PointLatLng bottomRight = gMapControl.ViewArea.LocationRightBottom;
            GeoBoundingBox geoBoundingBox = GeoBoundingBox.CreateEmpty();

            geoBoundingBox.Include(GeoCoordFormMapPoint(topLeft));
            geoBoundingBox.Include(GeoCoordFormMapPoint(bottomRight));

            bool tooBig =
                4000 < Math.Abs(geoBoundingBox.MaxNorthing) ||
                4000 < Math.Abs(geoBoundingBox.MinEasting) ||
                4000 < Math.Abs(geoBoundingBox.MinNorthing) ||
                4000 < Math.Abs(geoBoundingBox.MaxEasting);
            if (!tooBig)
            {
                Bitmap bitmap = new Bitmap(gMapControl.Width, gMapControl.Height);
                gMapControl.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

                if (!isColorMap)
                {
                    bitmap = glm.MakeGrayscale3(bitmap);
                }
                bingMap = new BingMap(geoBoundingBox, bitmap);
            }
            return bingMap;
        }

        private void SetAndSaveBingMap(BingMap bingMap)
        {
            mf.worldGrid.BingMap = bingMap;

            BingMapStreamer streamer = new BingMapStreamer();
            streamer.TryWrite(bingMap, mf.AppCore.ActiveField.FieldDirectory);
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
                BingMap bingMap = CreateBingMap();
                if (bingMap == null)
                {
                    mf.TimedMessageBox(2000, "BingMap Error", "Map Too Large");
                    Log.EventWriter("BingMap, Map Too Large");
                }
                SetAndSaveBingMap(bingMap);
            }
            else
            {
                cboxDrawMap.Image = Properties.Resources.MappingOff;
                ResetMapGrid();
            }
        }

        private void ResetMapGrid()
        {
            SetAndSaveBingMap(null);
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
            lblPoints.Text = (polygon.Points.Count > 0) ?
                (gStr.gsPoints + ": " + polygon.Points.Count.ToString()) :
                "";

            if (mf.bnd.bndList.Count == 0)
            {
                lblBnds.Text = gStr.gsNone;
            }
            else
            {
                lblBnds.Text = "1 " + gStr.gsOuter + "\r\n";
                if (1 < mf.bnd.bndList.Count)
                {
                    lblBnds.Text += (mf.bnd.bndList.Count - 1).ToString() + " " + gStr.gsInner;
                }
            }
        }

        private static string PointLatLngToString(PointLatLng point)
        {
            return point.Lat.ToString("N7") + ", " + point.Lng.ToString("N7");
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