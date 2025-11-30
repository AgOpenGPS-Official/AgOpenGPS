using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Drawing
{
    static public class Colors
    {
        // Physical colors
        static public readonly ColorRgba Black = new ColorRgba(0, 0, 0);
        static public readonly ColorRgb White = new ColorRgb(255, 255, 225);
        static public readonly ColorRgb Red = new ColorRgb(255, 0, 0);
        static public readonly ColorRgb Green = new ColorRgb(0, 255, 0);
        static public readonly ColorRgb Yellow = new ColorRgb(255, 255, 0);
        static public readonly ColorRgba Gray012 = new ColorRgba(0.12f, 0.12f, 0.12f);
        static public readonly ColorRgba Gray025 = new ColorRgba(0.25f, 0.25f, 0.25f);

        // Functional colors
        static public readonly ColorRgba AntennaColor = new ColorRgba(0.20f, 0.98f, 0.98f);
        static public readonly ColorRgba BingMapBackgroundColor = new ColorRgba(0.6f, 0.6f, 0.6f, 0.5f);
        static public readonly ColorRgb FlagRedColor = Red;
        static public readonly ColorRgb FlagGreenColor = Green;
        static public readonly ColorRgb FlagYellowColor = Yellow;
        static public readonly ColorRgba FlagSelectedBoxColor = new ColorRgba(0.980f, 0.0f, 0.980f);

        static public readonly ColorRgba GoalPointColor = new ColorRgba(0.98f, 0.98f, 0.098f);
        static public readonly ColorRgb HarvesterWheelColor = new ColorRgb(20, 20, 20);

        static public readonly ColorRgba HitchColor = new ColorRgba(0.765f, 0.76f, 0.32f);
        static public readonly ColorRgba HitchTrailingColor = new ColorRgba(0.7f, 0.4f, 0.2f);
        static public readonly ColorRgba HitchRigidColor = new ColorRgba(0.237f, 0.037f, 0.0397f);

        static public readonly ColorRgba SvenArrowColor = new ColorRgba(0.95f, 0.95f, 0.10f);

        static public readonly ColorRgba TramDotManualFlashOffColor = new ColorRgba(0.0f, 0.0f, 0.0f, 0.993f);
        static public readonly ColorRgba TramDotManualFlashOnColor = new ColorRgba(0.99f, 0.990f, 0.0f, 0.993f);
        static public readonly ColorRgba TramDotAutomaticControlBitOffColor = new ColorRgba(0.9f, 0.0f, 0.0f, 0.53f);
        static public readonly ColorRgba TramDotAutomaticControlBitOnColor = new ColorRgba(0.29f, 0.990f, 0.290f, 0.983f);
        static public readonly ColorRgba TramMarkerOnColor = new ColorRgba(0.0f, 0.900f, 0.39630f);

        static public readonly ColorRgba WorldGridDayColor = Gray012;
        static public readonly ColorRgba WorldGridNightColor = Gray025;
    }
}
