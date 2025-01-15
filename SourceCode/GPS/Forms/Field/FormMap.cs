using AgLibrary.Logging;
using AgOpenGPS.Culture;
using AgOpenGPS.Helpers;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormMap : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private bool isClosing;
        private Track bingLine = new Track(new TrackStyle(new Pen(Color.White, 4)));
        private bool isColorMap = true;

        public FormMap(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            mapControl.CacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MapControl");

            ITileServer[] tileServers = new ITileServer[]
            {
                new BingMapsHybridTileServer(),
                new BingMapsRoadsTileServer(),
                new OpenStreetMapTileServer(userAgent: "Map Control AgOpenGPS"),
                new OfflineTileServer(),
                new BingMapsAerialTileServer(),
            };

            cmbTileServers.Items.AddRange(tileServers);
            cmbTileServers.SelectedIndex = 0;
            mapControl.Tracks.Add(bingLine);
        }

        private void FormMap_Load(object sender, EventArgs e)
        {
            Size = Properties.Settings.Default.setWindow_BingMapSize;

            mapControl.ZoomLevel = Properties.Settings.Default.setWindow_BingZoom;//mapControl
            mapControl.Center = new GeoPoint((float)mf.pn.longitude, (float)mf.pn.latitude);

            mapControl.Invalidate();

            if (mf.worldGrid.isGeoMap)
            {
                cboxDrawMap.Checked = true;
            }
            else
            {
                cboxDrawMap.Checked = false;
            }

            if (mf.worldGrid.isGeoMap) cboxDrawMap.Image = Properties.Resources.MappingOn;
            else cboxDrawMap.Image = Properties.Resources.MappingOff;

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }

            btnDeleteAll.Enabled = true;
            label3.Text = gStr.gsBoundary;
        }

        private void FormMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                e.Cancel = true;
                return;
            }

            Properties.Settings.Default.setWindow_BingMapSize = Size;
            Properties.Settings.Default.setWindow_BingZoom = mapControl.ZoomLevel;
            Properties.Settings.Default.Save();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            isClosing = true;
            Close();
        }

        private void UpdateWindowTitle()
        {
            GeoPoint g = mapControl.Mouse;
            this.Text = $"Mouse = {g} / Zoom = {mapControl.ZoomLevel} ";
        }

        private void mapControl_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateWindowTitle();
        }

        private void mapControl_MouseWheel(object sender, MouseEventArgs e)
        {
            UpdateWindowTitle();
        }

        private void btnClearCache_Click(object sender, EventArgs e)
        {
            mapControl.ClearCache(true);
            ActiveControl = mapControl;
        }

        private void cmbTileServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            mapControl.TileServer = cmbTileServers.SelectedItem as ITileServer;
            ActiveControl = mapControl;
        }

        //private void mapControl_DrawMarker(object sender, DrawMarkerEventArgs e)
        //{
        //    e.Handled = true;
        //    e.Graphics.DrawImage(imageMarker, new Rectangle((int)e.Point.X - 12, (int)e.Point.Y - 24, 24, 24));
        //    if (mapControl.ZoomLevel >= 5)
        //    {
        //        e.Graphics.DrawString(e.Marker.Label, SystemFonts.DefaultFont, Brushes.Red, new PointF(e.Point.X, e.Point.Y + 5), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near });
        //    }
        //}

        private void mapControl_DoubleClick(object sender, EventArgs e)
        {
            //var coord = mapControl.Mouse;
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine($"Location: {coord}");
            //MessageBox.Show(sb.ToString(), "Info");
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (bingLine.Count == 0)
            {
                if (mapControl.Markers.Count == 0)
                {
                    mapControl.Markers.Clear();
                    mapControl.Center = new GeoPoint((float)mf.pn.longitude, (float)mf.pn.latitude);

                    // Create marker's location point
                    var point = new GeoPoint((float)mf.pn.longitude, (float)mf.pn.latitude);

                    var style = new MarkerStyle(10);

                    // Create marker instance: specify location on the map, drawing style, and label
                    var marker = new Marker(point, style, "");

                    // Add marker to the map
                    mapControl.Markers.Add(marker);

                    UpdateWindowTitle();
                    mapControl.Invalidate();
                }
                else
                {
                    mapControl.Markers.Clear();
                    mapControl.Center = new GeoPoint((float)mf.pn.longitude, (float)mf.pn.latitude);

                    UpdateWindowTitle();
                    mapControl.Invalidate();
                }
            }
            else
            {
                mapControl.Center = new GeoPoint((float)mf.pn.longitude, (float)mf.pn.latitude);

                UpdateWindowTitle();
                mapControl.Invalidate();
            }
        }

        private void mapControl_Click(object sender, EventArgs e)
        {
            if (cboxEnableLineDraw.Checked)
            {
                if (bingLine.Count == 0) mapControl.Markers.Clear();
                var coord = mapControl.Mouse;
                bingLine.Add(coord);
                mapControl.Invalidate();
                {
                    // Create marker's location point
                    var point = coord;

                    var style = new MarkerStyle(8);

                    // Create marker instance: specify location on the map, drawing style, and label
                    var marker = new Marker(point, style, bingLine.Count.ToString());

                    // Add marker to the map
                    mapControl.Markers.Add(marker);
                    mapControl.Invalidate();
                }
            }
        }

        private void btnDeletePoint_Click(object sender, EventArgs e)
        {
            if (bingLine != null && bingLine.Count > 0)
            {
                string sNum = bingLine.Count.ToString();

                if (bingLine.Count > 0) bingLine.RemoveAt(bingLine.Count - 1);
                foreach (var mark in mapControl.Markers)
                {
                    if (mark.Label == sNum)
                    {
                        mapControl.Markers.Remove(mark);
                        break;
                    }
                }
                // mapControl.Markers.Clear();

                mapControl.Invalidate();
            }
        }

        private void btnAddFence_Click(object sender, EventArgs e)
        {
            if (bingLine.Count > 2)
            {
                CBoundaryList New = new CBoundaryList();
                for (int i = 0; i < bingLine.Count; i++)
                {
                    mf.pn.ConvertWGS84ToLocal(bingLine[i].Latitude, bingLine[i].Longitude, out double nort, out double east);
                    vec3 v = new vec3(east, nort, 0);
                    New.fenceLine.Add(v);
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
            bingLine.Clear();
            mapControl.Markers.Clear();
            mapControl.Invalidate();

            btnAddFence.Enabled = false;
            btnDeletePoint.Enabled = false;
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (bingLine.Count > 0)
            {
                bingLine.Clear();
                mapControl.Markers.Clear();
                mapControl.Invalidate();
                return;
            }

            if (mf.bnd.bndList == null || mf.bnd.bndList.Count == 0)
            {
                mf.TimedMessageBox(2000, gStr.gsBoundary, gStr.gsNoBoundary);
                return;
            }

            DialogResult result3 = MessageBox.Show("Delete Last Field Boundary Made?",
                gStr.gsDeleteForSure,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result3 == DialogResult.Yes)
            {
                int cnt = mf.bnd.bndList.Count;
                mf.bnd.bndList[cnt-1].hdLine?.Clear();
                mf.bnd.bndList.RemoveAt(cnt - 1);

                mf.FileSaveBoundary();
                mf.bnd.BuildTurnLines();
                mf.fd.UpdateFieldBoundaryGUIAreas();
                mf.btnABDraw.Visible = false;
                //clean up line
                mapControl.Markers.Clear();
                bingLine.Clear();
                mapControl.Invalidate();
            }
            else
            {
                mf.TimedMessageBox(1500, gStr.gsNothingDeleted, gStr.gsActionHasBeenCancelled);
            }
            cboxEnableLineDraw.Checked = false;

            //clean up line
            bingLine.Clear();
            mapControl.Markers.Clear();
            mapControl.Invalidate();

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
                bingLine.Clear();
                mapControl.Markers.Clear();
                mapControl.Invalidate();
                Log.EventWriter("Bing Touch Boundary started");
            }
            else
            {
                bingLine.Clear();
                mapControl.Markers.Clear();
                mapControl.Invalidate();

                btnAddFence.Enabled = false;
                btnDeletePoint.Enabled = false;

            }
        }
        private void SaveBackgroundImage()
        {
            if (bingLine.Count > 0)
            {
                mf.TimedMessageBox(3000, gStr.gsBoundary, "Finish Making Boundary or Delete");
                return;
            }

            mf.worldGrid.isGeoMap = true;

            GeoPoint geoRef = mapControl.TopLeft;
            mf.pn.ConvertWGS84ToLocal(geoRef.Latitude, geoRef.Longitude, out double nor, out double eas);
            if (Math.Abs(nor) > 4000 || Math.Abs(eas) > 4000) mf.worldGrid.isGeoMap = false;
            mf.worldGrid.northingMaxGeo = nor;
            mf.worldGrid.eastingMinGeo = eas;

            geoRef = mapControl.BottomRight;
            mf.pn.ConvertWGS84ToLocal(geoRef.Latitude, geoRef.Longitude, out nor, out eas);
            if (Math.Abs(nor) > 4000 || Math.Abs(eas) > 4000) mf.worldGrid.isGeoMap = false;
            mf.worldGrid.northingMinGeo = nor;
            mf.worldGrid.eastingMaxGeo = eas;


            if (!mf.worldGrid.isGeoMap)
            {
                mf.TimedMessageBox(2000, "Map Error", "Map Too Large");
                Log.EventWriter("GeoMap, Map Too Large");
                ResetMapGrid();
                return;
            }

            Bitmap bitmap = new Bitmap(mapControl.Width, mapControl.Height);
            mapControl.DrawToBitmap(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

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

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.bingGrid]);
                BitmapData bitmapData = bitmap.LockBits(new
                    Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0,
                    PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
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

        private void btnBuildFieldBackground_Click(object sender, EventArgs e)
        {
            if (mf.worldGrid.isGeoMap)
            {
                mf.worldGrid.isGeoMap = false;
                ResetMapGrid();
            }
            SaveBackgroundImage();
        }

        private void cboxDrawMap_Click(object sender, EventArgs e)
        {
            if (bingLine.Count > 0)
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
            using (Bitmap bitmap = Properties.Resources.z_bingMap)
            {
                GL.GenTextures(1, out mf.texture[(int)FormGPS.textures.bingGrid]);
                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.bingGrid]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
            }

            string fileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "BackPic.png");
            try
            {
                if (File.Exists(fileAndDirectory))
                    File.Delete(fileAndDirectory);
            }
            catch { }

            mf.worldGrid.isGeoMap = false;
            bingLine.Clear();
            mapControl.Markers.Clear();
            mapControl.Invalidate();
        }

        private void btnGray_Click(object sender, EventArgs e)
        {
            isColorMap = !isColorMap;
            //if (isColorMap)
            //{
            //    btnGray.Image = Properties.Resources.MapColor;
            //}
            //else
            //{
            //    btnGray.Image = Properties.Resources.MapGray;
            //}
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            int zoom = mapControl.ZoomLevel;
            zoom--;
            if (zoom < 12) zoom = 12;
            mapControl.ZoomLevel = zoom;//mapControl
            mapControl.Invalidate();
            UpdateWindowTitle();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            int zoom = mapControl.ZoomLevel;
            zoom++;
            if (zoom > 19) zoom = 19;
            mapControl.ZoomLevel = zoom;//mapControl
            mapControl.Invalidate();
            UpdateWindowTitle();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblBnds.Text = mf.bnd.bndList.Count.ToString();
            if (bingLine.Count > 0)
                lblPoints.Text = "Pts: " + bingLine.Count.ToString();
            else
                lblPoints.Text = "";

            if (mf.bnd.bndList.Count > 1)
            {
                lblBnds.Text = "1 " + gStr.gsOuter +"\r\n" + (mf.bnd.bndList.Count - 1).ToString() + " " + gStr.gsInner;
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

    }
}