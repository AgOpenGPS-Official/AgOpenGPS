using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Helpers;

namespace AgOpenGPS
{
    public partial class FormTermsAndConditions : Form
    {
        private const string GitHubUrl = "https://github.com/AgOpenGPS-Official/AgOpenGPS";
        private const string DiscourseUrl = "https://discourse.agopengps.com";
        private const string YouTubeUrl = "https://www.youtube.com/@AgOpenGPS";

        public FormTermsAndConditions()
        {
            InitializeComponent();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            var borderPen = new Pen(Brushes.CornflowerBlue, 10);
            e.Graphics.DrawRectangle(borderPen, 0, 0, Width, Height);
        }

        private void Form_About_Load(object sender, EventArgs e)
        {
            // Translations
            labelTermsAndConditions.Text = gStr.gsTermsConditions;
            labelVersion.Text = gStr.gsVersion + ":";
            labelTerms.Text = gStr.gsTerms;
            labelAgree.Text = gStr.gsAgree;
            labelDisagree.Text = gStr.gsDisagree;
            buttonGitHub.Text = gStr.gsCheckForUpdates;
            buttonDiscourse.Text = gStr.gsDiscourseForum;
            buttonYouTube.Text = gStr.gsYouTubeTutorials;

            labelVersionActual.Text = Program.SemVer;

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void buttonGitHub_Click(object sender, EventArgs e)
        {
            Process.Start(GitHubUrl);
        }

        private void buttonDiscourse_Click(object sender, EventArgs e)
        {
            Process.Start(DiscourseUrl);
        }

        private void buttonYouTube_Click(object sender, EventArgs e)
        {
            Process.Start(YouTubeUrl);
        }
    }
}