using AgOpenGPS.Core.Models;
using AgOpenGPS.ResourcesBrands;
using System.Drawing;

namespace AgOpenGPS
{
    public static class TractorBitmaps
    {
        public static Bitmap GetBitmap(TractorBrand brand)
        {
            switch (brand)
            {
                case TractorBrand.Case:
                    return BrandImages.TractorCase;
                case TractorBrand.Claas:
                    return BrandImages.TractorClaas;
                case TractorBrand.Deutz:
                    return BrandImages.TractorDeutz;
                case TractorBrand.Fendt:
                    return BrandImages.TractorFendt;
                case TractorBrand.JohnDeere:
                    return BrandImages.TractorJohnDeere;
                case TractorBrand.Kubota:
                    return BrandImages.TractorKubota;
                case TractorBrand.Massey:
                    return BrandImages.TractorMassey;
                case TractorBrand.NewHolland:
                    return BrandImages.TractorNewHolland;
                case TractorBrand.Same:
                    return BrandImages.TractorSame;
                case TractorBrand.Steyr:
                    return BrandImages.TractorSteyr;
                case TractorBrand.Ursus:
                    return BrandImages.TractorUrsus;
                case TractorBrand.Valtra:
                    return BrandImages.TractorValtra;
                case TractorBrand.JCB:
                    return BrandImages.TractorJCB;
                default:
                    return BrandImages.TractorAoG;
            }
        }
    }

    public static class HarvesterBitmaps
    {
        public static Bitmap GetBitmap(HarvesterBrand brand)
        {
            switch (brand)
            {
                case HarvesterBrand.Case:
                    return BrandImages.HarvesterCase;
                case HarvesterBrand.Claas:
                    return BrandImages.HarvesterClaas;
                case HarvesterBrand.JohnDeere:
                    return BrandImages.HarvesterJohnDeere;
                case HarvesterBrand.NewHolland:
                    return BrandImages.HarvesterNewHolland;
                default:
                    return BrandImages.HarvesterAoG;
            }
        }
    }

    public static class ArticulatedBitmaps
    {
        public static Bitmap GetFrontBitmap(ArticulatedBrand brand)
        {
            switch (brand)
            {
                case ArticulatedBrand.Case:
                    return BrandImages.ArticulatedFrontCase;
                case ArticulatedBrand.Challenger:
                    return BrandImages.ArticulatedFrontChallenger;
                case ArticulatedBrand.JohnDeere:
                    return BrandImages.ArticulatedFrontJohnDeere;
                case ArticulatedBrand.NewHolland:
                    return BrandImages.ArticulatedFrontNewHolland;
                case ArticulatedBrand.Holder:
                    return BrandImages.ArticulatedFrontHolder;
                default:
                    return BrandImages.ArticulatedFrontAoG;
            }
        }

        public static Bitmap GetRearBitmap(ArticulatedBrand brand)
        {
            switch (brand)
            {
                case ArticulatedBrand.Case:
                    return BrandImages.ArticulatedRearCase;
                case ArticulatedBrand.Challenger:
                    return BrandImages.ArticulatedRearChallenger;
                case ArticulatedBrand.JohnDeere:
                    return BrandImages.ArticulatedRearJohnDeere;
                case ArticulatedBrand.NewHolland:
                    return BrandImages.ArticulatedRearNewHolland;
                case ArticulatedBrand.Holder:
                    return BrandImages.ArticulatedRearHolder;
                default:
                    return BrandImages.ArticulatedRearAoG;
            }
        }
    }
}
