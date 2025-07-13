using AgOpenGPS.Core.Translations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSaving : Form
    {
        public FormSaving()
        {
            InitializeComponent();
        }

        public void InitializeSteps(bool isJobStarted)
        {
            lstSteps.Items.Clear();

            if (isJobStarted)
            {
                lstSteps.Items.Add(ShutdownSteps.SaveParams);
                lstSteps.Items.Add(ShutdownSteps.SaveField);
                lstSteps.Items.Add(ShutdownSteps.SaveSettings);
                lstSteps.Items.Add(ShutdownSteps.Finalizing);
            }
            else
            {
                lstSteps.Items.Add(ShutdownSteps.SaveSettings);
                lstSteps.Items.Add(ShutdownSteps.Finalizing);
            }
        }

        public void UpdateStep(int index, string text)
        {
            if (index >= 0 && index < lstSteps.Items.Count)
                lstSteps.Items[index] = text;
        }

        public void InsertStep(int index, string text)
        {
            if (index >= 0 && index <= lstSteps.Items.Count)
                lstSteps.Items.Insert(index, text);
        }

        public void AddFinalMessage()
        {
            lstSteps.Items.Add("");
            lstSteps.Items.Add(ShutdownSteps.Beer);
        }
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
    public static string Beer => "🍺 " + gStr.gsSaveBeerTime;
}

