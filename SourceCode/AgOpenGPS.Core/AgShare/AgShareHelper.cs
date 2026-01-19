using System;
using System.Collections.Generic;
using AgOpenGPS.Core.AgShare.Models;

namespace AgOpenGPS.Core.AgShare
{
    public static class GeoUtils
    {
        // Calculates approximate area of a lat/lon polygon in hectares
        public static double CalculateAreaInHa(List<CoordinateDto> coords)
        {
            if (coords == null || coords.Count < 3)
                return 0;

            double area = 0;
            for (int i = 0, j = coords.Count - 1; i < coords.Count; j = i++)
            {
                double xi = coords[i].Longitude;
                double yi = coords[i].Latitude;
                double xj = coords[j].Longitude;
                double yj = coords[j].Latitude;
                area += (xj + xi) * (yj - yi);
            }

            return Math.Abs(area * 111.32 * 111.32 / 2.0) / 10000.0;
        }
    }
}
