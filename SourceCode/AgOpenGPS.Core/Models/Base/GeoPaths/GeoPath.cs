﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AgOpenGPS.Core.Models
{
    public class GeoPath : GeoPathBase
    {
        protected readonly List<GeoCoord> _coords;
        public GeoPath()
        {
            _coords = new List<GeoCoord>();
        }

        public override int Count => _coords.Count;
        public GeoCoord Last => _coords[_coords.Count - 1];
        public ReadOnlyCollection<GeoCoord> Coords => _coords.AsReadOnly();

        public override GeoCoord this[int index]
        {
            get { return _coords[index]; }
        }

        public void Clear()
        {
            _coords.Clear();
        }

        public virtual void Add(GeoCoord coord)
        {
            _coords.Add(coord);
        }

        public bool IsFarAwayFromPath(GeoCoord testCoord, double minimumDistanceSquared)
        {
            return _coords.TrueForAll(coordOnPath => coordOnPath.DistanceSquared(testCoord) >= minimumDistanceSquared);
        }

    }
}
