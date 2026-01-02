using AgOpenGPS.Core.Drawing;
using AgOpenGPS.Core.DrawLib;
using AgOpenGPS.Core.Models;
using System;
using System.Collections.Generic;

namespace AgOpenGPS.Core.Visuals
{
    public class FieldGridVisual
    {
        static public readonly ColorRgba FieldGridDayColor = Colors.Gray012;
        static public readonly ColorRgba FieldGridNightColor = Colors.Gray025;

        private readonly FieldGrid _fieldGrid;

        public FieldGridVisual(FieldGrid fieldGrid)
        {
            _fieldGrid = fieldGrid;
        }

        public void Draw(bool isDay, GeoBoundingBox fieldBoundingBox)
        {
            ColorRgba color = isDay ? FieldGridDayColor : FieldGridNightColor;
            Draw(color, fieldBoundingBox);
        }

        public void Draw(ColorRgba color, GeoBoundingBox fieldBoundingBox)
        {
            if (fieldBoundingBox.IsEmpty) return;
            GeoCoord origin = new GeoCoord(0.0, 0.0);
            // Simply use the max distance from origin to a boundingbox corner, so it will work for any grid rotation
            double bbMaxMaxDist = origin.Distance(fieldBoundingBox.MaxCoord);
            double bbMinMinDist = origin.Distance(fieldBoundingBox.MinCoord);
            double bbMaxMinDist = origin.Distance(new GeoCoord(fieldBoundingBox.MaxNorthing, fieldBoundingBox.MinEasting));
            double bbMinMaxDist = origin.Distance(new GeoCoord(fieldBoundingBox.MinNorthing, fieldBoundingBox.MaxEasting));
            double maxDist = Math.Max(
                Math.Max(bbMaxMaxDist, bbMinMinDist),
                Math.Max(bbMaxMinDist, bbMinMaxDist));

            GLW.RotateZ(-_fieldGrid.GridAlignment.AngleInDegrees);

            GLW.SetLineWidth(1.0f);
            GLW.SetColor(color);
            // Start with the two perpendicular lines through the origin
            List<GeoCoord> vertices = new List<GeoCoord>
            {
                new GeoCoord(0.0, -maxDist),
                new GeoCoord(0.0, +maxDist),
                new GeoCoord(-maxDist, 0.0),
                new GeoCoord(+maxDist, 0.0)
            };
            for (double offset = _fieldGrid.GridStep; offset < maxDist; offset += _fieldGrid.GridStep)
            {
                vertices.Add(new GeoCoord(+offset, -maxDist));
                vertices.Add(new GeoCoord(+offset, +maxDist));
                vertices.Add(new GeoCoord(-offset, -maxDist));
                vertices.Add(new GeoCoord(-offset, +maxDist));

                vertices.Add(new GeoCoord(-maxDist, +offset));
                vertices.Add(new GeoCoord(+maxDist, +offset));
                vertices.Add(new GeoCoord(-maxDist, -offset));
                vertices.Add(new GeoCoord(+maxDist, -offset));
            }
            GLW.DrawLinesPrimitive(vertices.ToArray());
            GLW.RotateZ(_fieldGrid.GridAlignment.AngleInDegrees);
        }

    }
}
