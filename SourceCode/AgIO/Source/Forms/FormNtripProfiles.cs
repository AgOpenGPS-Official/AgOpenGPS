using System;
using System.Windows.Forms;
using AgLibrary.Ntrip;

namespace AgIO
{
    internal enum ProfileAction
    {
        None,
        /// <summary>Start working with the profile now.</summary>
        Use,
        /// <summary>Open the profile in the full settings window.</summary>
        Edit,
    }

    /// <summary>
    /// The first thing the NTRIP setup shows: the saved profiles with a short summary of the selected one,
    /// so changing profile is quick. The full settings window is one tap away behind Edit.
    /// </summary>
    public partial class FormNtripProfiles : Form
    {
        private readonly FormLoop mf;
        private readonly string activeName;
        private readonly NtripProfileStore store = NtripProfiles.Store;

        internal ProfileAction Action { get; private set; }

        /// <summary>The profile in use was deleted, so the settings changed even if the window is cancelled.</summary>
        internal bool ActiveChanged { get; private set; }

        /// <summary>The profile the action is about.</summary>
        internal NtripProfileSet Chosen { get; private set; }

        internal FormNtripProfiles(FormLoop callingForm, string activeName)
        {
            mf = callingForm;
            this.activeName = NtripProfileStore.FileSafe(activeName ?? "");
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Fill(activeName);
        }

        private void Fill(string select)
        {
            lstProfiles.Items.Clear();
            foreach (string name in store.Names()) lstProfiles.Items.Add(name);
            int index = lstProfiles.Items.IndexOf(NtripProfileStore.FileSafe(select ?? ""));
            if (index >= 0) lstProfiles.SelectedIndex = index;
            else if (lstProfiles.Items.Count > 0) lstProfiles.SelectedIndex = 0;
            ShowDetails();
        }

        private string Selected()
        {
            return lstProfiles.SelectedItem as string;
        }

        private void ShowDetails()
        {
            string name = Selected();
            bool any = name != null;
            btnProfUse.Enabled = btnProfEdit.Enabled = btnProfCopy.Enabled = btnProfRename.Enabled = btnProfDelete.Enabled = any;

            if (!any)
            {
                profileSummary.SetProfile(null, false, "No profiles yet.\r\n\r\nTap New profile to make one.");
                return;
            }

            NtripProfileSet set = store.Load(name);
            if (set == null)
            {
                profileSummary.SetProfile(null, false, "This profile file could not be read.");
                return;
            }

            profileSummary.SetProfile(set, IsActive(name), "");
        }

        private bool IsActive(string name)
        {
            return string.Equals(name, activeName, StringComparison.OrdinalIgnoreCase);
        }

        // Big rows with a line between them; the chosen row is green.
        private void lstProfiles_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            string name = (string)lstProfiles.Items[e.Index];
            bool selected = (e.State & DrawItemState.Selected) != 0;

            using (var back = new System.Drawing.SolidBrush(selected ? System.Drawing.Color.FromArgb(21, 110, 75) : System.Drawing.Color.White))
                e.Graphics.FillRectangle(back, e.Bounds);
            using (var line = new System.Drawing.Pen(System.Drawing.Color.FromArgb(150, 150, 150), 1F))
                e.Graphics.DrawLine(line, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

            System.Drawing.Color ink = selected ? System.Drawing.Color.White : System.Drawing.Color.Black;
            bool active = IsActive(name);
            var textArea = new System.Drawing.Rectangle(e.Bounds.Left + 14, e.Bounds.Top, e.Bounds.Width - (active ? 110 : 28), e.Bounds.Height - 1);
            TextRenderer.DrawText(e.Graphics, name, e.Font, textArea, ink,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);

            if (active)
            {
                using (var small = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold))
                    TextRenderer.DrawText(e.Graphics, "IN USE", small, new System.Drawing.Rectangle(e.Bounds.Right - 92, e.Bounds.Top, 84, e.Bounds.Height - 1),
                        selected ? System.Drawing.Color.White : System.Drawing.Color.FromArgb(21, 110, 75),
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Right | TextFormatFlags.NoPadding);
            }
        }
        private string AskName(string start)
        {
            if (!mf.isKeyboardOn) return null;
            using (var keyboard = new FormKeyboard(start))
            {
                if (keyboard.ShowDialog(this) != DialogResult.OK) return null;
                string name = NtripProfileStore.FileSafe(keyboard.ReturnString);
                return name.Length == 0 ? null : name;
            }
        }

        private void Finish(ProfileAction action)
        {
            string name = Selected();
            if (name == null) return;

            NtripProfileSet set = store.Load(name);
            if (set == null)
            {
                mf.TimedMessageBox(3000, "Cannot read it", "This profile file could not be read.");
                return;
            }

            if (action == ProfileAction.Use && !set.Main.IsComplete)
            {
                mf.YesMessageBox("This profile is not filled in yet.\r\nTap Edit to fill in the main service.");
                return;
            }

            Chosen = set;
            Action = action;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void lstProfiles_SelectedIndexChanged(object sender, EventArgs e) { ShowDetails(); }
        private void btnProfUse_Click(object sender, EventArgs e) { Finish(ProfileAction.Use); }
        private void btnProfEdit_Click(object sender, EventArgs e) { Finish(ProfileAction.Edit); }
        private void lstProfiles_DoubleClick(object sender, EventArgs e) { Finish(ProfileAction.Use); }

        private void btnProfNew_Click(object sender, EventArgs e)
        {
            string name = AskName("");
            if (name == null) return;

            // Created here, saved when the settings are saved.
            Chosen = new NtripProfileSet { Name = store.UniqueName(name) };
            Action = ProfileAction.Edit;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnProfCopy_Click(object sender, EventArgs e)
        {
            string name = Selected();
            NtripProfileSet set = name == null ? null : store.Load(name);
            if (set == null) return;

            set.Name = store.UniqueName(name + " copy");
            store.Save(set);
            Fill(set.Name);
        }

        private void btnProfRename_Click(object sender, EventArgs e)
        {
            string name = Selected();
            if (name == null) return;

            string next = AskName(name);
            if (next == null) return;
            if (!store.Rename(name, next))
            {
                mf.TimedMessageBox(3000, "Cannot rename", "That name is already used.");
                return;
            }

            if (string.Equals(Properties.Settings.Default.setNTRIP_profileName, name, StringComparison.OrdinalIgnoreCase))
                Properties.Settings.Default.setNTRIP_profileName = next;
            Fill(next);
        }

        private void btnProfDelete_Click(object sender, EventArgs e)
        {
            string name = Selected();
            if (name == null) return;

            using (var ask = new FormYes("Delete the NTRIP profile \"" + name + "\"?", true))
            {
                if (ask.ShowDialog(this) != DialogResult.OK) return;
            }
            store.Delete(name);

            // The profile in use is gone: point to another one, or to none (then the old loose settings apply).
            var settings = Properties.Settings.Default;
            if (string.Equals(settings.setNTRIP_profileName, name, StringComparison.OrdinalIgnoreCase))
            {
                var rest = new System.Collections.Generic.List<string>(store.Names());
                settings.setNTRIP_profileName = rest.Count > 0 ? rest[0] : "";
                // No profile left: NTRIP is off until the user makes a new one.
                if (rest.Count == 0) settings.setNTRIP_isOn = false;
                settings.Save();
                ActiveChanged = true;
            }
            Fill("");
        }
    }
}
