using AgOpenGPS.Core.Models;
namespace AgOpenGPS.Core.Drawing
{
    static public class Colors
    {
        // Physical colors
        static public ColorRgb Black = new ColorRgb(0, 0, 0);
        static public ColorRgb White = new ColorRgb(255, 255, 225);
        static public ColorRgb Red = new ColorRgb(255, 0, 0);
        static public ColorRgb Green = new ColorRgb(0, 255, 0);
        static public ColorRgb Yellow = new ColorRgb(255, 255, 0);

        // Functional colors
        static public ColorRgb HitchColor = new ColorRgb(0.765f, 0.76f, 0.32f);
        static public ColorRgb HitchTrailingColor = new ColorRgb(0.7f, 0.4f, 0.2f);

        static public ColorRgba TramDotManualFlashOffColor = new ColorRgba(0.0f, 0.0f, 0.0f, 0.993f);
        static public ColorRgba TramDotManualFlashOnColor = new ColorRgba(0.99f, 0.990f, 0.0f, 0.993f);
        static public ColorRgba TramDotAutomaticControlBitOffColor = new ColorRgba(0.9f, 0.0f, 0.0f, 0.53f);
        static public ColorRgba TramDotAutomaticControlBitOnColor = new ColorRgba(0.29f, 0.990f, 0.290f, 0.983f);
        static public ColorRgb TramMarkerOnColor = new ColorRgb(0.0f, 0.900f, 0.39630f);

        static public ColorRgb FlagRedColor = Red;
        static public ColorRgb FlagGreenColor = Green;
        static public ColorRgb FlagYellowColor = Yellow;
    }
}
