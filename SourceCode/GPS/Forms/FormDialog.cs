using System;
using System.Windows.Forms;

namespace AgOpenGPS.Forms
{
    public partial class FormDialog : Form
    {
        public FormDialog()
        {
            InitializeComponent();
            ConfigureButtons(MessageBoxButtons.OKCancel);
            panelOptions.Visible = false; // hide
        }

        public static DialogResult Show(string title, string message, MessageBoxButtons buttons = MessageBoxButtons.OKCancel)
        {
            using (var dlg = new FormDialog())
            {
                dlg.SetTitleAndMessage(title, message);
                dlg.ConfigureButtons(buttons);
                dlg.panelOptions.Visible = false;
                return dlg.ShowDialog();
            }
        }
        public void Configure(string title, string message, MessageBoxButtons buttons, bool showOptions, bool saveNudgeDefault, bool cleanAppliedDefault)
        {
            SetTitleAndMessage(title, message);
            ConfigureButtons(buttons);
            ConfigureOptions(showOptions, saveNudgeDefault, cleanAppliedDefault);
        }

        public bool SaveNudgeChecked
        {
            get { return chkSaveNudge.Checked; }
        }

        public bool CleanAppliedAreaChecked
        {
            get { return chkCleanAppliedArea.Checked; }
        }


        // Set title and message labels
        private void SetTitleAndMessage(string title, string message)
        {
            lblTitle.Text = title ?? string.Empty;
            lblMessage.Text = message ?? string.Empty;
        }

        private void ConfigureButtons(MessageBoxButtons buttons)
        {
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
                    btnCancel.Visible = false; 
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

        private void ConfigureOptions(bool showOptions, bool saveNudgeDefault, bool cleanAppliedDefault)
        {
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
