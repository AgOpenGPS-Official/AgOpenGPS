using System;
using System.Windows.Forms;
using AgLibrary.Ntrip;

namespace AgIO
{
    /// <summary>The saved NTRIP profiles: use, create, copy, rename or delete one.</summary>
    public partial class FormNtripProfiles : Form
    {
        private readonly FormLoop mf;
        private readonly NtripProfileSet current;
        private readonly NtripProfileStore store = NtripProfiles.Store;

        /// <summary>The profile to continue with when the dialog ends with OK.</summary>
        internal NtripProfileSet Chosen { get; private set; }

        internal FormNtripProfiles(FormLoop callingForm, NtripProfileSet current)
        {
            mf = callingForm;
            this.current = current;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Fill(current.Name);
        }

        private void Fill(string select)
        {
            lstProfiles.Items.Clear();
            foreach (string name in store.Names()) lstProfiles.Items.Add(name);
            int index = lstProfiles.Items.IndexOf(NtripProfileStore.FileSafe(select ?? ""));
            if (index >= 0) lstProfiles.SelectedIndex = index;
            else if (lstProfiles.Items.Count > 0) lstProfiles.SelectedIndex = 0;
        }

        private string Selected()
        {
            return lstProfiles.SelectedItem as string;
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

        private void Use()
        {
            string name = Selected();
            if (name == null) return;

            NtripProfileSet set = store.Load(name);
            if (set == null)
            {
                mf.TimedMessageBox(3000, "Cannot read it", "This profile file could not be read.");
                return;
            }

            Chosen = set;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnProfUse_Click(object sender, EventArgs e) { Use(); }
        private void lstProfiles_DoubleClick(object sender, EventArgs e) { Use(); }

        private void btnProfNew_Click(object sender, EventArgs e)
        {
            string name = AskName("");
            if (name == null) return;

            // Created here, saved when the settings are saved.
            Chosen = new NtripProfileSet { Name = store.UniqueName(name) };
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
