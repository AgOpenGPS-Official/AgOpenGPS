using System;
using System.Text;
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
                lblProfileDetails.Text = "No profiles yet.\r\n\r\nTap New profile to make one.";
                return;
            }

            NtripProfileSet set = store.Load(name);
            if (set == null)
            {
                lblProfileDetails.Text = "This profile file could not be read.";
                return;
            }

            var sb = new StringBuilder();
            if (string.Equals(name, activeName, StringComparison.OrdinalIgnoreCase)) sb.Append("In use now\r\n\r\n");

            sb.Append("Main service\r\n");
            if (set.Main.IsComplete)
            {
                sb.Append(set.Main.Host).Append(':').Append(set.Main.Port).Append("\r\n");
                sb.Append(set.Main.Mount.Length > 0 ? "Mountpoint: " + set.Main.Mount : "No mountpoint").Append("\r\n");
            }
            else
            {
                sb.Append("Not filled in\r\n");
            }

            sb.Append("\r\nFallback\r\n");
            FallbackDefinition f = set.Fallback;
            if (!f.Enabled)
            {
                sb.Append("Off");
            }
            else if (f.Type == FallbackType.SerialRadio)
            {
                sb.Append("Serial radio on ").Append(f.RadioPort.Length > 0 ? f.RadioPort : "(no port)");
            }
            else if (f.Service.IsComplete)
            {
                sb.Append(f.Service.Host).Append(':').Append(f.Service.Port).Append("\r\n");
                sb.Append(f.Service.Mount.Length > 0 ? "Mountpoint: " + f.Service.Mount : "No mountpoint");
            }
            else
            {
                sb.Append("Not filled in");
            }
            lblProfileDetails.Text = sb.ToString();
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
            Fill("");
        }
    }
}
