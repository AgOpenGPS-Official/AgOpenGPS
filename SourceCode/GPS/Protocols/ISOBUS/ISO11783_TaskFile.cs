using System.Collections.Generic;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Models.Guidance;
using AgOpenGPS.Core.Interfaces.Services;
using Dev4Agriculture.ISO11783.ISOXML.TaskFile;
using Dev4Agriculture.ISO11783.ISOXML;
using System;
using System.Linq;

namespace AgOpenGPS.Protocols.ISOBUS
{
    public class ISO11783_TaskFile
    {
        public enum Version { V3, V4 }

        // NEW Phase 6.5: Changed CTrack → ITrackService
        public static void Export(
            string directoryName,
            string designator,
            int area,
            List<CBoundaryList> bndList,
            LocalPlane localPlane,
            ITrackService trackService,
            Version version)
        {
            if (!Enum.IsDefined(typeof(Version), version))
                throw new ArgumentOutOfRangeException(nameof(version), version, "Invalid version");

            var isoxml = ISOXML.Create(directoryName);

            SetFileInformation(isoxml, version);
            AddPartfield(isoxml, designator, area, bndList, localPlane, trackService, version);

            isoxml.Save();
        }

        private static void SetFileInformation(ISOXML isoxml, Version version)
        {
            isoxml.DataTransferOrigin = ISO11783TaskDataFileDataTransferOrigin.FMIS;
            isoxml.ManagementSoftwareManufacturer = "AgOpenGPS";
            isoxml.ManagementSoftwareVersion = Program.Version;

            switch (version)
            {
                case Version.V3:
                    isoxml.VersionMajor = ISO11783TaskDataFileVersionMajor.Version3;
                    isoxml.VersionMinor = ISO11783TaskDataFileVersionMinor.Item3;
                    break;

                case Version.V4:
                    isoxml.VersionMajor = ISO11783TaskDataFileVersionMajor.Version4;
                    isoxml.VersionMinor = ISO11783TaskDataFileVersionMinor.Item2;
                    break;
            }
        }

        // NEW Phase 6.5: Changed CTrack → ITrackService
        private static void AddPartfield(
            ISOXML isoxml,
            string designator,
            int area,
            List<CBoundaryList> bndList,
            LocalPlane localPlane,
            ITrackService trackService,
            Version version)
        {
            var partfield = new ISOPartfield();
            isoxml.IdTable.AddObjectAndAssignIdIfNone(partfield);
            partfield.PartfieldDesignator = designator;
            partfield.PartfieldArea = (ulong)area;

            AddBoundary(partfield, bndList, localPlane);
            AddHeadland(partfield, bndList, localPlane);
            AddTracks(isoxml, partfield, trackService, localPlane, version);

            isoxml.Data.Partfield.Add(partfield);
        }

        private static void AddBoundary(ISOPartfield partfield, List<CBoundaryList> bndList, LocalPlane localPlane)
        {
            for (int i = 0; i < bndList.Count; i++)
            {
                var polygon = new ISOPolygon
                {
                    PolygonType = i == 0 ? ISOPolygonType.PartfieldBoundary : ISOPolygonType.Obstacle
                };

                var lineString = new ISOLineString
                {
                    LineStringType = ISOLineStringType.PolygonExterior
                };

                foreach (vec2 v2 in bndList[i].fenceLineEar)
                {
                    Wgs84 latLon = localPlane.ConvertGeoCoordToWgs84(v2.ToGeoCoord());
                    lineString.Point.Add(new ISOPoint
                    {
                        PointType = ISOPointType.other,
                        PointNorth = (decimal)latLon.Latitude,
                        PointEast = (decimal)latLon.Longitude
                    });
                }

                polygon.LineString.Add(lineString);

                partfield.PolygonnonTreatmentZoneonly.Add(polygon);
            }
        }

        private static void AddHeadland(ISOPartfield partfield, List<CBoundaryList> bndList, LocalPlane localPlane)
        {
            foreach (CBoundaryList boundaryList in bndList)
            {
                if (boundaryList.hdLine.Count < 1) continue;

                var polygon = new ISOPolygon
                {
                    PolygonType = ISOPolygonType.Headland
                };

                var lineString = new ISOLineString
                {
                    LineStringType = ISOLineStringType.PolygonExterior
                };

                foreach (vec3 v3 in boundaryList.hdLine)
                {
                    Wgs84 latLon = localPlane.ConvertGeoCoordToWgs84(v3.ToGeoCoord());
                    lineString.Point.Add(new ISOPoint
                    {
                        PointType = ISOPointType.other,
                        PointNorth = (decimal)latLon.Latitude,
                        PointEast = (decimal)latLon.Longitude
                    });
                }

                polygon.LineString.Add(lineString);

                partfield.PolygonnonTreatmentZoneonly.Add(polygon);
            }
        }

        // NEW Phase 6.5: Changed CTrack → ITrackService
        private static void AddTracks(ISOXML isoxml, ISOPartfield partfield, ITrackService trackService, LocalPlane localPlane, Version version)
        {
            var allTracks = trackService.GetAllTracks();
            if (allTracks == null || allTracks.Count == 0) return;

            foreach (Track track in allTracks)
            {
                // NEW: Use PascalCase properties
                if (track.Mode != Core.Models.Guidance.TrackMode.AB && track.Mode != Core.Models.Guidance.TrackMode.Curve) continue;

                switch (version)
                {
                    case Version.V3:
                        {
                            ISOLineString lineString = CreateLineString(track, localPlane, version);
                            lineString.LineStringDesignator = track.Name;  // PascalCase
                            partfield.LineString.Add(lineString);
                        }
                        break;

                    case Version.V4:
                        {
                            var guidanceGroup = new ISOGuidanceGroup
                            {
                                GuidanceGroupDesignator = track.Name  // PascalCase
                            };
                            isoxml.IdTable.AddObjectAndAssignIdIfNone(guidanceGroup);

                            var guidancePattern = new ISOGuidancePattern
                            {
                                GuidancePatternId = guidanceGroup.GuidanceGroupId,
                                GuidancePatternPropagationDirection = ISOGuidancePatternPropagationDirection.Bothdirections,
                                GuidancePatternExtension = ISOGuidancePatternExtension.Frombothfirstandlastpoint,
                                GuidancePatternGNSSMethod = ISOGuidancePatternGNSSMethod.Desktopgenerateddata
                            };

                            ISOLineString lineString = CreateLineString(track, localPlane, version);

                            switch (track.Mode)  // PascalCase
                            {
                                case Core.Models.Guidance.TrackMode.AB:  // Full namespace
                                    guidancePattern.GuidancePatternType = ISOGuidancePatternType.AB;
                                    break;

                                case Core.Models.Guidance.TrackMode.Curve:  // Full namespace
                                    guidancePattern.GuidancePatternType = ISOGuidancePatternType.Curve;
                                    break;

                                default:
                                    throw new InvalidOperationException("Track mode is invalid");
                            }

                            guidancePattern.LineString.Add(lineString);

                            guidanceGroup.GuidancePattern.Add(guidancePattern);

                            partfield.GuidanceGroup.Add(guidanceGroup);
                        }
                        break;
                }
            }
        }

        // NEW Phase 6.5: Changed CTrk → Track
        private static ISOLineString CreateLineString(Track track, LocalPlane localPlane, Version version)
        {
            switch (track.Mode)  // PascalCase
            {
                case Core.Models.Guidance.TrackMode.AB:
                    return CreateABLineString(track, localPlane, version);

                case Core.Models.Guidance.TrackMode.Curve:
                    return CreateCurveLineString(track, localPlane, version);

                default:
                    throw new InvalidOperationException("Track mode is invalid");
            }
        }


        // NEW Phase 6.5: Changed CTrk → Track
        private static ISOLineString CreateABLineString(Track track, LocalPlane localPlane, Version version)
        {
            var lineString = new ISOLineString
            {
                LineStringType = ISOLineStringType.GuidancePattern
            };

            GeoCoord pointA = track.PtA.ToGeoCoord();  // PascalCase
            GeoDir heading = new GeoDir(track.Heading);  // PascalCase
            Wgs84 latLon = localPlane.ConvertGeoCoordToWgs84(pointA - 1000.0 * heading);

            lineString.Point.Add(new ISOPoint
            {
                PointType = version == Version.V4 ? ISOPointType.GuidanceReferenceA : ISOPointType.other,
                PointNorth = (decimal)latLon.Latitude,
                PointEast = (decimal)latLon.Longitude
            });

            latLon = localPlane.ConvertGeoCoordToWgs84(pointA + 1000.0 * heading);

            lineString.Point.Add(new ISOPoint
            {
                PointType = version == Version.V4 ? ISOPointType.GuidanceReferenceB : ISOPointType.other,
                PointNorth = (decimal)latLon.Latitude,
                PointEast = (decimal)latLon.Longitude
            });

            return lineString;
        }

        // NEW Phase 6.5: Changed CTrk → Track
        private static ISOLineString CreateCurveLineString(Track track, LocalPlane localPlane, Version version)
        {
            var lineString = new ISOLineString
            {
                LineStringType = ISOLineStringType.GuidancePattern
            };

            for (int j = 0; j < track.CurvePts.Count; j++)  // PascalCase
            {
                Wgs84 latLon = localPlane.ConvertGeoCoordToWgs84(track.CurvePts[j].ToGeoCoord());  // PascalCase

                var point = new ISOPoint
                {
                    PointNorth = (decimal)latLon.Latitude,
                    PointEast = (decimal)latLon.Longitude
                };

                if (version == Version.V4)
                {
                    if (j == 0)
                    {
                        point.PointType = ISOPointType.GuidanceReferenceA;
                    }
                    else if (j == track.CurvePts.Count - 1)  // PascalCase
                    {
                        point.PointType = ISOPointType.GuidanceReferenceB;
                    }
                    else
                    {
                        point.PointType = ISOPointType.GuidancePoint;
                    }
                }
                else
                {
                    point.PointType = ISOPointType.other;
                }

                lineString.Point.Add(point);
            }

            return lineString;
        }
    }
}
