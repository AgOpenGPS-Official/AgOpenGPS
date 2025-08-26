using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Text;
using AgLibrary.Logging;
using AgOpenGPS.Protocols.ISOBUS;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Classes.IO;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        //list of the list of patch data individual triangles for field sections
        public List<List<vec3>> patchSaveList = new List<List<vec3>>();

        //list of the list of patch data individual triangles for contour tracking
        public List<List<vec3>> contourSaveList = new List<List<vec3>>();

        // NOTE: Add this using at the top of your Form file:
        // using AgOpenGPS.Classes.IO;

        #region File IO (per filetype) — updated to AgOpenGPS.Classes.IO helpers

        // --- Keep this helper as-is ---
        private string GetFieldDir()
        {
            return Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);
        }

        /// <summary>
        /// Open a field from Field.txt using per-filetype loaders and refresh the UI.
        /// </summary>
        public void FileOpenField(string openType)
        {
            // If a job is running, persist before switching field
            if (isJobStarted) _ = FileSaveEverythingBeforeClosingField();

            // Resolve file path to Field.txt based on openType
            string fileAndDirectory = "Cancel";
            if (!string.IsNullOrEmpty(openType) && openType.Contains("Field.txt"))
            {
                fileAndDirectory = openType;
                openType = "Load";
            }

            switch (openType)
            {
                case "Resume":
                    fileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "Field.txt");
                    if (!File.Exists(fileAndDirectory)) fileAndDirectory = "Cancel";
                    break;

                case "Open":
                    using (var ofd = new OpenFileDialog())
                    {
                        ofd.InitialDirectory = RegistrySettings.fieldsDirectory;
                        ofd.RestoreDirectory = true;
                        ofd.Filter = "Field files (Field.txt)|Field.txt";
                        fileAndDirectory = (ofd.ShowDialog(this) == DialogResult.Cancel) ? "Cancel" : ofd.FileName;
                    }
                    break;
            }

            if (fileAndDirectory == "Cancel") return;

            // Set current field directory from chosen file
            currentFieldDirectory = new DirectoryInfo(Path.GetDirectoryName(fileAndDirectory)).Name;
            var dir = GetFieldDir();

            // --- Local plane from Field.txt (or fallback to current GNSS)
            try
            {
                var localPlane = FieldPlaneFiles.LoadLocalPlaneOrDefault(dir, AppModel.CurrentLatLon);
                pn.DefineLocalPlane(localPlane.Origin, true);   // keep existing side effects
                AppModel.LocalPlane = localPlane;               // keep current model in sync
            }
            catch (Exception e)
            {
                Log.EventWriter("While Opening Field (LocalPlane) " + e);
                TimedMessageBox(2000, gStr.gsFieldFileIsCorrupt, gStr.gsChooseADifferentField);
                return;
            }

            // Start a clean job
            this.JobNew();

            // ---------------- Tracks ----------------
            trk.gArr?.Clear();
            var tracks = TrackFiles.Load(dir);
            trk.gArr.AddRange(tracks);
            trk.idx = -1;

            // ---------------- Sections ----------------
            // Parse patches + total area, then map into triStrip like legacy
            var sections = SectionsFiles.Load(dir);
            fd.workedAreaTotal = 0;
            if (triStrip != null && triStrip.Count > 0 && triStrip[0] != null)
            {
                if (triStrip[0].patchList == null) triStrip[0].patchList = new List<List<vec3>>();
                triStrip[0].patchList.Clear();
                foreach (var patch in sections.Patches)
                {
                    triStrip[0].triangleList = new List<vec3>(patch);
                    triStrip[0].patchList.Add(triStrip[0].triangleList);
                }
            }

            // ---------------- Contour ----------------
            var contourPatches = ContourFiles.Load(dir);
            ct.stripList?.Clear();
            foreach (var patch in contourPatches)
            {
                ct.ptList = new List<vec3>(patch);
                ct.stripList.Add(ct.ptList);
            }

            // ---------------- Flags ----------------
            flagPts?.Clear();
            var flags = FlagsFiles.Load(dir);
            flagPts.AddRange(flags);

            // ---------------- Boundaries + Headlands ----------------
            LoadBoundariesAndHeadlands();

            // Post-processing like before
            try
            {
                CalculateMinMax();
                bnd.BuildTurnLines();
            }
            catch
            {
                // keep soft-fail like legacy
            }

            btnABDraw.Visible = bnd.bndList.Count > 0;

            if (bnd.bndList.Count > 0 && bnd.bndList[0].hdLine.Count > 0)
            {
                bnd.isHeadlandOn = true;
                btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
                btnHeadlandOnOff.Visible = true;
                btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
            }
            else
            {
                bnd.isHeadlandOn = false;
                btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;
                btnHeadlandOnOff.Visible = false;
            }
            int sett = Properties.Settings.Default.setArdMac_setting0;
            btnHydLift.Visible = (((sett & 2) == 2) && bnd.isHeadlandOn);

            // ---------------- Tram ----------------
            tram.tramBndOuterArr?.Clear();
            tram.tramBndInnerArr?.Clear();
            tram.tramList?.Clear();
            tram.displayMode = 0;
            btnTramDisplayMode.Visible = false;

            var tramData = TramFiles.Load(dir);
            tram.tramBndOuterArr.AddRange(tramData.Outer);
            tram.tramBndInnerArr.AddRange(tramData.Inner);
            tram.tramList.AddRange(tramData.Lines);
            if (tram.tramBndOuterArr.Count > 0) tram.displayMode = 1;
            try { FixTramModeButton(); } catch { }

            // ---------------- RecPath ----------------
            recPath.recList.Clear();
            var rec = RecPathFiles.Load(dir, "RecPath.txt");
            recPath.recList.AddRange(rec);
            panelDrag.Visible = recPath.recList.Count > 0;

            // ---------------- Background image ----------------
            worldGrid.isGeoMap = false;
            var back = BackPicFiles.Load(dir);
            worldGrid.isGeoMap = back.IsGeoMap;
            if (worldGrid.isGeoMap)
            {
                worldGrid.eastingMaxGeo = back.EastingMax;
                worldGrid.eastingMinGeo = back.EastingMin;
                worldGrid.northingMaxGeo = back.NorthingMax;
                worldGrid.northingMinGeo = back.NorthingMin;

                if (!string.IsNullOrEmpty(back.ImagePathPng) && File.Exists(back.ImagePathPng))
                {
                    try
                    {
                        using (var img = Image.FromFile(back.ImagePathPng)) // dispose source image
                        {
                            worldGrid.BingBitmap = new Bitmap(img); // detach from file
                        }
                    }
                    catch
                    {
                        worldGrid.isGeoMap = false; // soft-fail like legacy
                    }
                }
                else
                {
                    worldGrid.isGeoMap = false;
                }
            }

            // ---------------- Final UI refresh ----------------
            PanelsAndOGLSize();
            SetZoom();
            oglZoom.Refresh();
        }

        /// <summary>
        /// Load boundaries and optionally attach headlands; update UI toggles.
        /// </summary>
        private void LoadBoundariesAndHeadlands()
        {
            var dir = GetFieldDir();

            // Boundaries
            var boundaries = BoundaryFiles.Load(dir);
            bnd.bndList?.Clear();
            bnd.bndList.AddRange(boundaries);

            // Headlands attached if present
            HeadlandFiles.AttachLoad(dir, bnd.bndList);

            // Preserve post-processing (turn lines, min-max, UI)
            CalculateMinMax();
            bnd.BuildTurnLines();
            btnABDraw.Visible = bnd.bndList.Count > 0;

            // Headland UI toggles
            if (bnd.bndList.Count > 0 && bnd.bndList[0].hdLine.Count > 0)
            {
                bnd.isHeadlandOn = true;
                btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
                btnHeadlandOnOff.Visible = true;
                btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
            }
            else
            {
                bnd.isHeadlandOn = false;
                btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;
                btnHeadlandOnOff.Visible = false;
            }

            int sett = Properties.Settings.Default.setArdMac_setting0;
            btnHydLift.Visible = (((sett & 2) == 2) && bnd.isHeadlandOn);
        }

        /// <summary>
        /// Save HeadLines (CHeadPath).
        /// </summary>
        public void FileSaveHeadLines()
        {
            var dir = GetFieldDir();
            HeadlinesFiles.Save(dir, hdl.tracksArr);
        }

        /// <summary>
        /// Load HeadLines (CHeadPath).
        /// </summary>
        public void FileLoadHeadLines()
        {
            hdl.tracksArr?.Clear();
            var list = HeadlinesFiles.Load(GetFieldDir());
            hdl.tracksArr.AddRange(list);
            hdl.idx = -1;
        }

        /// <summary>
        /// Save tracks (AB + Curve).
        /// </summary>
        public void FileSaveTracks()
        {
            var dir = GetFieldDir();
            TrackFiles.Save(dir, trk.gArr);
        }

        /// <summary>
        /// Load tracks (AB + Curve).
        /// </summary>
        public void FileLoadTracks()
        {
            var dir = GetFieldDir();
            var tracks = TrackFiles.Load(dir);

            trk.gArr?.Clear();
            trk.gArr.AddRange(tracks);
            trk.idx = -1;
        }

        /// <summary>
        /// Create Field.txt for a new field session.
        /// </summary>
        public void FileCreateField()
        {
            if (!isJobStarted)
            {
                TimedMessageBox(3000, gStr.gsFieldNotOpen, gStr.gsCreateNewField);
                return;
            }

            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName))) Directory.CreateDirectory(directoryName);

            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "Field.txt")))
            {
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));
                writer.WriteLine("$FieldDir");
                writer.WriteLine("FieldNew");
                writer.WriteLine("$Offsets");
                writer.WriteLine("0,0");
                writer.WriteLine("Convergence");
                writer.WriteLine("0");
                writer.WriteLine("StartFix");
                writer.WriteLine(
                    AppModel.CurrentLatLon.Latitude.ToString(CultureInfo.InvariantCulture) + "," +
                    AppModel.CurrentLatLon.Longitude.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Create Elevation.txt header.
        /// </summary>
        public void FileCreateElevation()
        {
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName))) Directory.CreateDirectory(directoryName);

            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "Elevation.txt")))
            {
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));
                writer.WriteLine("$FieldDir");
                writer.WriteLine("Elevation");
                writer.WriteLine("$Offsets");
                writer.WriteLine("0,0");
                writer.WriteLine("Convergence");
                writer.WriteLine("0");
                writer.WriteLine("StartFix");
                writer.WriteLine(
                    AppModel.CurrentLatLon.Latitude.ToString(CultureInfo.InvariantCulture) + "," +
                    AppModel.CurrentLatLon.Longitude.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("Latitude,Longitude,Elevation,Quality,Easting,Northing,Heading,Roll");
            }
        }

        /// <summary>
        /// Append Section triangle-strips if pending.
        /// </summary>
        public void FileSaveSections()
        {
            if (patchSaveList.Count > 0)
            {
                SectionsFiles.Append(GetFieldDir(), patchSaveList);
                patchSaveList.Clear();
            }
        }

        /// <summary>
        /// Create/overwrite empty Sections.txt.
        /// </summary>
        public void FileCreateSections()
        {
            SectionsFiles.CreateEmpty(GetFieldDir());
        }

        /// <summary>
        /// Create/overwrite Boundary.txt header (legacy-compatible).
        /// </summary>
        public void FileCreateBoundary()
        {
            // No dedicated "create" helper needed; write legacy header directly.
            var path = Path.Combine(GetFieldDir(), "Boundary.txt");
            Directory.CreateDirectory(GetFieldDir());
            File.WriteAllText(path, "$Boundary" + Environment.NewLine);
        }

        /// <summary>
        /// Create Flags.txt header and zero count.
        /// </summary>
        public void FileCreateFlags()
        {
            // Use saver to ensure consistent header/format
            FlagsFiles.Save(GetFieldDir(), new List<CFlag>(0));
        }

        /// <summary>
        /// Create/overwrite Contour.txt with header.
        /// </summary>
        public void FileCreateContour()
        {
            ContourFiles.CreateFile(GetFieldDir());
        }

        /// <summary>
        /// Append contour patches if pending.
        /// </summary>
        public void FileSaveContour()
        {
            if (contourSaveList.Count > 0)
            {
                ContourFiles.Append(GetFieldDir(), contourSaveList);
                contourSaveList.Clear();
            }
        }

        /// <summary>
        /// Save boundaries.
        /// </summary>
        public void FileSaveBoundary()
        {
            BoundaryFiles.Save(GetFieldDir(), bnd.bndList);
        }

        /// <summary>
        /// Save tram data.
        /// </summary>
        public void FileSaveTram()
        {
            TramFiles.Save(
                GetFieldDir(),
                tram.tramBndOuterArr,
                tram.tramBndInnerArr,
                tram.tramList);
        }

        /// <summary>
        /// Save headland(s) attached to boundaries.
        /// </summary>
        public void FileSaveHeadland()
        {
            HeadlandFiles.Save(GetFieldDir(), bnd.bndList);
        }

        /// <summary>
        /// Create RecPath with header + zero count (legacy-compatible).
        /// </summary>
        public void FileCreateRecPath()
        {
            var dir = GetFieldDir();
            Directory.CreateDirectory(dir);
            using (var writer = new StreamWriter(Path.Combine(dir, "RecPath.txt")))
            {
                writer.WriteLine("$RecPath");
                writer.WriteLine("0");
            }
        }

        /// <summary>
        /// Save the recorded path.
        /// </summary>
        public void FileSaveRecPath(string name = "RecPath.Txt")
        {
            RecPathFiles.Save(GetFieldDir(), recPath.recList, name);
        }

        /// <summary>
        /// Load RecPath.txt.
        /// </summary>
        public void FileLoadRecPath()
        {
            recPath.recList.Clear();
            var list = RecPathFiles.Load(GetFieldDir(), "RecPath.txt");
            recPath.recList.AddRange(list);
            panelDrag.Visible = recPath.recList.Count > 0;
        }

        /// <summary>
        /// Save flags.
        /// </summary>
        public void FileSaveFlags()
        {
            FlagsFiles.Save(GetFieldDir(), flagPts);
        }

        /// <summary>
        /// Load flags into flagPts.
        /// </summary>
        private void LoadFlags()
        {
            var flags = FlagsFiles.Load(GetFieldDir());
            flagPts?.Clear();
            flagPts.AddRange(flags);
        }

        /// <summary>
        /// Append elevation grid lines to Elevation.txt.
        /// </summary>
        public void FileSaveElevation()
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "Elevation.txt"), true))
            {
                writer.Write(sbGrid.ToString());
            }
            sbGrid.Clear();
        }

        #endregion


        //generate KML file from flag
        public void FileSaveSingleFlagKML2(int flagNumber)
        {
            Wgs84 latLon = AppModel.LocalPlane.ConvertGeoCoordToWgs84(flagPts[flagNumber - 1].GeoCoord);

            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Flag.kml";

            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, myFileName)))
            {
                //match new fix to current position

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");
                writer.WriteLine(@"  <Placemark>                                  ");
                writer.WriteLine(@"<Style> <IconStyle>");
                if (flagPts[flagNumber - 1].color == 0)  //red - xbgr
                    writer.WriteLine(@"<color>ff4400ff</color>");
                if (flagPts[flagNumber - 1].color == 1)  //grn - xbgr
                    writer.WriteLine(@"<color>ff44ff00</color>");
                if (flagPts[flagNumber - 1].color == 2)  //yel - xbgr
                    writer.WriteLine(@"<color>ff44ffff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> " + flagNumber.ToString(CultureInfo.InvariantCulture) + @"</name>");
                writer.WriteLine(@"<Point><coordinates> "
                    + latLon.Longitude.ToString(CultureInfo.InvariantCulture) + ","
                    + latLon.Latitude.ToString(CultureInfo.InvariantCulture) + ",0"
                    + @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");
            }
        }

        //generate KML file from flag
        public void FileSaveSingleFlagKML(int flagNumber)
        {

            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Flag.kml";

            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, myFileName)))
            {
                //match new fix to current position

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                writer.WriteLine(@"  <Placemark>                                  ");
                writer.WriteLine(@"<Style> <IconStyle>");
                if (flagPts[flagNumber - 1].color == 0)  //red - xbgr
                    writer.WriteLine(@"<color>ff4400ff</color>");
                if (flagPts[flagNumber - 1].color == 1)  //grn - xbgr
                    writer.WriteLine(@"<color>ff44ff00</color>");
                if (flagPts[flagNumber - 1].color == 2)  //yel - xbgr
                    writer.WriteLine(@"<color>ff44ffff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> " + flagNumber.ToString(CultureInfo.InvariantCulture) + @"</name>");
                writer.WriteLine(@"<Point><coordinates> " +
                                flagPts[flagNumber - 1].longitude.ToString(CultureInfo.InvariantCulture) + "," + flagPts[flagNumber - 1].latitude.ToString(CultureInfo.InvariantCulture) + ",0" +
                                @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }

        //generate KML file from flag
        public void FileMakeKMLFromCurrentPosition(Wgs84 currentLatLon)
        {
            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }


            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "CurrentPosition.kml")))
            {

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                writer.WriteLine(@"  <Placemark>                                  ");
                writer.WriteLine(@"<Style> <IconStyle>");
                writer.WriteLine(@"<color>ff4400ff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> Your Current Position </name>");
                writer.WriteLine(@"<Point><coordinates> "
                    + currentLatLon.Longitude.ToString(CultureInfo.InvariantCulture) + ","
                    + currentLatLon.Latitude.ToString(CultureInfo.InvariantCulture) + ",0"
                    + @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }

        //generate KML file from flags
        public void ExportFieldAs_KML()
        {
            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Field.kml";

            XmlTextWriter kml = new XmlTextWriter(Path.Combine(directoryName, myFileName), Encoding.UTF8);

            kml.Formatting = Formatting.Indented;
            kml.Indentation = 3;

            kml.WriteStartDocument();
            kml.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            kml.WriteStartElement("Document");

            //Description  ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Field Stats");
            kml.WriteElementString("description", fd.GetDescription());
            kml.WriteEndElement(); // <Folder>
            //End of Desc

            //Boundary  ----------------------------------------------------------------------
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Boundaries");

            for (int i = 0; i < bnd.bndList.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                if (i == 0) kml.WriteElementString("name", currentFieldDirectory);

                //lineStyle
                kml.WriteStartElement("Style");
                kml.WriteStartElement("LineStyle");
                if (i == 0) kml.WriteElementString("color", "ffdd00dd");
                else kml.WriteElementString("color", "ff4d3ffd");
                kml.WriteElementString("width", "4");
                kml.WriteEndElement(); // <LineStyle>

                kml.WriteStartElement("PolyStyle");
                if (i == 0) kml.WriteElementString("color", "407f3f55");
                else kml.WriteElementString("color", "703f38f1");
                kml.WriteEndElement(); // <PloyStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("Polygon");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("outerBoundaryIs");
                kml.WriteStartElement("LinearRing");

                //coords
                kml.WriteStartElement("coordinates");
                string bndPts = "";
                if (bnd.bndList[i].fenceLine.Count > 3)
                    bndPts = GetBoundaryPointsLatLon(i);
                kml.WriteRaw(bndPts);
                kml.WriteEndElement(); // <coordinates>

                kml.WriteEndElement(); // <Linear>
                kml.WriteEndElement(); // <OuterBoundary>
                kml.WriteEndElement(); // <Polygon>
                kml.WriteEndElement(); // <Placemark>
            }

            kml.WriteEndElement(); // <Folder>  
            //End of Boundary

            //guidance lines AB
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "AB_Lines");
            kml.WriteElementString("visibility", "0");

            string linePts = "";

            foreach (CTrk track in trk.gArr)
            {
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("visibility", "0");

                kml.WriteElementString("name", track.name);
                kml.WriteStartElement("Style");

                kml.WriteStartElement("LineStyle");
                kml.WriteElementString("color", "ff0000ff");
                kml.WriteElementString("width", "2");
                kml.WriteEndElement(); // <LineStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("LineString");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("coordinates");

                GeoCoord pointA = track.ptA.ToGeoCoord();
                GeoDir heading = new GeoDir(track.heading);
                linePts = GetGeoCoordToWgs84_KML(pointA - ABLine.abLength * heading);
                linePts += GetGeoCoordToWgs84_KML(pointA + ABLine.abLength * heading);
                kml.WriteRaw(linePts);

                kml.WriteEndElement(); // <coordinates>
                kml.WriteEndElement(); // <LineString>
                kml.WriteEndElement(); // <Placemark>
            }
            kml.WriteEndElement(); // <Folder>   

            //guidance lines Curve
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Curve_Lines");
            kml.WriteElementString("visibility", "0");

            for (int i = 0; i < trk.gArr.Count; i++)
            {
                linePts = "";
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("visibility", "0");

                kml.WriteElementString("name", trk.gArr[i].name);
                kml.WriteStartElement("Style");

                kml.WriteStartElement("LineStyle");
                kml.WriteElementString("color", "ff6699ff");
                kml.WriteElementString("width", "2");
                kml.WriteEndElement(); // <LineStyle>
                kml.WriteEndElement(); //Style

                kml.WriteStartElement("LineString");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("coordinates");

                foreach (vec3 v3 in trk.gArr[i].curvePts)
                {
                    linePts += GetGeoCoordToWgs84_KML(v3.ToGeoCoord());
                }
                kml.WriteRaw(linePts);

                kml.WriteEndElement(); // <coordinates>
                kml.WriteEndElement(); // <LineString>

                kml.WriteEndElement(); // <Placemark>
            }
            kml.WriteEndElement(); // <Folder>   

            //Recorded Path
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Recorded Path");
            kml.WriteElementString("visibility", "1");

            linePts = "";
            kml.WriteStartElement("Placemark");
            kml.WriteElementString("visibility", "1");

            kml.WriteElementString("name", "Path " + 1);
            kml.WriteStartElement("Style");

            kml.WriteStartElement("LineStyle");
            kml.WriteElementString("color", "ff44ffff");
            kml.WriteElementString("width", "2");
            kml.WriteEndElement(); // <LineStyle>
            kml.WriteEndElement(); //Style

            kml.WriteStartElement("LineString");
            kml.WriteElementString("tessellate", "1");
            kml.WriteStartElement("coordinates");

            for (int j = 0; j < recPath.recList.Count; j++)
            {
                linePts += GetGeoCoordToWgs84_KML(recPath.recList[j].AsGeoCoord);
            }
            kml.WriteRaw(linePts);

            kml.WriteEndElement(); // <coordinates>
            kml.WriteEndElement(); // <LineString>

            kml.WriteEndElement(); // <Placemark>
            kml.WriteEndElement(); // <Folder>

            //flags  *************************************************************************
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Flags");

            for (int i = 0; i < flagPts.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("name", "Flag_" + i.ToString());

                kml.WriteStartElement("Style");
                kml.WriteStartElement("IconStyle");

                if (flagPts[i].color == 0)  //red - xbgr
                    kml.WriteElementString("color", "ff4400ff");
                if (flagPts[i].color == 1)  //grn - xbgr
                    kml.WriteElementString("color", "ff44ff00");
                if (flagPts[i].color == 2)  //yel - xbgr
                    kml.WriteElementString("color", "ff44ffff");

                kml.WriteEndElement(); //IconStyle
                kml.WriteEndElement(); //Style

                kml.WriteElementString("name", ((i + 1).ToString() + " " + flagPts[i].notes));
                kml.WriteStartElement("Point");
                kml.WriteElementString("coordinates", flagPts[i].longitude.ToString(CultureInfo.InvariantCulture) +
                    "," + flagPts[i].latitude.ToString(CultureInfo.InvariantCulture) + ",0");
                kml.WriteEndElement(); //Point
                kml.WriteEndElement(); // <Placemark>
            }
            kml.WriteEndElement(); // <Folder>   
            //End of Flags

            //Sections  ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Sections");
            //kml.WriteElementString("description", fd.GetDescription() );

            string secPts = "";
            int cntr = 0;

            for (int j = 0; j < triStrip.Count; j++)
            {
                int patches = triStrip[j].patchList.Count;

                if (patches > 0)
                {
                    //for every new chunk of patch
                    foreach (var triList in triStrip[j].patchList)
                    {
                        if (triList.Count > 0)
                        {
                            kml.WriteStartElement("Placemark");
                            kml.WriteElementString("name", "Sections_" + cntr.ToString());
                            cntr++;

                            string collor = "F0" + ((byte)(triList[0].heading)).ToString("X2") +
                                ((byte)(triList[0].northing)).ToString("X2") + ((byte)(triList[0].easting)).ToString("X2");

                            //lineStyle
                            kml.WriteStartElement("Style");

                            kml.WriteStartElement("LineStyle");
                            kml.WriteElementString("color", collor);
                            //kml.WriteElementString("width", "6");
                            kml.WriteEndElement(); // <LineStyle>

                            kml.WriteStartElement("PolyStyle");
                            kml.WriteElementString("color", collor);
                            kml.WriteEndElement(); // <PloyStyle>
                            kml.WriteEndElement(); //Style

                            kml.WriteStartElement("Polygon");
                            kml.WriteElementString("tessellate", "1");
                            kml.WriteStartElement("outerBoundaryIs");
                            kml.WriteStartElement("LinearRing");

                            //coords
                            kml.WriteStartElement("coordinates");
                            secPts = "";
                            for (int i = 1; i < triList.Count; i += 2)
                            {
                                secPts += GetGeoCoordToWgs84_KML(triList[i].ToGeoCoord());
                            }
                            for (int i = triList.Count - 1; i > 1; i -= 2)
                            {
                                secPts += GetGeoCoordToWgs84_KML(triList[i].ToGeoCoord());
                            }
                            secPts += GetGeoCoordToWgs84_KML(triList[1].ToGeoCoord());

                            kml.WriteRaw(secPts);
                            kml.WriteEndElement(); // <coordinates>

                            kml.WriteEndElement(); // <LinearRing>
                            kml.WriteEndElement(); // <outerBoundaryIs>
                            kml.WriteEndElement(); // <Polygon>

                            kml.WriteEndElement(); // <Placemark>
                        }
                    }
                }
            }
            kml.WriteEndElement(); // <Folder>
            //End of sections

            //end of document
            kml.WriteEndElement(); // <Document>
            kml.WriteEndElement(); // <kml>

            //The end
            kml.WriteEndDocument();

            kml.Flush();

            //Write the XML to file and close the kml
            kml.Close();
        }

        public string GetBoundaryPointsLatLon(int bndNum)
        {
            StringBuilder sb = new StringBuilder();

            foreach (vec3 v3 in bnd.bndList[bndNum].fenceLine)
            {
                sb.Append(GetGeoCoordToWgs84_KML(v3.ToGeoCoord()));
            }
            return sb.ToString();
        }

        private void FileUpdateAllFieldsKML()
        {

            //get the directory and make sure it exists, create if not
            string directoryName = RegistrySettings.fieldsDirectory;

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            {
                return; //We have no fields to aggregate.
            }

            string myFileName;
            myFileName = "AllFields.kml";

            XmlTextWriter kml = new XmlTextWriter(Path.Combine(directoryName, myFileName), Encoding.UTF8);

            kml.Formatting = Formatting.Indented;
            kml.Indentation = 3;

            kml.WriteStartDocument();
            kml.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            kml.WriteStartElement("Document");

            foreach (String dir in Directory.EnumerateDirectories(directoryName).OrderBy(d => new DirectoryInfo(d).Name).ToArray())
            //loop
            {
                if (!File.Exists(Path.Combine(dir, "Field.kml"))) continue;

                directoryName = Path.GetFileName(dir);
                kml.WriteStartElement("Folder");
                kml.WriteElementString("name", directoryName);

                var lines = File.ReadAllLines(Path.Combine(dir, "Field.kml"));
                LinkedList<string> linebuffer = new LinkedList<string>();
                for (var i = 3; i < lines.Length - 2; i++)  //We want to skip the first 3 and last 2 lines
                {
                    linebuffer.AddLast(lines[i]);
                    if (linebuffer.Count > 2)
                    {
                        kml.WriteRaw("   ");
                        kml.WriteRaw(Environment.NewLine);
                        kml.WriteRaw(linebuffer.First.Value);
                        linebuffer.RemoveFirst();
                    }
                }
                kml.WriteRaw("   ");
                kml.WriteRaw(Environment.NewLine);
                kml.WriteRaw(linebuffer.First.Value);
                linebuffer.RemoveFirst();
                kml.WriteRaw("   ");
                kml.WriteRaw(Environment.NewLine);
                kml.WriteRaw(linebuffer.First.Value);
                kml.WriteRaw(Environment.NewLine);

                kml.WriteEndElement(); // <Folder>
                kml.WriteComment("End of " + directoryName);
            }

            //end of document
            kml.WriteEndElement(); // <Document>
            kml.WriteEndElement(); // <kml>

            //The end
            kml.WriteEndDocument();

            kml.Flush();

            //Write the XML to file and close the kml
            kml.Close();
        }

        private string GetGeoCoordToWgs84_KML(GeoCoord geoCoord)
        {
            Wgs84 latLon = AppModel.LocalPlane.ConvertGeoCoordToWgs84(geoCoord);

            return
                latLon.Longitude.ToString("N7", CultureInfo.InvariantCulture) + ',' +
                latLon.Latitude.ToString("N7", CultureInfo.InvariantCulture) + ",0 ";
        }
        public void ExportFieldAs_ISOXMLv3()
        {
            //if (bnd.bndList.Count < 1) return;//If no Bnd, Quit

            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "zISOXML", "v3");

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            try
            {
                ISO11783_TaskFile.Export(
                    directoryName,
                    currentFieldDirectory,
                    (int)(fd.areaOuterBoundary),
                    bnd.bndList,
                    AppModel.LocalPlane,
                    trk,
                    ISO11783_TaskFile.Version.V3);
            }
            catch (Exception e)
            {
                TimedMessageBox(2000, "ISOXML Exception ", e.ToString());
                Log.EventWriter("Export field as ISOXML Exception" + e);
            }
        }

        public void ExportFieldAs_ISOXMLv4()
        {
            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "zISOXML", "v4");

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            try
            {
                ISO11783_TaskFile.Export(
                    directoryName,
                    currentFieldDirectory,
                    (int)(fd.areaOuterBoundary),
                    bnd.bndList,
                    AppModel.LocalPlane,
                    trk,
                    ISO11783_TaskFile.Version.V4);
            }
            catch (Exception e)
            {
                Log.EventWriter("Export Field as ISOXML: " + e.Message);
            }
        }

    }
}