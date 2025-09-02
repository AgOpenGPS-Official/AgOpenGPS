using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using AgLibrary.Logging;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.IO;
using AgOpenGPS.Protocols.ISOBUS;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        // Holds pending section patches to persist.
        public List<List<vec3>> patchSaveList = new List<List<vec3>>();

        // Holds pending contour patches to persist.
        public List<List<vec3>> contourSaveList = new List<List<vec3>>();

        // Returns field directory; creates it when ensureExists is true.
        private string GetFieldDir(bool ensureExists = false)
        {
            var dir = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);
            if (ensureExists && !string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        // Open a field with required precheck and per-file loaders.
        public void FileOpenField(string openType)
        {
            if (isJobStarted)
            {
                _ = FileSaveEverythingBeforeClosingField();
            }

            // Resolve Field.txt path
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

            // Set current field directory
            currentFieldDirectory = new DirectoryInfo(Path.GetDirectoryName(fileAndDirectory)).Name;
            var dir = GetFieldDir(false);

            // --- Load all field data ---
            if (!TryLoad("Field.txt", LoadCriticality.Required, () => FieldPlaneFiles.LoadOrigin(dir), out Wgs84 origin))
            {
                return;
            }
            pn.DefineLocalPlane(origin, true);

            JobNew();

            // --- Tracks ---
            FileLoadTracks();

            // --- Sections into triStrip + area ---
            if (TryLoad("Sections.txt", LoadCriticality.Optional, () => SectionsFiles.Load(dir), out var sections))
            {
                fd.workedAreaTotal = 0;
                fd.distanceUser = 0;
                if (triStrip != null && triStrip.Count > 0 && triStrip[0] != null)
                {
                    triStrip[0].patchList = new List<List<vec3>>();
                    foreach (var patch in sections)
                    {
                        triStrip[0].triangleList = new List<vec3>(patch);
                        triStrip[0].patchList.Add(triStrip[0].triangleList);

                        int verts = patch.Count - 2;
                        if (verts >= 2)
                        {
                            for (int j = 1; j < verts; j++)
                            {
                                double temp = patch[j].easting * (patch[j + 1].northing - patch[j + 2].northing)
                                            + patch[j + 1].easting * (patch[j + 2].northing - patch[j].northing)
                                            + patch[j + 2].easting * (patch[j].northing - patch[j + 1].northing);
                                fd.workedAreaTotal += Math.Abs(temp * 0.5);
                            }
                        }
                    }
                }
            }

            // --- Contour ---
            if (TryLoad("Contour.txt", LoadCriticality.Optional, () => ContourFiles.Load(dir), out var contours))
            {
                ct.stripList.Clear();
                foreach (var patch in contours)
                {
                    ct.ptList = new List<vec3>(patch);
                    ct.stripList.Add(ct.ptList);
                }
            }

            // --- Flags ---
            if (TryLoad("Flags.txt", LoadCriticality.Optional, () => FlagsFiles.Load(dir), out var flags))
            {
                flagPts.Clear();
                flagPts.AddRange(flags);
            }

            // --- Boundaries ---
            if (TryLoad("Boundary.txt", LoadCriticality.Optional, () => BoundaryFiles.Load(dir), out var boundaries))
            {
                bnd.bndList.Clear();
                bnd.bndList.AddRange(boundaries);
                CalculateMinMax();
                bnd.BuildTurnLines();

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
            }

            // --- Headlands ---
            TryRun("Headland.txt", LoadCriticality.Optional, () => HeadlandFiles.AttachLoad(dir, boundaries));

            // --- Tram ---
            if (TryLoad("Tram.txt", LoadCriticality.Optional, () => TramFiles.Load(dir), out var tramData))
            {
                tram.tramBndOuterArr.Clear();
                tram.tramBndOuterArr.AddRange(tramData.Outer);
                tram.tramBndInnerArr.Clear();
                tram.tramBndInnerArr.AddRange(tramData.Inner);
                tram.tramList.Clear();
                tram.tramList.AddRange(tramData.Lines);
                tram.displayMode = tram.tramBndOuterArr.Count > 0 ? 1 : 0;
                FixTramModeButton();
            }

            // --- RecPath ---
            if (TryLoad("RecPath.txt", LoadCriticality.Optional, () => RecPathFiles.Load(dir), out var recPathList))
            {
                recPath.recList.Clear();
                recPath.recList.AddRange(recPathList);
                panelDrag.Visible = recPath.recList.Count > 0;
            }

            // --- BackPic ---
            var backPic = BackPicFiles.Load(dir);
            worldGrid.isGeoMap = backPic.IsGeoMap;
            if (worldGrid.isGeoMap)
            {
                worldGrid.eastingMaxGeo = backPic.EastingMax;
                worldGrid.eastingMinGeo = backPic.EastingMin;
                worldGrid.northingMaxGeo = backPic.NorthingMax;
                worldGrid.northingMinGeo = backPic.NorthingMin;

                var bitmap = BackPicFiles.LoadImage(dir);
                if (bitmap != null)
                {
                    worldGrid.BingBitmap = bitmap;
                }
                else
                {
                    worldGrid.isGeoMap = false;
                }
            }

            // optional
            TryLoad("Elevation.txt", LoadCriticality.Optional, () => ElevationFiles.Load(dir), out var elevation);

            // --- Final UI refresh ---
            PanelsAndOGLSize();
            SetZoom();
            oglZoom.Refresh();
        }

        // Save HeadLines.
        public void FileSaveHeadLines()
        {
            HeadlinesFiles.Save(GetFieldDir(true), hdl.tracksArr);
        }

        // Load HeadLines (no message if missing).
        public void FileLoadHeadLines()
        {
            var dir = GetFieldDir();
            List<CHeadPath> headlines;
            if (!TryLoad("Headlines.txt", LoadCriticality.Optional, () => HeadlinesFiles.Load(dir), out headlines))
            {
                headlines = new List<CHeadPath>();
            }

            hdl.tracksArr?.Clear();
            hdl.tracksArr.AddRange(headlines);
            hdl.idx = -1;
        }

        // Save tracks
        public void FileSaveTracks()
        {
            TrackFiles.Save(GetFieldDir(true), trk.gArr);
        }

        // Load tracks
        public void FileLoadTracks()
        {
            var dir = GetFieldDir();

            List<CTrk> tracks;
            if (!TryLoad("TrackLines.txt", LoadCriticality.Optional, () => TrackFiles.Load(dir), out tracks))
            {
                tracks = new List<CTrk>();
            }

            trk.gArr?.Clear();
            trk.gArr.AddRange(tracks);
            trk.idx = -1;
        }

        // Create Field.txt for a new field session.
        public void FileCreateField()
        {
            if (!isJobStarted)
            {
                TimedMessageBox(3000, gStr.gsFieldNotOpen, gStr.gsCreateNewField);
                return;
            }

            var dir = GetFieldDir(true);
            var startFix = new Wgs84(AppModel.CurrentLatLon.Latitude, AppModel.CurrentLatLon.Longitude);
            FieldPlaneFiles.Save(dir, DateTime.Now, startFix);
        }

        public void FileCreateElevation()
        {
            var dir = GetFieldDir(true);
            var startFix = new Wgs84(AppModel.CurrentLatLon.Latitude, AppModel.CurrentLatLon.Longitude);
            ElevationFiles.CreateHeader(dir, DateTime.Now, startFix);
        }

        public void FileSaveElevation()
        {
            var dir = GetFieldDir(true);
            ElevationFiles.Append(dir, sbGrid.ToString());
            sbGrid.Clear();
        }

        // Append pending sections.
        public void FileSaveSections()
        {
            if (patchSaveList.Count > 0)
            {
                SectionsFiles.Append(GetFieldDir(true), patchSaveList);
                patchSaveList.Clear();
            }
        }

        // Create empty Sections.txt.
        public void FileCreateSections()
        {
            SectionsFiles.CreateEmpty(GetFieldDir(true));
        }

        // Create Boundary.txt header.
        public void FileCreateBoundary()
        {
            var dir = GetFieldDir(true);
            BoundaryFiles.CreateEmpty(dir);
        }


        // Create Flags.txt header and zero count.
        public void FileCreateFlags()
        {
            FlagsFiles.Save(GetFieldDir(true), new List<CFlag>(0));
        }

        // Create Contour.txt with header.
        public void FileCreateContour()
        {
            ContourFiles.CreateFile(GetFieldDir(true));
        }

        // Append pending contour patches.
        public void FileSaveContour()
        {
            if (contourSaveList.Count > 0)
            {
                ContourFiles.Append(GetFieldDir(true), contourSaveList);
                contourSaveList.Clear();
            }
        }

        // Save boundaries.
        public void FileSaveBoundary()
        {
            BoundaryFiles.Save(GetFieldDir(true), bnd.bndList);
        }

        // Save tram data.
        public void FileSaveTram()
        {
            TramFiles.Save(GetFieldDir(true), tram.tramBndOuterArr, tram.tramBndInnerArr, tram.tramList);
        }

        // Save headland(s).
        public void FileSaveHeadland()
        {
            HeadlandFiles.Save(GetFieldDir(true), bnd.bndList);
        }

        // Create RecPath header + zero count.
        public void FileCreateRecPath()
        {
            var dir = GetFieldDir(true);
            RecPathFiles.CreateEmpty(dir);
        }


        // Save recorded path.
        public void FileSaveRecPath(string name = "RecPath.Txt")
        {
            RecPathFiles.Save(GetFieldDir(true), recPath.recList, name);
        }

        // Load RecPath.txt (message if missing).
        public void FileLoadRecPath()
        {
            var dir = GetFieldDir();

            List<CRecPathPt> rec;
            if (!TryLoad("RecPath.txt", LoadCriticality.Optional, () => RecPathFiles.Load(dir), out rec))
            {
                rec = new List<CRecPathPt>();
            }

            recPath.recList.Clear();
            recPath.recList.AddRange(rec);
            panelDrag.Visible = recPath.recList.Count > 0;
        }

        // Save flags.
        public void FileSaveFlags()
        {
            FlagsFiles.Save(GetFieldDir(true), flagPts);
        }

        // Export one flag to KML using WGS84 from LocalPlane.
        public void FileSaveSingleFlagKML2(int flagNumber)
        {
            Wgs84 latLon = AppModel.LocalPlane.ConvertGeoCoordToWgs84(flagPts[flagNumber - 1].GeoCoord);

            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            {
                Directory.CreateDirectory(directoryName);
            }

            string myFileName = "Flag.kml";
            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, myFileName)))
            {
                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2"">");
                writer.WriteLine(@"<Document>");
                writer.WriteLine(@"  <Placemark>");
                writer.WriteLine(@"<Style> <IconStyle>");
                if (flagPts[flagNumber - 1].color == 0)
                {
                    writer.WriteLine(@"<color>ff4400ff</color>");
                }
                if (flagPts[flagNumber - 1].color == 1)
                {
                    writer.WriteLine(@"<color>ff44ff00</color>");
                }
                if (flagPts[flagNumber - 1].color == 2)
                {
                    writer.WriteLine(@"<color>ff44ffff</color>");
                }
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> " + flagNumber.ToString(CultureInfo.InvariantCulture) + @"</name>");
                writer.WriteLine(@"<Point><coordinates> "
                    + latLon.Longitude.ToString(CultureInfo.InvariantCulture) + ","
                    + latLon.Latitude.ToString(CultureInfo.InvariantCulture) + ",0"
                    + @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>");
            }
        }

        // Export one flag to KML using stored WGS84.
        public void FileSaveSingleFlagKML(int flagNumber)
        {
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            {
                Directory.CreateDirectory(directoryName);
            }

            string myFileName = "Flag.kml";
            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, myFileName)))
            {
                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2"">");
                writer.WriteLine(@"<Document>");
                writer.WriteLine(@"  <Placemark>");
                writer.WriteLine(@"<Style> <IconStyle>");
                if (flagPts[flagNumber - 1].color == 0)
                {
                    writer.WriteLine(@"<color>ff4400ff</color>");
                }
                if (flagPts[flagNumber - 1].color == 1)
                {
                    writer.WriteLine(@"<color>ff44ff00</color>");
                }
                if (flagPts[flagNumber - 1].color == 2)
                {
                    writer.WriteLine(@"<color>ff44ffff</color>");
                }
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> " + flagNumber.ToString(CultureInfo.InvariantCulture) + @"</name>");
                writer.WriteLine(@"<Point><coordinates> " +
                                flagPts[flagNumber - 1].longitude.ToString(CultureInfo.InvariantCulture) + "," + flagPts[flagNumber - 1].latitude.ToString(CultureInfo.InvariantCulture) + ",0" +
                                @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>");
            }
        }

        // Export current position to KML.
        public void FileMakeKMLFromCurrentPosition(Wgs84 currentLatLon)
        {
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            {
                Directory.CreateDirectory(directoryName);
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "CurrentPosition.kml")))
            {
                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2"">");
                writer.WriteLine(@"<Document>");
                writer.WriteLine(@"  <Placemark>");
                writer.WriteLine(@"<Style> <IconStyle>");
                writer.WriteLine(@"<color>ff4400ff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> Your Current Position </name>");
                writer.WriteLine(@"<Point><coordinates> "
                    + currentLatLon.Longitude.ToString(CultureInfo.InvariantCulture) + ","
                    + currentLatLon.Latitude.ToString(CultureInfo.InvariantCulture) + ",0"
                    + @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>");
            }
        }

        // Export full field to KML.
        public void ExportFieldAs_KML()
        {
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            {
                Directory.CreateDirectory(directoryName);
            }

            string myFileName = "Field.kml";
            XmlTextWriter kml = new XmlTextWriter(Path.Combine(directoryName, myFileName), Encoding.UTF8)
            {
                Formatting = Formatting.Indented,
                Indentation = 3
            };

            kml.WriteStartDocument();
            kml.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            kml.WriteStartElement("Document");

            // Description
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Field Stats");
            kml.WriteElementString("description", fd.GetDescription());
            kml.WriteEndElement();

            // Boundaries
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Boundaries");

            for (int i = 0; i < bnd.bndList.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                if (i == 0)
                {
                    kml.WriteElementString("name", currentFieldDirectory);
                }

                kml.WriteStartElement("Style");
                kml.WriteStartElement("LineStyle");
                if (i == 0)
                {
                    kml.WriteElementString("color", "ffdd00dd");
                }
                else
                {
                    kml.WriteElementString("color", "ff4d3ffd");
                }
                kml.WriteElementString("width", "4");
                kml.WriteEndElement();

                kml.WriteStartElement("PolyStyle");
                if (i == 0)
                {
                    kml.WriteElementString("color", "407f3f55");
                }
                else
                {
                    kml.WriteElementString("color", "703f38f1");
                }
                kml.WriteEndElement();
                kml.WriteEndElement();

                kml.WriteStartElement("Polygon");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("outerBoundaryIs");
                kml.WriteStartElement("LinearRing");

                kml.WriteStartElement("coordinates");
                string bndPts = "";
                if (bnd.bndList[i].fenceLine.Count > 3)
                {
                    bndPts = GetBoundaryPointsLatLon(i);
                }
                kml.WriteRaw(bndPts);
                kml.WriteEndElement();

                kml.WriteEndElement();
                kml.WriteEndElement();
                kml.WriteEndElement();
                kml.WriteEndElement();
            }

            kml.WriteEndElement(); // Boundaries

            // AB lines
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
                kml.WriteEndElement();
                kml.WriteEndElement();

                kml.WriteStartElement("LineString");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("coordinates");

                GeoCoord pointA = track.ptA.ToGeoCoord();
                GeoDir heading = new GeoDir(track.heading);
                linePts = GetGeoCoordToWgs84_KML(pointA - ABLine.abLength * heading);
                linePts += GetGeoCoordToWgs84_KML(pointA + ABLine.abLength * heading);
                kml.WriteRaw(linePts);

                kml.WriteEndElement();
                kml.WriteEndElement();
                kml.WriteEndElement();
            }
            kml.WriteEndElement(); // AB_Lines

            // Curve lines
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
                kml.WriteEndElement();
                kml.WriteEndElement();

                kml.WriteStartElement("LineString");
                kml.WriteElementString("tessellate", "1");
                kml.WriteStartElement("coordinates");

                foreach (vec3 v3 in trk.gArr[i].curvePts)
                {
                    linePts += GetGeoCoordToWgs84_KML(v3.ToGeoCoord());
                }
                kml.WriteRaw(linePts);

                kml.WriteEndElement();
                kml.WriteEndElement();

                kml.WriteEndElement();
            }
            kml.WriteEndElement(); // Curve_Lines

            // Recorded path
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Recorded Path");
            kml.WriteElementString("visibility", "1");

            linePts = "";
            kml.WriteStartElement("Placemark");
            kml.WriteElementString("visibility", "1");
            kml.WriteElementString("name", "Path 1");

            kml.WriteStartElement("Style");
            kml.WriteStartElement("LineStyle");
            kml.WriteElementString("color", "ff44ffff");
            kml.WriteElementString("width", "2");
            kml.WriteEndElement();
            kml.WriteEndElement();

            kml.WriteStartElement("LineString");
            kml.WriteElementString("tessellate", "1");
            kml.WriteStartElement("coordinates");

            for (int j = 0; j < recPath.recList.Count; j++)
            {
                linePts += GetGeoCoordToWgs84_KML(recPath.recList[j].AsGeoCoord);
            }
            kml.WriteRaw(linePts);

            kml.WriteEndElement();
            kml.WriteEndElement();

            kml.WriteEndElement(); // Placemark
            kml.WriteEndElement(); // Folder

            // Flags
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Flags");

            for (int i = 0; i < flagPts.Count; i++)
            {
                kml.WriteStartElement("Placemark");
                kml.WriteElementString("name", "Flag_" + i.ToString());

                kml.WriteStartElement("Style");
                kml.WriteStartElement("IconStyle");
                if (flagPts[i].color == 0)
                {
                    kml.WriteElementString("color", "ff4400ff");
                }
                if (flagPts[i].color == 1)
                {
                    kml.WriteElementString("color", "ff44ff00");
                }
                if (flagPts[i].color == 2)
                {
                    kml.WriteElementString("color", "ff44ffff");
                }
                kml.WriteEndElement();
                kml.WriteEndElement();

                kml.WriteElementString("name", ((i + 1).ToString() + " " + flagPts[i].notes));
                kml.WriteStartElement("Point");
                kml.WriteElementString("coordinates", flagPts[i].longitude.ToString(CultureInfo.InvariantCulture) +
                    "," + flagPts[i].latitude.ToString(CultureInfo.InvariantCulture) + ",0");
                kml.WriteEndElement();
                kml.WriteEndElement();
            }
            kml.WriteEndElement(); // Flags

            // Sections
            kml.WriteStartElement("Folder");
            kml.WriteElementString("name", "Sections");

            string secPts = "";
            int cntr = 0;

            for (int j = 0; j < triStrip.Count; j++)
            {
                int patches = triStrip[j].patchList.Count;

                if (patches > 0)
                {
                    foreach (var triList in triStrip[j].patchList)
                    {
                        if (triList.Count > 0)
                        {
                            kml.WriteStartElement("Placemark");
                            kml.WriteElementString("name", "Sections_" + cntr.ToString());
                            cntr++;

                            string collor = "F0" + ((byte)(triList[0].heading)).ToString("X2") +
                                ((byte)(triList[0].northing)).ToString("X2") + ((byte)(triList[0].easting)).ToString("X2");

                            kml.WriteStartElement("Style");
                            kml.WriteStartElement("LineStyle");
                            kml.WriteElementString("color", collor);
                            kml.WriteEndElement();

                            kml.WriteStartElement("PolyStyle");
                            kml.WriteElementString("color", collor);
                            kml.WriteEndElement();
                            kml.WriteEndElement();

                            kml.WriteStartElement("Polygon");
                            kml.WriteElementString("tessellate", "1");
                            kml.WriteStartElement("outerBoundaryIs");
                            kml.WriteStartElement("LinearRing");

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
                            kml.WriteEndElement();

                            kml.WriteEndElement();
                            kml.WriteEndElement();
                            kml.WriteEndElement();

                            kml.WriteEndElement();
                        }
                    }
                }
            }
            kml.WriteEndElement(); // Sections

            // End document
            kml.WriteEndElement();
            kml.WriteEndElement();

            kml.WriteEndDocument();
            kml.Flush();
            kml.Close();
        }

        // Helper to build lat/lon list for one boundary.
        public string GetBoundaryPointsLatLon(int bndNum)
        {
            StringBuilder sb = new StringBuilder();

            foreach (vec3 v3 in bnd.bndList[bndNum].fenceLine)
            {
                sb.Append(GetGeoCoordToWgs84_KML(v3.ToGeoCoord()));
            }
            return sb.ToString();
        }

        // Regenerates an overview KML for all fields that already produced Field.kml.
        private void FileUpdateAllFieldsKML()
        {
            string directoryName = RegistrySettings.fieldsDirectory;
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            {
                return;
            }

            string myFileName = "AllFields.kml";
            XmlTextWriter kml = new XmlTextWriter(Path.Combine(directoryName, myFileName), Encoding.UTF8)
            {
                Formatting = Formatting.Indented,
                Indentation = 3
            };

            kml.WriteStartDocument();
            kml.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            kml.WriteStartElement("Document");

            foreach (string dir in Directory.EnumerateDirectories(directoryName).OrderBy(d => new DirectoryInfo(d).Name).ToArray())
            {
                if (!File.Exists(Path.Combine(dir, "Field.kml")))
                {
                    continue;
                }

                string name = Path.GetFileName(dir);
                kml.WriteStartElement("Folder");
                kml.WriteElementString("name", name);

                var lines = File.ReadAllLines(Path.Combine(dir, "Field.kml"));
                LinkedList<string> linebuffer = new LinkedList<string>();
                for (int i = 3; i < lines.Length - 2; i++)
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

                kml.WriteEndElement(); // Folder
                kml.WriteComment("End of " + name);
            }

            kml.WriteEndElement(); // Document
            kml.WriteEndElement(); // kml
            kml.WriteEndDocument();
            kml.Flush();
            kml.Close();
        }

        // Formats a GeoCoord as "lon,lat,0 ".
        private string GetGeoCoordToWgs84_KML(GeoCoord geoCoord)
        {
            Wgs84 latLon = AppModel.LocalPlane.ConvertGeoCoordToWgs84(geoCoord);
            return latLon.Longitude.ToString("N7", CultureInfo.InvariantCulture) + ',' +
                   latLon.Latitude.ToString("N7", CultureInfo.InvariantCulture) + ",0 ";
        }

        // Export ISOXML v3.
        public void ExportFieldAs_ISOXMLv3()
        {
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "zISOXML", "v3");
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            {
                Directory.CreateDirectory(directoryName);
            }

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

        // Export ISOXML v4.
        public void ExportFieldAs_ISOXMLv4()
        {
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory, "zISOXML", "v4");
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            {
                Directory.CreateDirectory(directoryName);
            }

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

        // Criticality flag for loader calls.
        private enum LoadCriticality
        {
            Required,
            Optional
        }

        // Runs a loader with return value; logs + user message on failure.
        private bool TryLoad<T>(string fileLabel, LoadCriticality criticality, Func<T> loader, out T result)
        {
            try
            {
                result = loader();
                return true;
            }
            catch (Exception)
            {
                Log.EventWriter($"[Load:{fileLabel}] failed");
                if (criticality == LoadCriticality.Required)
                {
                    TimedMessageBox(2500, gStr.gsFieldFileIsCorrupt, $"{fileLabel} is required and could not be loaded.");
                }
                else
                {
                    TimedMessageBox(2000, "Optional file problem", $"{fileLabel} is missing or corrupt but Field is Loaded");
                }
                result = default(T);
                return false;
            }
        }

        // Runs an action (no return); logs + user message on failure.
        private bool TryRun(string fileLabel, LoadCriticality criticality, Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                Log.EventWriter($"[Load:{fileLabel}] failed: {ex}");
                if (criticality == LoadCriticality.Required)
                {
                    TimedMessageBox(2500, gStr.gsFieldFileIsCorrupt, $"{fileLabel} is required and could not be processed.");
                }
                else
                {
                    TimedMessageBox(2000, "Optional file problem", $"{fileLabel} is missing or corrupt; it will be recreated on save.");
                }
                return false;
            }
        }
    }
}
