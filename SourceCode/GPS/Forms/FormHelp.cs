using System;
using System.Diagnostics;
using System.Windows.Forms;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Helpers;

namespace AgOpenGPS
{
    public partial class FormHelp : Form
    {
        private const string GitHubUrl = "https://github.com/AgOpenGPS-Official/AgOpenGPS";
        private const string DiscourseUrl = "https://discourse.agopengps.com";
        private const string YouTubeUrl = "https://www.youtube.com/@AgOpenGPS";

        public FormHelp()
        {
            InitializeComponent();
        }

        private void FormHelp_Load(object sender, EventArgs e)
        {
            // Translation
            Text = gStr.gsHelp;
            buttonGitHub.Text = gStr.gsCheckForUpdates;
            buttonDiscourse.Text = gStr.gsDiscourseForum;
            buttonYouTube.Text = gStr.gsYouTubeTutorials;
            buttonClose.Text = gStr.gsClose;

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