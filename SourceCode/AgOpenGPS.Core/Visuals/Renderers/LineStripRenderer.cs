using AgOpenGPS.Core.DrawLib;

namespace AgOpenGPS.Core.Visuals
{
    public class LineStripRenderer : PrimitiveRenderer
    {

        public LineStripRenderer()
        {
        }

        public override void DrawPrimitive()
        {
            GLW.DrawLineStripArrays(_vertexBuffer);
        }

    }
}
