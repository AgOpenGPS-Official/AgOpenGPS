using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS.Controls
{
    // Source: https://github.com/intrueder/Control.Draggable
    public static class DraggableControlExtension
    {
        // TKey is control to drag, TValue is a flag used while dragging
        private static readonly Dictionary<Control, bool> _draggables = new Dictionary<Control, bool>();
        private static Size _mouseOffset;

        /// <summary>
        /// Enabling/disabling dragging for control
        /// </summary>
        public static void Draggable(this Control control, bool enable)
        {
            if (enable)
            {
                // enable drag feature
                if (_draggables.ContainsKey(control))
                {
                    // return if control is already draggable
                    return;
                }
                // 'false' - initial state is 'not dragging'
                _draggables.Add(control, false);

                // assign required event handlersnnn
                control.MouseDown += new MouseEventHandler(control_MouseDown);
                control.MouseUp += new MouseEventHandler(control_MouseUp);
                control.MouseMove += new MouseEventHandler(control_MouseMove);
            }
            else
            {
                // disable drag feature
                if (!_draggables.ContainsKey(control))
                {
                    // return if control is not draggable
                    return;
                }
                // remove event handlers
                control.MouseDown -= control_MouseDown;
                control.MouseUp -= control_MouseUp;
                control.MouseMove -= control_MouseMove;
                _draggables.Remove(control);
            }
        }

        private static void control_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseOffset = new Size(e.Location);
            // turning on dragging
            _draggables[(Control)sender] = true;
        }

        private static void control_MouseUp(object sender, MouseEventArgs e)
        {
            // turning off dragging
            _draggables[(Control)sender] = false;
        }

        private static void control_MouseMove(object sender, MouseEventArgs e)
        {
            // only if dragging is turned on
            if (_draggables[(Control)sender] == true)
            {
                // calculations of control's new position
                Point newLocationOffset = e.Location - _mouseOffset;
                ((Control)sender).Left += newLocationOffset.X;
                ((Control)sender).Top += newLocationOffset.Y;
            }
        }
    }
}
