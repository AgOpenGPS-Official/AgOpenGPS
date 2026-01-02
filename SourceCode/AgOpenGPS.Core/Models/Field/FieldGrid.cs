namespace AgOpenGPS.Core.Models
{
    public class FieldGrid
    {
        public FieldGrid DeepCopy()
        {
            FieldGrid copy = new FieldGrid
            {
                GridAlignment = GridAlignment,
                GridStep = GridStep
            };
            return copy;
        }

        public GeoDir GridAlignment { get; set; }
        public double GridStep { get; set; }

    }

}
