using AgOpenGPS.Core.Drawing;
using AgOpenGPS.Core.DrawLib;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Visuals
{
    public static class VehicleDotVisual
    {
        public static void DrawVehicleDot(GeoCoord dotCoord)
        {
            GLW.SetPointSize(16.0f);
            GLW.SetColor(Colors.Red);
            GLW.DrawPoint(dotCoord);

            GLW.SetPointSize(8.0f);
            GLW.SetColor(Colors.White);
            GLW.DrawPoint(dotCoord);
        }
    }
}
