using AgOpenGPS.Core.DrawLib;
using AgOpenGPS.Properties;
using System.Drawing;

namespace AgOpenGPS.Classes
{
    public class VehicleTextures
    {
        private Texture2D _tractor;
        private Texture2D _harvester;
        private Texture2D _articulatedFront;
        private Texture2D _articulatedRear;

        private Texture2D _frontWheel;
        private Texture2D _tire;
        private Texture2D _toolAxle;

        public VehicleTextures()
        {
        }

        public Texture2D Tractor
        {
            get
            {
                if (_tractor == null) _tractor = new Texture2D(null);
                return _tractor;
            }
        }

        public Texture2D Harvester
        {
            get
            {
                if (_harvester == null) _harvester = new Texture2D(null);
                return _harvester;
            }
        }

        public Texture2D ArticulatedFront
        {
            get
            {
                if (_articulatedFront == null) _articulatedFront = new Texture2D(null);
                return _articulatedFront;
            }
        }

        public Texture2D ArticulatedRear
        {
            get
            {
                if (_articulatedRear == null) _articulatedRear = new Texture2D(null);
                return _articulatedRear;
            }
        }

        public Texture2D FrontWheel
        {
            get
            {
                if (_frontWheel == null) _frontWheel = new Texture2D(Resources.z_FrontWheels);
                return _frontWheel;
            }
        }

        public Texture2D Tire
        {
            get
            {
                if (_tire == null) _tire = new Texture2D(Resources.z_Tire);
                return _tire;
            }
        }

        public Texture2D ToolAxle
        {
            get
            {
                if (_toolAxle == null) _toolAxle = new Texture2D(Resources.z_Tool);
                return _toolAxle;
            }
        }

    }
}
