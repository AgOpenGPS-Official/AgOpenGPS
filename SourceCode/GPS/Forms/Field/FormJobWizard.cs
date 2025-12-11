using System;
using System.Windows.Forms;
using AgOpenGPS.Core.Translations;

namespace AgOpenGPS
{
    /// <summary>
    /// Job creation wizard - 3 step process: Machine -> Field -> Work Type
    /// </summary>
    public partial class FormJobWizard : Form
    {
        private readonly FormGPS mf;
        private int currentStep = 1;

        // Wizard selections
        public string SelectedProfile { get; private set; }
        public string SelectedFieldDirectory { get; private set; }
        public string SelectedWorkType { get; private set; }
        public string JobName { get; private set; }
        public CJob CreatedJob { get; private set; }

        public FormJobWizard(FormGPS callingForm)
        {
            mf = callingForm;
            InitializeComponent();
        }

        private void FormJobWizard_Load(object sender, EventArgs e)
        {
            // Initialize WorkTypes
            WorkTypes.Initialize(RegistrySettings.vehiclesDirectory);

            ShowStep(1);
        }

        private void ShowStep(int step)
        {
            currentStep = step;

            panelStep1.Visible = (step == 1);
            panelStep2.Visible = (step == 2);
            panelStep3.Visible = (step == 3);

            btnBack.Enabled = (step > 1);
            btnNext.Text = (step == 3) ? "START" : "Next >";
            lblStepTitle.Text = GetStepTitle(step);
        }

        private string GetStepTitle(int step)
        {
            switch (step)
            {
                case 1: return "Step 1: Select Machine Profile";
                case 2: return "Step 2: Select Field";
                case 3: return "Step 3: Select Work Type";
                default: return "";
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentStep < 3)
            {
                // Validate current step before proceeding
                if (ValidateCurrentStep())
                {
                    ShowStep(currentStep + 1);
                    LoadStepContent(currentStep);
                }
            }
            else
            {
                // Final step - create job and start
                if (ValidateCurrentStep())
                {
                    CreateJobAndStart();
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (currentStep > 1)
            {
                ShowStep(currentStep - 1);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateCurrentStep()
        {
            switch (currentStep)
            {
                case 1:
                    // For now, accept any selection (will be enhanced later)
                    SelectedProfile = "Default";
                    return true;
                case 2:
                    // For now, accept if field is entered
                    return true;
                case 3:
                    // For now, accept any work type
                    SelectedWorkType = "Other";
                    JobName = txtJobName.Text;
                    return !string.IsNullOrWhiteSpace(JobName);
            }
            return false;
        }

        private void LoadStepContent(int step)
        {
            switch (step)
            {
                case 2:
                    // Load field list (will be implemented fully later)
                    break;
                case 3:
                    // Generate default job name
                    txtJobName.Text = $"{DateTime.Now:yyyy-MM-dd}_{SelectedWorkType ?? "Work"}";
                    break;
            }
        }

        private void CreateJobAndStart()
        {
            // Create the job
            double implementWidth = mf.tool.width;
            CreatedJob = new CJob(
                SelectedFieldDirectory ?? mf.currentFieldDirectory,
                SelectedProfile,
                SelectedWorkType,
                implementWidth,
                JobName
            );

            // For now, just show a message - full integration will come later
            mf.TimedMessageBox(1500, "Job Created", $"Job: {CreatedJob.Name}");

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
