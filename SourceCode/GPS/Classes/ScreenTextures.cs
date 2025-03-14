using AgOpenGPS.Core.DrawLib;
using AgOpenGPS.Properties;

namespace AgOpenGPS.Classes
{
    public class ScreenTextures
    {
        private Texture2D _compass;
        private Texture2D _crossTrackBackGround;
        private Texture2D _font;
        private Texture2D _lateralManual;
        private Texture2D _lift;
        private Texture2D _menuShowHide;
        private Texture2D _noGps;
        private Texture2D _pan;
        private Texture2D _speedo;
        private Texture2D _speedoNeedle;
        private Texture2D _steerDot;
        private Texture2D _steerPointer;
        private Texture2D _tramDot;
        private Texture2D _turn;
        private Texture2D _turnCancel;
        private Texture2D _turnManuel;
        private Texture2D _uTurnU;
        private Texture2D _uTurnH;
        private Texture2D _questionMark;
        private Texture2D _zoomIn;
        private Texture2D _zoomOut;

        public ScreenTextures()
        {
        }

        public Texture2D Compass
        {
            get
            {
                if (_compass == null) _compass = new Texture2D(Resources.z_Compass);
                return _compass;
            }
        }

        public Texture2D CrossTrackBackground
        {
            get
            {
                if (_crossTrackBackGround == null) _crossTrackBackGround = new Texture2D(Resources.ZoomOut48);
                return _crossTrackBackGround;
            }
        }

        public Texture2D Font
        {
            get
            {
                if (_font == null) _font = new Texture2D(Resources.z_Font);
                return _font;
            }
        }

        public Texture2D LateralManual
        {
            get
            {
                if (_lateralManual == null) _lateralManual = new Texture2D(Resources.z_LateralManual);
                return _lateralManual;
            }
        }

        public Texture2D Lift
        {
            get
            {
                if (_lift == null) _lift = new Texture2D(Resources.z_Lift);
                return _lift;
            }
        }

        public Texture2D MenuShowHide
        {
            get
            {
                if (_menuShowHide == null) _menuShowHide = new Texture2D(Resources.MenuHideShow);
                return _menuShowHide;
            }
        }

        public Texture2D NoGps
        {
            get
            {
                if (_noGps == null) _noGps = new Texture2D(Resources.z_NoGPS);
                return _noGps;
            }
        }

        public Texture2D Pan
        {
            get
            {
                if (_pan == null) _pan = new Texture2D(Resources.Pan);
                return _pan;
            }
        }

        public Texture2D Speedo
        {
            get
            {
                if (_speedo == null) _speedo = new Texture2D(Resources.z_Speedo);
                return _speedo;
            }
        }

        public Texture2D SpeedoNeedle
        {
            get
            {
                if (_speedoNeedle == null) _speedoNeedle = new Texture2D(Resources.z_SpeedoNeedle);
                return _speedoNeedle;
            }
        }

        public Texture2D SteerDot
        {
            get
            {
                if (_steerDot == null) _steerDot = new Texture2D(Resources.z_SteerDot);
                return _steerDot;
            }
        }

        public Texture2D SteerPointer
        {
            get
            {
                if (_steerPointer == null) _steerPointer = new Texture2D(Resources.z_SteerPointer);
                return _steerPointer;
            }
        }

        public Texture2D TramDot
        {
            get
            {
                if (_tramDot == null) _tramDot = new Texture2D(Resources.z_TramOnOff);
                return _tramDot;
            }
        }

        public Texture2D Turn
        {
            get
            {
                if (_turn == null) _turn = new Texture2D(Resources.z_Turn);
                return _turn;
            }
        }

        public Texture2D TurnCancel
        {
            get
            {
                if (_turnCancel == null) _turnCancel = new Texture2D(Resources.z_TurnCancel);
                return _turnCancel;
            }
        }

        public Texture2D TurnManual
        {
            get
            {
                if (_turnManuel == null) _turnManuel = new Texture2D(Resources.z_TurnManual);
                return _turnManuel;
            }
        }

        public Texture2D UTurnU
        {
            get
            {
                if (_uTurnU == null) _uTurnU = new Texture2D(Resources.YouTurnU);
                return _uTurnU;
            }
        }

        public Texture2D UTurnH
        {
            get
            {
                if (_uTurnH == null) _uTurnH = new Texture2D(Resources.YouTurnH);
                return _uTurnH;
            }
        }

        public Texture2D QuestionMark
        {
            get
            {
                if (_questionMark == null) _questionMark = new Texture2D(Resources.z_QuestionMark);
                return _questionMark;
            }
        }

        public Texture2D ZoomIn
        {
            get
            {
                if (_zoomIn == null) _zoomIn = new Texture2D(Resources.ZoomIn48);
                return _zoomIn;
            }
        }

        public Texture2D ZoomOut
        {
            get
            {
                if (_zoomOut == null) _zoomOut = new Texture2D(Resources.ZoomOut48);
                return _zoomOut;
            }
        }


    }

}
