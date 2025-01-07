using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS.Helpers
{
    public static class ScreenHelper
    {
        /// <summary>
        /// Determines if a rectangle is fully shown across the total screen area of all monitors.
        /// </summary>
        /// <param name="rectancle">The rectangle to test</param>
        /// <returns>A boolean indicating if the rectangle is shown or not.</returns>
        public static bool IsOnScreen(Rectangle rectancle)
        {
            double visiblePixels = 0;

            foreach (Screen screen in Screen.AllScreens)
            {
                Rectangle intersection = Rectangle.Intersect(rectancle, screen.WorkingArea);
                // intersect rectangle with screen
                if (intersection.Width != 0 & intersection.Height != 0)
                {
                    visiblePixels += (intersection.Width * intersection.Height);
                    // tally visible pixels
                }
            }

            return visiblePixels >= (rectancle.Width * rectancle.Height);
        }
    }
}
