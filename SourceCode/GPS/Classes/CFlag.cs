using AgOpenGPS.Core.Models;
using System;

namespace AgOpenGPS
{
    public class CFlag
    {
        //WGS84 Lat Long
        public double latitude = 0;

        public double longitude = 0;

        public double heading = 0;

        //color of the flag - 0 is red, 1 is green, 2 is purple
        public int color = 0;

        public int ID = 0;

        public string notes = "";

        //constructor
        public CFlag(double _lati, double _longi, double _easting, double _northing, double _heading, int _color, int _ID, string _notes = "Notes")
        {
            GeoCoord = new GeoCoord(Math.Round(_northing, 7), Math.Round(_easting, 7));

            latitude = Math.Round(_lati, 7);
            longitude = Math.Round(_longi, 7);
            heading = Math.Round(_heading, 7);
            color = _color;
            ID = _ID;
            notes = _notes;
        }

        public GeoCoord GeoCoord { get; }
        public double northing => GeoCoord.Northing;
        public double easting => GeoCoord.Easting;

    }
}