//Please, if you use this give me some credit
//Copyright BrianTee, copy right out of it.

using System;
using System.Windows.Forms;
using AgOpenGPS.Culture;

namespace AgOpenGPS
{
    public partial class FormFieldData : Form
    {
        private readonly FormGPS mf = null;

        public FormFieldData(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
            labelTotal.Text = gStr.gsTotal;
            labelWorked.Text = gStr.gsWorked;
            labelApplied.Text = gStr.gsApplied;
            labelApplied2.Text = gStr.gsApplied;
            labelRemain.Text = gStr.gsRemain;
            labelRemain2.Text = gStr.gsRemain;
            labelOverlap.Text = gStr.gsOverlap;
            labelActual.Text = gStr.gsActual;   
            labelRate.Text = gStr.gsRate;   

        }
        private void FormFieldData_Load(object sender, EventArgs e)
        {
            timer1_Tick(this, EventArgs.Empty);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //lblEastingField.Text = Math.Round(mf.pn.fix.easting, 1).ToString();
            //lblNorthingField.Text = Math.Round(mf.pn.fix.northing, 1).ToString();

            lblOverlapPercent.Text = mf.fd.ActualOverlapPercent;

            if (mf.isMetric)
            {
                lblWorkRate.Text = mf.fd.WorkRateHectares;
                lblApplied.Text = mf.fd.WorkedHectares;
                lblActualLessOverlap.Text = mf.fd.ActualAreaWorkedHectares;

            }
            else
            {
                lblWorkRate.Text = mf.fd.WorkRateAcres;
                lblApplied.Text = mf.fd.WorkedAcres;
                lblActualLessOverlap.Text = mf.fd.ActualAreaWorkedAcres;
            }

            if (mf.bnd.bndList.Count > 0)
            {
                lblTimeRemaining.Text = mf.fd.TimeTillFinished;
                lblRemainPercent.Text = mf.fd.WorkedAreaRemainPercentage;

                if (mf.isMetric)
                {
                    lblTotalArea.Text = mf.fd.AreaBoundaryLessInnersHectares;
                    lblAreaRemain.Text = mf.fd.WorkedAreaRemainHectares;
                    lblActualRemain.Text = mf.fd.ActualRemainHectares;
                }
                else
                {
                    lblTotalArea.Text = mf.fd.AreaBoundaryLessInnersAcres;
                    lblAreaRemain.Text = mf.fd.WorkedAreaRemainAcres;
                    lblActualRemain.Text = mf.fd.ActualRemainAcres;
                }
            }
            else
            {
                lblTotalArea.Text = "-";
                lblAreaRemain.Text = "-";
                lblTimeRemaining.Text = "-";
                lblRemainPercent.Text = "-";

                //if (mf.isMetric) lblActualLessOverlap.Text = 
                //        ((100-mf.fd.overlapPercent) * 0.01 * mf.fd.workedAreaTotal * glm.m2ha).ToString("N2");
                //else
                //    lblActualLessOverlap.Text =
                //    ((100-mf.fd.overlapPercent) * 0.01 * mf.fd.workedAreaTotal * glm.m2ac).ToString("N2");
            }
        }

    }
}

//lblLookOnLeft.Text = mf.tool.lookAheadDistanceOnPixelsLeft.ToString("N0");
//lblLookOnRight.Text = mf.tool.lookAheadDistanceOnPixelsRight.ToString("N0");
//lblLookOffLeft.Text = mf.tool.lookAheadDistanceOffPixelsLeft.ToString("N0");
//lblLookOffRight.Text = mf.tool.lookAheadDistanceOffPixelsRight.ToString("N0");

//lblLeftToolSpd.Text = (mf.tool.toolFarLeftSpeed*3.6).ToString("N1");
//lblRightToolSpd.Text = (mf.tool.toolFarRightSpeed*3.6).ToString("N1");

//lblSectSpdLeft.Text = (mf.section[0].speedPixels*0.36).ToString("N1");
//lblSectSpdRight.Text = (mf.section[mf.tool.numOfSections-1].speedPixels*0.36).ToString("N1");