using AgOpenGPS.Core.Drawing;
using AgOpenGPS.Core.Models;
using OpenTK;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS.WinForms
{
    public class GeoViewport : GeoViewportBase
    {
        private readonly GLControl _glControl;
        public GeoViewport(
            GeoBoundingBox boundingBox,
            GLControl glControl
        )
            : base()
        {
            _glControl = glControl;
            Initialize(boundingBox);
            _glControl.MouseDown += ViewportControlMouseDown;
        }

        protected override void MakeCurrent()
        {
            _glControl.MakeCurrent();
        }

        public override void Refresh()
        {
            _glControl?.Refresh();
        }

        public override ViewportSize2D ViewportSize
        {
            get
            {
                Size size = _glControl.Size;
                return new ViewportSize2D
                {
                    SizeX = size.Width,
                    SizeY = size.Height
                };
            }
        }

        public override void EndPaint()
        {
            base.EndPaint();
            _glControl.SwapBuffers();
        }

        private void ViewportControlMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Point pt = _glControl.PointToClient(Cursor.Position);
            XyCoord xyClient = new XyCoord(pt.X, pt.Y);
            GeoCoord mouseDownCoord = GetGeoCoord(xyClient);

            OnMouseDown(mouseDownCoord);
        }

    }

}
