using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Drawing
{
    static public class Colors
    {
        // Physical colors
        static public readonly ColorRgba Black = new ColorRgba(0xff000000);
        static public readonly ColorRgba White = new ColorRgba(0xffffffff);
        static public readonly ColorRgba Red = new ColorRgba(0xffff0000);
        static public readonly ColorRgba Green = new ColorRgba(0xff00ff00);
        static public readonly ColorRgba Yellow = new ColorRgba(0xffffff00);
        static public readonly ColorRgba Gray012 = new ColorRgba(0xff1e1e1e);
        static public readonly ColorRgba Gray025 = new ColorRgba(0xff3f3f3f);

        // Functional colors
        static public readonly ColorRgba AntennaColor = new ColorRgba(0xff33f9f9);
        static public readonly ColorRgba BingMapBackgroundColor = new ColorRgba(0x7f999999);
        static public readonly ColorRgba FlagSelectedBoxColor = new ColorRgba(0xfff900f9);

        static public readonly ColorRgba GoalPointColor = new ColorRgba(0xfff9f918);
        static public readonly ColorRgba HarvesterWheelColor = new ColorRgba(0xff141414);

        static public readonly ColorRgba HitchColor = new ColorRgba(0xffc3c151);
        static public readonly ColorRgba HitchTrailingColor = new ColorRgba(0xffb26633);
        static public readonly ColorRgba HitchRigidColor = new ColorRgba(0xff3c090a);

        static public readonly ColorRgba SvenArrowColor = new ColorRgba(0xfff2f219);

        static public readonly ColorRgba TramDotManualFlashOffColor = new ColorRgba(0xfd000000);
        static public readonly ColorRgba TramDotManualFlashOnColor = new ColorRgba(0xfdfcfc00);
        static public readonly ColorRgba TramDotAutomaticControlBitOffColor = new ColorRgba(0x87e50000);
        static public readonly ColorRgba TramDotAutomaticControlBitOnColor = new ColorRgba(0xfa49fc49);
        static public readonly ColorRgba TramMarkerOnColor = new ColorRgba(0xff00e565);

        static public readonly ColorRgba WorldGridDayColor = Gray012;
        static public readonly ColorRgba WorldGridNightColor = Gray025;
    }
}
