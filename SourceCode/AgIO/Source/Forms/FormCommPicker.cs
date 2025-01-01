using System;
using System.IO;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormCommPicker : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        public FormCommPicker(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();
        }

        private void FormCommPicker_Load(object sender, EventArgs e)
        {
            DirectoryInfo dinfo = new DirectoryInfo(FormLoop.profileDirectory);
            FileInfo[] Files = dinfo.GetFiles("*.xml");
            if (Files.Length == 0)
            {
                DialogResult = DialogResult.Ignore;
                Close();
                FormTimedMessage form = new FormTimedMessage(2000, "No Profiles To Load", "Save one First");
                form.Show();
            }
            else
            {
                foreach (FileInfo file in Files)
                {
                    string temp = Path.GetFileNameWithoutExtension(file.Name);

                    cboxEnv.Items.Add(temp);
                }
            }
        }

        private void cboxVeh_SelectedIndexChanged(object sender, EventArgs e)
        {
            var newname = cboxEnv.SelectedItem.ToString().Trim();
            Properties.RegistrySettings.Save("ProfileName", newname);
            Properties.Settings.Default.Load();
            DialogResult = DialogResult.OK;
        }
    }
}