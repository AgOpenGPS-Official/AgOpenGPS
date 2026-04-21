// ============================================================================
// ModulePopupForm.cs - Popup borderless genérico para módulos AgroParallel
// Ubicación: SourceCode/GPS/AgroParallel/Common/ModulePopupForm.cs
// Target: net48 (C# 7.3)
//
// Hereda de BorderlessPopupForm (AgroParallel.VistaX.BorderlessPopupForm) y
// personaliza título + color de acento según el AgroParallelModuleEntry.
// Reutiliza toda la lógica de drag/resize/Escape-close del padre.
// ============================================================================

using AgroParallel.VistaX;
using System.Drawing;
using System.Windows.Forms;

namespace AgroParallel.Common
{
    public class ModulePopupForm : BorderlessPopupForm
    {
        public ModulePopupForm(AgroParallelModuleEntry module)
            : base(module.Url, SafeSize(module.PopupWidth, 1100), SafeSize(module.PopupHeight, 750))
        {
            ApplyModuleTitle(module);
        }

        private static int SafeSize(int requested, int fallback)
        {
            return requested > 0 ? requested : fallback;
        }

        private void ApplyModuleTitle(AgroParallelModuleEntry module)
        {
            Color accent = AgroParallelModulesConfig.ResolveAccentColor(module);

            string emoji = string.IsNullOrEmpty(module.Emoji) ? "" : module.Emoji + " ";
            string title = string.IsNullOrEmpty(module.PopupTitle) ? module.Name : module.PopupTitle;
            this.Text = title;

            // Buscar el titleBar (primer Panel Dock=Top) y su Label de título.
            foreach (Control c in this.Controls)
            {
                if (c is Panel && c.Dock == DockStyle.Top)
                {
                    foreach (Control inner in c.Controls)
                    {
                        if (inner is Label lbl)
                        {
                            lbl.Text = emoji + title;
                            lbl.ForeColor = accent;
                            return;
                        }
                    }
                }
            }
        }
    }
}
