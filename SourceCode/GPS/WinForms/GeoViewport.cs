using AgOpenGPS.Core.Drawing;
using AgOpenGPS.Core.Models;
using OpenTK;
using System.Drawing;

namespace AgOpenGPS.WinForms
{
    public class GeoViewport : GeoViewportBase
    {
        private readonly GLControl _glControl;
        public GeoViewport(
            GeoBoundingBox boundingBox,
            GLControl glControl
        )
            : base(boundingBox)
        {
            _glControl = glControl;
            _glControl.MakeCurrent();
            Initialize();
        }

        protected override void MakeCurrent()
        {
            _glControl.MakeCurrent();
        }

        public override void Refresh()
        {
            _glControl?.Refresh();
        }

        public override XyDelta ViewportSize
        {
            get
            {
                Size size = _glControl.Size;
                return new XyDelta(size.Width, size.Height);
            }
        }

        public override void EndPaint()
        {
            base.EndPaint();
            _glControl.SwapBuffers();
        }

    }

}
