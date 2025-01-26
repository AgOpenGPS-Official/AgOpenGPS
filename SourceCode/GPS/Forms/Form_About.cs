using AgOpenGPS.Culture;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class Form_About : Form
    {
        public Form_About()
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
            lblVersion.Text = "Version " + Program.SemVer;

            // Add a link to the LinkLabel.
            LinkLabel.Link link = new LinkLabel.Link { LinkData = "https://github.com/AgOpenGPS-Official/AgOpenGPS" };
            linkLabelGit.Links.Add(link);

            // Add a link to the LinkLabel.
            LinkLabel.Link linkCf = new LinkLabel.Link
            {
                LinkData = "https://discourse.agopengps.com/"
            };
            linkLabelCombineForum.Links.Add(linkCf);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setDisplay_isTermsAccepted = false;
            Properties.Settings.Default.Save();
        }

        private void btnVideo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(gStr.v_AboutIntro))
                System.Diagnostics.Process.Start(gStr.v_AboutIntro);
        }
    }
}