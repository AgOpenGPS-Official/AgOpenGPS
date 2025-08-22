using System;
using System.Windows.Forms;

namespace AgOpenGPS.Forms
{
    public partial class FormDialog : Form
    {
        // Parameterless constructor as per project convention
        public FormDialog()
        {
            InitializeComponent();
            // All comment lines explain the function with a short text
            // Ensure default visual state
            ConfigureButtons(MessageBoxButtons.OKCancel);
            panelOptions.Visible = false; // hide options by default
        }

        // Backward-compatible static helper for existing usages
        public static DialogResult Show(string title, string message, MessageBoxButtons buttons = MessageBoxButtons.OKCancel)
        {
            // All comment lines explain the function with a short text
            using (var dlg = new FormDialog())
            {
                dlg.SetTitleAndMessage(title, message);
                dlg.ConfigureButtons(buttons);
                dlg.panelOptions.Visible = false; // no options for classic Show
                return dlg.ShowDialog();
            }
        }

        // Configure dialog content, buttons and options for flexible scenarios
        public void Configure(string title, string message, MessageBoxButtons buttons, bool showOptions, bool saveNudgeDefault, bool cleanAppliedDefault)
        {
            // All comment lines explain the function with a short text
            SetTitleAndMessage(title, message);
            ConfigureButtons(buttons);
            ConfigureOptions(showOptions, saveNudgeDefault, cleanAppliedDefault);
        }

        // Expose the two option states to the caller
        public bool SaveNudgeChecked
        {
            get { return chkSaveNudge.Checked; }
        }

        public bool CleanAppliedAreaChecked
        {
            get { return chkCleanAppliedArea.Checked; }
        }

        // --- helpers (logic only) ---

        // Set title and message labels
        private void SetTitleAndMessage(string title, string message)
        {
            // All comment lines explain the function with a short text
            lblTitle.Text = title ?? string.Empty;
            lblMessage.Text = message ?? string.Empty;
        }

        // Map MessageBoxButtons to our two buttons and DialogResults
        private void ConfigureButtons(MessageBoxButtons buttons)
        {
            // All comment lines explain the function with a short text
            switch (buttons)
            {
                case MessageBoxButtons.YesNo:
                    btnOK.Text = "Yes";
                    btnOK.Visible = true;
                    btnOK.DialogResult = DialogResult.Yes;

                    btnCancel.Text = "No";
                    btnCancel.Visible = true;
                    btnCancel.DialogResult = DialogResult.No;

                    this.AcceptButton = btnOK;
                    this.CancelButton = btnCancel;
                    break;

                case MessageBoxButtons.OK:
                    btnOK.Text = "OK";
                    btnOK.Visible = true;
                    btnOK.DialogResult = DialogResult.OK;

                    btnCancel.Text = "Cancel";
                    btnCancel.Visible = false; // hide cancel when only OK
                    btnCancel.DialogResult = DialogResult.Cancel;

                    this.AcceptButton = btnOK;
                    this.CancelButton = btnCancel;
                    break;

                case MessageBoxButtons.OKCancel:
                default:
                    btnOK.Text = "OK";
                    btnOK.Visible = true;
                    btnOK.DialogResult = DialogResult.OK;

                    btnCancel.Text = "Cancel";
                    btnCancel.Visible = true;
                    btnCancel.DialogResult = DialogResult.Cancel;

                    this.AcceptButton = btnOK;
                    this.CancelButton = btnCancel;
                    break;
            }
        }

        // Show/hide the options panel and set default states
        private void ConfigureOptions(bool showOptions, bool saveNudgeDefault, bool cleanAppliedDefault)
        {
            // All comment lines explain the function with a short text
            panelOptions.Visible = showOptions;
            if (showOptions)
            {
                chkSaveNudge.Checked = saveNudgeDefault;
                chkCleanAppliedArea.Checked = cleanAppliedDefault;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            // Not used; DialogResult is already set on the button
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Not used; DialogResult is already set on the button
        }
    }
}
