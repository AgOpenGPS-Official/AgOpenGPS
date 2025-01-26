using System;
using System.Windows.Forms;
using AgOpenGPS.Culture;
using AgOpenGPS.Helpers;

namespace AgOpenGPS
{
    public partial class Form_First : Form
    {
        public Form_First(Form callingForm)
        {
            InitializeComponent();
            


        }

        private void linkLabelGit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void linkLabelCombineForum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void Form_About_Load(object sender, EventArgs e)
        {
            
            //translate labels
            labelAgree.Text = gStr.gsAgree;
            labelDisagree.Text = gStr.gsDisagree;
            labelTermsOne.Text = gStr.gsTermsOne;
            labelTerms2.Text = gStr.gsTermsTwo;
            labelTerms3.Text = gStr.gsTermsThree;
            labelTermsAndVersion.Text = gStr.gsTermsConditions + Program.SemVer;
            labelDiscussionsAt.Text = gStr.gsDiscussions;
            labelCheckForUpdates.Text = gStr.gsCheckForUpdates;


            // Add a link to the LinkLabel.
            LinkLabel.Link link = new LinkLabel.Link { LinkData = "https://github.com/AgOpenGPS-Official/AgOpenGPS" };
            linkLabelGit.Links.Add(link);

            // Add a link to the LinkLabel.
            LinkLabel.Link linkCf = new LinkLabel.Link
            {
                LinkData = "https://discourse.agopengps.com/"
            };
            linkLabelCombineForum.Links.Add(linkCf);

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }

            label1.Text = RegistrySettings.culture + " | " +
                RegistrySettings.vehiclesDirectory + " -> " + 
                RegistrySettings.vehicleFileName + ".xml";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setDisplay_isTermsAccepted = true;
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }
    }   
}