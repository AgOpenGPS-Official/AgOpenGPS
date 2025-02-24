using AgOpenGPS.Core.Resources;
using System.Drawing;

namespace AgOpenGPS.Core.Models
{
    public enum TractorBrand { AGOpenGPS, Case, Claas, Deutz, Fendt, JohnDeere, Kubota, Massey, NewHolland, Same, Steyr, Ursus, Valtra, JCB }
    public enum HarvesterBrand { AgOpenGPS, Case, Claas, JohnDeere, NewHolland }
    public enum ArticulatedBrand { AgOpenGPS, Case, Challenger, JohnDeere, NewHolland, Holder }

    public static class TractorBitmaps
    {
        public static Bitmap GetBitmap(TractorBrand brand)
        {
            switch (brand)
            {
                case TractorBrand.Case:
                    return BrandBitmaps.TractorCase;
                case TractorBrand.Claas:
                    return BrandBitmaps.TractorClaas;
                case TractorBrand.Deutz:
                    return BrandBitmaps.TractorDeutz;
                case TractorBrand.Fendt:
                    return BrandBitmaps.TractorFendt;
                case TractorBrand.JohnDeere:
                    return BrandBitmaps.TractorJohnDeere;
                case TractorBrand.Kubota:
                    return BrandBitmaps.TractorKubota;
                case TractorBrand.Massey:
                    return BrandBitmaps.TractorMassey;
                case TractorBrand.NewHolland:
                    return BrandBitmaps.TractorNewHolland;
                case TractorBrand.Same:
                    return BrandBitmaps.TractorSame;
                case TractorBrand.Steyr:
                    return BrandBitmaps.TractorSteyr;
                case TractorBrand.Ursus:
                    return BrandBitmaps.TractorUrsus;
                case TractorBrand.Valtra:
                    return BrandBitmaps.TractorValtra;
                case TractorBrand.JCB:
                    return BrandBitmaps.TractorJCB;
                default:
                    return BrandBitmaps.TractorAoG;
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
                    return BrandBitmaps.HarvesterCase;
                case HarvesterBrand.Claas:
                    return BrandBitmaps.HarvesterClaas;
                case HarvesterBrand.JohnDeere:
                    return BrandBitmaps.HarvesterJohnDeere;
                case HarvesterBrand.NewHolland:
                    return BrandBitmaps.HarvesterNewHolland;
                default:
                    return BrandBitmaps.HarvesterAoG;
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
                    return BrandBitmaps.ArticulatedFrontCase;
                case ArticulatedBrand.Challenger:
                    return BrandBitmaps.ArticulatedFrontChallenger;
                case ArticulatedBrand.JohnDeere:
                    return BrandBitmaps.ArticulatedFrontJohnDeere;
                case ArticulatedBrand.NewHolland:
                    return BrandBitmaps.ArticulatedFrontNewHolland;
                case ArticulatedBrand.Holder:
                    return BrandBitmaps.ArticulatedFrontHolder;
                default:
                    return BrandBitmaps.ArticulatedFrontAoG;
            }
        }

        public static Bitmap GetRearBitmap(ArticulatedBrand brand)
        {
            switch (brand)
            {
                case ArticulatedBrand.Case:
                    return BrandBitmaps.ArticulatedRearCase;
                case ArticulatedBrand.Challenger:
                    return BrandBitmaps.ArticulatedRearChallenger;
                case ArticulatedBrand.JohnDeere:
                    return BrandBitmaps.ArticulatedRearJohnDeere;
                case ArticulatedBrand.NewHolland:
                    return BrandBitmaps.ArticulatedRearNewHolland;
                case ArticulatedBrand.Holder:
                    return BrandBitmaps.ArticulatedRearHolder;
                default:
                    return BrandBitmaps.ArticulatedRearAoG;
            }
        }
    }
}
