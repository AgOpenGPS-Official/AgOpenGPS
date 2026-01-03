using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Drawing
{
    public static class Colors
    {
        // Physical colors
        public static readonly ColorRgba Black = new ColorRgba(0, 0, 0);
        public static readonly ColorRgba White = new ColorRgba(255, 255, 225);
        public static readonly ColorRgba Red = new ColorRgba(255, 0, 0);
        public static readonly ColorRgba Green = new ColorRgba(0, 255, 0);
        public static readonly ColorRgba Yellow = new ColorRgba(255, 255, 0);
        public static readonly ColorRgba Gray012 = new ColorRgba(0.12f, 0.12f, 0.12f);
        public static readonly ColorRgba Gray025 = new ColorRgba(0.25f, 0.25f, 0.25f);

        // Functional colors
        public static readonly ColorRgba AntennaColor = new ColorRgba(0.20f, 0.98f, 0.98f);
        public static readonly ColorRgba BingMapBackgroundColor = new ColorRgba(0.6f, 0.6f, 0.6f, 0.5f);
        public static readonly ColorRgba FlagSelectedBoxColor = new ColorRgba(0.980f, 0.0f, 0.980f);

        public static readonly ColorRgba GoalPointColor = new ColorRgba(0.98f, 0.98f, 0.098f);
        public static readonly ColorRgba HarvesterWheelColor = new ColorRgba(20, 20, 20);

        public static readonly ColorRgba HitchColor = new ColorRgba(0.765f, 0.76f, 0.32f);
        public static readonly ColorRgba HitchTrailingColor = new ColorRgba(0.7f, 0.4f, 0.2f);
        public static readonly ColorRgba HitchRigidColor = new ColorRgba(0.237f, 0.037f, 0.0397f);

        public static readonly ColorRgba SvenArrowColor = new ColorRgba(0.95f, 0.95f, 0.10f);

        public static readonly ColorRgba TramDotManualFlashOffColor = new ColorRgba(0.0f, 0.0f, 0.0f, 0.993f);
        public static readonly ColorRgba TramDotManualFlashOnColor = new ColorRgba(0.99f, 0.990f, 0.0f, 0.993f);
        public static readonly ColorRgba TramDotAutomaticControlBitOffColor = new ColorRgba(0.9f, 0.0f, 0.0f, 0.53f);
        public static readonly ColorRgba TramDotAutomaticControlBitOnColor = new ColorRgba(0.29f, 0.990f, 0.290f, 0.983f);
        public static readonly ColorRgba TramMarkerOnColor = new ColorRgba(0.0f, 0.900f, 0.39630f);

    }
}
