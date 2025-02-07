using AgOpenGPS.Core.Models;
using System.Collections.Generic;

namespace AgOpenGPS.Core.Models
{
    public class Boundary
    {

        public Boundary()
        {
            InnerBoundaries = new List<BoundaryPolygon>();
        }

        public BoundaryPolygon OuterBoundary { get; set; }
        public List<BoundaryPolygon> InnerBoundaries { get; }

        public double Area => OuterBoundary?.Area ?? 0.0;

    }
}
