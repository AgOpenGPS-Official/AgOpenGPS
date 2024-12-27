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
            lblVersion.Text = "Version " + Application.ProductVersion.ToString(CultureInfo.InvariantCulture);

            // Add a link to the LinkLabel.
            LinkLabel.Link link = new LinkLabel.Link { LinkData = "https://github.com/farmerbriantee/AgOpenGPS" };
            linkLabelGit.Links.Add(link);

            // Add a link to the LinkLabel.
            LinkLabel.Link linkCf = new LinkLabel.Link
            {
                LinkData = "https://discourse.agopengps.com/"
            };
            linkLabelCombineForum.Links.Add(linkCf);

            this.Text = gStr.gsAboutAOG;

            label13.Text = gStr.gsTermsAndConditions;
            label7.Text = gStr.gsTermsParagraph1;
            label9.Text = gStr.gsTermsParagraph2;
            label10.Text = gStr.gsTermsParagraph3;
            label14.Text = gStr.gsThanks;
            label3.Text = gStr.gsDiscussion;
            label6.Text = gStr.gsCheckForUpdates;
            label8.Text = gStr.gsShortIntro;
            label15.Text = gStr.gsEnableTerms;

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