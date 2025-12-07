using AgOpenGPS.Core.Translations;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public enum SavingStepState { Pending, Done, Failed }

    public partial class FormSaving : Form
    {
        public FormSaving()
        {
            InitializeComponent();

            labelBeer.Text = "✔ " + gStr.gsSaveBeerTime;
        }

        public void AddStep(string key, string message)
        {
            listViewSteps.Items.Add(new ListViewItem()
            {
                Name = key,
                Text = GetStepText(message + "...", SavingStepState.Pending),
                ForeColor = GetStepColor(SavingStepState.Pending)
            });
        }

        public void UpdateStep(string key, string message, SavingStepState state)
        {
            listViewSteps.Items[key].Text = GetStepText(message, state);
            listViewSteps.Items[key].ForeColor = GetStepColor(state);
        }

        public void Finish()
        {
            progressBar.Visible = false;
            labelBeer.Visible = true;
        }

        private static string GetStepText(string message, SavingStepState state)
        {
            switch (state)
            {
                case SavingStepState.Pending:
                    return "• " + message;
                case SavingStepState.Done:
                    return "✓ " + message;
                case SavingStepState.Failed:
                    return "✗ " + message;
                default:
                    throw new InvalidEnumArgumentException(nameof(state), (int)state, typeof(SavingStepState));
            }
        }

        private static Color GetStepColor(SavingStepState state)
        {
            return state == SavingStepState.Pending ? Color.Gray : Color.Black;
        }
    }
}
