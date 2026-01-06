using AgOpenGPS.Core.DrawLib;

namespace AgOpenGPS.Core.Visuals
{
    public class LineLoopRenderer : PrimitiveRenderer
    {
        public LineLoopRenderer()
        {
        }

        public override void DrawPrimitive()
        {
            GLW.DrawLineLoopArrays(_vertexBuffer);
        }

    }
}
