using AgOpenGPS.Core.Translations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSaving : Form
    {
        private List<Color> itemColors = new List<Color>();

        public FormSaving()
        {
            InitializeComponent();
            lstSteps.DrawMode = DrawMode.OwnerDrawFixed;
            lstSteps.DrawItem += LstSteps_DrawItem;
        }

        public void InitializeSteps(bool isJobStarted)
        {
            lstSteps.Items.Clear();
            itemColors.Clear();

            if (isJobStarted)
            {
                AddStep(ShutdownSteps.SaveParams);
                AddStep(ShutdownSteps.SaveField);
                AddStep(ShutdownSteps.SaveSettings);
                AddStep(ShutdownSteps.Finalizing);
            }
            else
            {
                AddStep(ShutdownSteps.SaveSettings);
                AddStep(ShutdownSteps.Finalizing);
            }
        }

        private void AddStep(string stepText)
        {
            lstSteps.Items.Add(stepText);
            itemColors.Add(Color.Gray);
        }

        public void UpdateStep(int index, string newText)
        {
            if (index >= 0 && index < lstSteps.Items.Count)
            {
                lstSteps.Items[index] = newText;
                itemColors[index] = Color.Black;
                lstSteps.Invalidate();
            }
        }

        public void InsertStep(int index, string text)
        {
            if (index >= 0 && index <= lstSteps.Items.Count)
            {
                lstSteps.Items.Insert(index, text);
                itemColors.Insert(index, Color.Gray);
                lstSteps.Invalidate();
            }
        }

        public void AddFinalMessage()
        {
            lstSteps.Items.Add("");
            itemColors.Add(Color.Gray);

            lstSteps.Items.Add(ShutdownSteps.Beer);
            itemColors.Add(Color.Black);
        }

        private void LstSteps_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lstSteps.Items.Count)
                return;

            e.DrawBackground();

            Color textColor = itemColors.Count > e.Index ? itemColors[e.Index] : Color.Black;
            using (Brush brush = new SolidBrush(textColor))
            {
                e.Graphics.DrawString(
                    lstSteps.Items[e.Index].ToString(),
                    e.Font,
                    brush,
                    e.Bounds
                );
            }

            e.DrawFocusRectangle();
        }
    }

    public static class ShutdownSteps
    {
        public static string SaveParams => "• " + gStr.gsSaveFieldParam;
        public static string SaveField => "• " + gStr.gsSaveField;
        public static string SaveSettings => "• " + gStr.gsSaveSettings;
        public static string Finalizing => "• " + gStr.gsSaveFinalizeShutdown;

        public static string UploadAgShare => "• " + gStr.gsSaveUploadToAgshare;
        public static string UploadDone => "✓ " + gStr.gsSaveUploadCompleted;
        public static string UploadFailed => "✗ " + gStr.gsSaveUploadFailed;

        public static string ParamsDone => "✓ " + gStr.gsSaveFieldParamSaved;
        public static string FieldSaved => "✓ " + gStr.gsSaveFieldSavedLocal;
        public static string SettingsSaved => "✓ " + gStr.gsSaveSettingsSaved;
        public static string AllDone => "✔ " + gStr.gsSaveAllDone;
        public static string Beer => "✔ " + gStr.gsSaveBeerTime;
    }
}
