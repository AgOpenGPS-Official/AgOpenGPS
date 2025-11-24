using System;
using System.Collections.Generic;
using AgOpenGPS.Core.Models;
using AgOpenGPS.IO;

namespace AgOpenGPS
{
    /// <summary>
    /// Handles copying tracks between fields with coordinate conversion.
    /// Stateless utility class for track operations.
    /// </summary>
    public static class TrackCopier
    {
        /// <summary>
        /// Convert tracks from one field to another, handling coordinate system transformations.
        /// </summary>
        /// <param name="tracks">Tracks to convert</param>
        /// <param name="sourceFieldDirectory">Source field directory path</param>
        /// <param name="targetFieldDirectory">Target field directory path</param>
        /// <param name="sharedFieldProperties">Shared field properties for coordinate conversion</param>
        /// <returns>List of converted tracks</returns>
        private static List<CTrk> ConvertTracks(
            List<CTrk> tracks,
            string sourceFieldDirectory,
            string targetFieldDirectory,
            SharedFieldProperties sharedFieldProperties)
        {
            if (tracks == null || tracks.Count == 0)
                return new List<CTrk>();

            // Load origins
            Wgs84 sourceOrigin = FieldPlaneFiles.LoadOrigin(sourceFieldDirectory);
            Wgs84 targetOrigin = FieldPlaneFiles.LoadOrigin(targetFieldDirectory);

            // Create LocalPlane converters
            var sourcePlane = new LocalPlane(sourceOrigin, sharedFieldProperties);
            var targetPlane = new LocalPlane(targetOrigin, sharedFieldProperties);

            return ConvertTracksWithPlanes(tracks, sourcePlane, targetPlane);
        }

        /// <summary>
        /// Convert tracks using pre-configured LocalPlane objects.
        /// </summary>
        private static List<CTrk> ConvertTracksWithPlanes(
            List<CTrk> tracks,
            LocalPlane sourcePlane,
            LocalPlane targetPlane)
        {
            var convertedTracks = new List<CTrk>();

            foreach (var sourceTrack in tracks)
            {
                // Normalize heading to 0-2π range
                double normalizedHeading = NormalizeHeading(sourceTrack.heading);

                var newTrack = new CTrk
                {
                    name = sourceTrack.name,
                    mode = sourceTrack.mode,
                    heading = normalizedHeading,
                    isVisible = true, // Always make copied tracks visible
                    nudgeDistance = sourceTrack.nudgeDistance,
                    workedTracks = new HashSet<int>() // Reset worked tracks for new field
                };

                // Convert points
                newTrack.ptA = ConvertVec2(sourceTrack.ptA, sourcePlane, targetPlane);
                newTrack.ptB = ConvertVec2(sourceTrack.ptB, sourcePlane, targetPlane);
                newTrack.endPtA = ConvertVec2(sourceTrack.endPtA, sourcePlane, targetPlane);
                newTrack.endPtB = ConvertVec2(sourceTrack.endPtB, sourcePlane, targetPlane);

                // Convert curve points
                newTrack.curvePts = ConvertCurvePoints(sourceTrack.curvePts, sourcePlane, targetPlane);

                convertedTracks.Add(newTrack);
            }

            return convertedTracks;
        }

        /// <summary>
        /// Convert a vec2 position from one coordinate system to another.
        /// </summary>
        private static vec2 ConvertVec2(vec2 source, LocalPlane sourcePlane, LocalPlane targetPlane)
        {
            // Convert vec2 (easting, northing) to GeoCoord
            var sourceGeoCoord = new GeoCoord(source.northing, source.easting);

            // Convert to WGS84 using source plane
            var wgs84 = sourcePlane.ConvertGeoCoordToWgs84(sourceGeoCoord);

            // Convert to target plane
            var targetGeoCoord = targetPlane.ConvertWgs84ToGeoCoord(wgs84);

            // Convert back to vec2
            return new vec2(targetGeoCoord.Easting, targetGeoCoord.Northing);
        }

        /// <summary>
        /// Convert curve points, recalculating headings between consecutive points.
        /// </summary>
        private static List<vec3> ConvertCurvePoints(
            List<vec3> sourceCurvePts,
            LocalPlane sourcePlane,
            LocalPlane targetPlane)
        {
            if (sourceCurvePts == null || sourceCurvePts.Count == 0)
                return new List<vec3>();

            var convertedCurvePts = new List<vec3>();

            // First convert all positions
            var convertedPositions = new List<vec2>();
            foreach (var pt in sourceCurvePts)
            {
                convertedPositions.Add(ConvertVec2(new vec2(pt.easting, pt.northing), sourcePlane, targetPlane));
            }

            // Now recalculate headings between consecutive converted points using GeoDir
            for (int i = 0; i < convertedPositions.Count; i++)
            {
                double heading;
                if (i < convertedPositions.Count - 1)
                {
                    // Calculate heading to next point using GeoDir
                    var fromCoord = new GeoCoord(convertedPositions[i].northing, convertedPositions[i].easting);
                    var toCoord = new GeoCoord(convertedPositions[i + 1].northing, convertedPositions[i + 1].easting);
                    var geoDir = new GeoDir(fromCoord, toCoord);
                    heading = geoDir.AngleInRadians;

                    // Normalize to 0-2π
                    heading = NormalizeHeading(heading);
                }
                else
                {
                    // Last point: use same heading as previous point
                    heading = convertedCurvePts[i - 1].heading;
                }

                convertedCurvePts.Add(new vec3(convertedPositions[i].easting, convertedPositions[i].northing, heading));
            }

            return convertedCurvePts;
        }

        /// <summary>
        /// Normalize heading to 0-2π range.
        /// </summary>
        private static double NormalizeHeading(double heading)
        {
            while (heading < 0)
                heading += Math.PI * 2.0;
            while (heading >= Math.PI * 2.0)
                heading -= Math.PI * 2.0;
            return heading;
        }

        /// <summary>
        /// Copy tracks from source field to target field, saving and reloading as needed.
        /// </summary>
        /// <param name="sourceFieldDirectory">Source field directory</param>
        /// <param name="targetFieldDirectory">Target field directory</param>
        /// <param name="tracksToConvert">Tracks to copy</param>
        /// <param name="sharedFieldProperties">Shared field properties</param>
        /// <returns>Number of tracks copied</returns>
        public static int CopyTracksToField(
            string sourceFieldDirectory,
            string targetFieldDirectory,
            List<CTrk> tracksToConvert,
            SharedFieldProperties sharedFieldProperties)
        {
            // Convert tracks
            var convertedTracks = ConvertTracks(
                tracksToConvert,
                sourceFieldDirectory,
                targetFieldDirectory,
                sharedFieldProperties);

            // Load existing tracks from target field
            var existingTracks = TrackFiles.Load(targetFieldDirectory);

            // Add converted tracks
            existingTracks.AddRange(convertedTracks);

            // Save to target field
            TrackFiles.Save(targetFieldDirectory, existingTracks);

            return convertedTracks.Count;
        }
    }
}
