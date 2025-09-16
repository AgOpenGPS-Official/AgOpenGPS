using System;
using System.Windows.Forms;

namespace AgOpenGPS.Helpers
{
    public sealed class HotkeyMessageFilter : IMessageFilter, IDisposable
    {
        private readonly FormGPS _mf;

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        public HotkeyMessageFilter(FormGPS mf) => _mf = mf ?? throw new ArgumentNullException(nameof(mf));
        public bool Enabled { get; set; } = true;

        public bool PreFilterMessage(ref Message m)
        {
            if (!Enabled) return false;
            if (m.Msg != WM_KEYDOWN && m.Msg != WM_SYSKEYDOWN) return false;

            // if user in typing in textbox, ignore hotkey
            if (IsTypingContext()) return false;

            var key = (Keys)m.WParam.ToInt32();
            var mods = Control.ModifierKeys;

            // Let FormGPS do the handling
            return _mf.HandleAppWideKey(key, mods);
        }

        private static bool IsTypingContext()
        {
            var af = Form.ActiveForm;
            if (af == null) return false;
            var c = af.ActiveControl;
            while (c != null)
            {
                if (c is TextBoxBase) return true;
                c = c.Parent;
            }
            return false;
        }

        public void Dispose() { }
    }
}
