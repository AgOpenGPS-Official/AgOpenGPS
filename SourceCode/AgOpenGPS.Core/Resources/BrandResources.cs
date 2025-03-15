using System.Drawing;

namespace AgOpenGPS.Core.Resources
{
    // This file circumvents a weird bug that happens during editing of FormConfig.resx.
    // Without this file, FormConfig.resx needs to reference the resoures in BrandImages.resx
    // directly, and for some unknown reason, it would add all these resources also to the file
    // FormConfig.resx itself. This does not happen when FormConfig can reference the Bitmaps in this file.
    public static class BrandResources
    {
        public static Bitmap BrandAoG => BrandImages.BrandAoG;
        public static Bitmap BrandClaas => BrandImages.BrandClaas;
        public static Bitmap BrandCase => BrandImages.BrandCase;
        public static Bitmap BrandChallenger => BrandImages.BrandChallenger;
        public static Bitmap BrandDeutz => BrandImages.BrandDeutz;
        public static Bitmap BrandFendt => BrandImages.BrandFendt;
        public static Bitmap BrandHolder => BrandImages.BrandHolder;
        public static Bitmap BrandJCB => BrandImages.BrandJCB;
        public static Bitmap BrandJohnDeere => BrandImages.BrandJohnDeere;
        public static Bitmap BrandKubota => BrandImages.BrandKubota;
        public static Bitmap BrandMassey => BrandImages.BrandMassey;
        public static Bitmap BrandNewHolland => BrandImages.BrandNewHolland;
        public static Bitmap BrandSame => BrandImages.BrandSame;
        public static Bitmap BrandSteyr => BrandImages.BrandSteyr;
        public static Bitmap BrandUrsus => BrandImages.BrandUrsus;
        public static Bitmap BrandValtra => BrandImages.BrandValtra;

        public static Bitmap BrandTriangleVehicle => BrandImages.BrandTriangleVehicle;
        public static Bitmap TractorDeutz => BrandImages.TractorDeutz;
    }
}
