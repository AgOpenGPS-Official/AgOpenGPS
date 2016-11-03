﻿//Please, if you use this, share the improvements


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSettings : Form
    {
       //class variables
        private FormGPS mainForm = null;

        //constructor
        public FormSettings(Form callingForm, int page)
        {
            //get copy of the calling main form
            mainForm = callingForm as FormGPS;
            InitializeComponent();

            //select the page as per calling menu or button from mainGPS form
            tabControl1.SelectedIndex = page;
        }

        //do any field initializing for form here
        private void FormSettings_Load(object sender, EventArgs e)
        {

            //check if GPS port is open or closed and set buttons accordingly
            if (mainForm.sp.IsOpen)
            {
                cboxBaud.Enabled = false;
                cboxPort.Enabled = false;
                btnCloseSerial.Enabled = true;
                btnOpenSerial.Enabled = false;
            }

            else
            {
                cboxBaud.Enabled = true;
                cboxPort.Enabled = true;
                btnCloseSerial.Enabled = false;
                btnOpenSerial.Enabled = true;
            }

            //check if Arduino port is open or closed and set buttons accordingly
            if (mainForm.spArduino.IsOpen)
            {
                cboxPortArduino.Enabled = false;
                btnCloseSerialArduino.Enabled = true;
                btnOpenSerialArduino.Enabled = false;
            }

            else
            {
                cboxPortArduino.Enabled = true;
                btnCloseSerialArduino.Enabled = false;
                btnOpenSerialArduino.Enabled = true;
            }

            //load the port box with valid port names
            cboxPort.Items.Clear();
            cboxPortArduino.Items.Clear();
            foreach (String s in System.IO.Ports.SerialPort.GetPortNames()) 
            {
                cboxPort.Items.Add(s);
                cboxPortArduino.Items.Add(s);
            }

            lblCurrentBaud.Text = mainForm.sp.BaudRate.ToString();
            lblCurrentPort.Text = mainForm.sp.PortName;

            lblCurrentArduinoPort.Text = mainForm.spArduino.PortName;

            nudNMEAHz.Value = Properties.Settings.Default.setPort_NMEAHz;

             //set vehicle settings to what it is in the settings page
            nudOverlap.Value = (decimal)mainForm.vehicle.toolOverlap;
            OverlapUpdate();

            nudForeAft.Value = (decimal)mainForm.vehicle.toolForeAft;
            ForeAftUpdate();

            nudAntennaHeight.Value = (decimal)mainForm.vehicle.antennaHeight;
            AntennaHeightUpdate();

            if (mainForm.vehicle.isHitched) rboHitched.Checked = true;
            if (!mainForm.vehicle.isHitched) rboRigid.Checked = true;

            nudLookAhead.Value = (decimal)mainForm.vehicle.lookAhead;

            //sections set to settings page
            nudNumberOfSections.Value = (decimal)mainForm.vehicle.numberOfSections;

            //total width of implement
            lblVehicleToolWidth.Text = Convert.ToString(mainForm.vehicle.toolWidth);

            //also update the total tool width in feet inches
            SectionFeetInchesTotalWidthLabelUpdate();

            //Enable or Disable the section distance spinners according to number of sections
            SectionEnDisSpinners( (int)nudNumberOfSections.Value);

            //Fill spinners with stored values
            nudSection1.Value = Properties.Settings.Default.setSection_nudSpin1;
            nudSection2.Value = Properties.Settings.Default.setSection_nudSpin2;
            nudSection3.Value = Properties.Settings.Default.setSection_nudSpin3;
            nudSection4.Value = Properties.Settings.Default.setSection_nudSpin4;
            nudSection5.Value = Properties.Settings.Default.setSection_nudSpin5;
            nudSection6.Value = Properties.Settings.Default.setSection_nudSpin6;

            ////load the delay slides
            //tbarDelayCamera.Value =  Properties.Settings.Default.setDelay_camera;
            //tbarDelaySection.Value = Properties.Settings.Default.setDelay_section;
            //tbarDelayVehicle.Value = Properties.Settings.Default.setDelay_vehicle;

            //lblDelayCamera.Text =  tbarDelayCamera.Value.ToString();
            //lblDelayVehicle.Text = tbarDelayVehicle.Value.ToString();
            //lblDelaySection.Text = tbarDelaySection.Value.ToString();

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //serial Ports  -  mostly done directly-------------------
            Properties.Settings.Default.setPort_NMEAHz = mainForm.rmcUpdateHz;
            
  
            //Vehicle settings ----------------
            mainForm.vehicle.toolForeAft = (double)nudForeAft.Value;
            mainForm.vehicle.lookAhead = (double)nudLookAhead.Value;
            mainForm.vehicle.antennaHeight = (double)nudAntennaHeight.Value;

            if (rboHitched.Checked) mainForm.vehicle.isHitched = true;
            else mainForm.vehicle.isHitched = false;

            //save the new set width in settings
            Properties.Settings.Default.setVehicle_toolWidth = mainForm.vehicle.toolWidth;
            Properties.Settings.Default.setVehicle_toolForeAft = mainForm.vehicle.toolForeAft;
            Properties.Settings.Default.setVehicle_antennaHeight = mainForm.vehicle.antennaHeight;
            Properties.Settings.Default.setVehicle_lookAhead = mainForm.vehicle.lookAhead;
            Properties.Settings.Default.setVehicle_isHitched = mainForm.vehicle.isHitched;
            Properties.Settings.Default.Save();

            //Guidance save the new set width in settings-------------------
            mainForm.vehicle.toolOverlap = (double)nudOverlap.Value;

            Properties.Settings.Default.setVehicle_toolOverlap = mainForm.vehicle.toolOverlap;
            Properties.Settings.Default.Save();

            //Sections ----------------------------------------
            //save the number of sections selected
            mainForm.vehicle.numberOfSections = (Convert.ToInt32(nudNumberOfSections.Value));

            //save the total number of sections in settings
            Properties.Settings.Default.setVehicle_numSections = (int)nudNumberOfSections.Value;
            Properties.Settings.Default.Save();

            //save the values in each spinner for section position widths in settings
            Properties.Settings.Default.setSection_nudSpin1 = nudSection1.Value;
            Properties.Settings.Default.setSection_nudSpin2 = nudSection2.Value;
            Properties.Settings.Default.setSection_nudSpin3 = nudSection3.Value;
            Properties.Settings.Default.setSection_nudSpin4 = nudSection4.Value;
            Properties.Settings.Default.setSection_nudSpin5 = nudSection5.Value;
            Properties.Settings.Default.setSection_nudSpin6 = nudSection6.Value;

            //update the sections to newly configured widths and positions in main
            mainForm.SectionSetPosition();

            //update the widths of sections and tool width in main
            mainForm.SectionCalcWidths();

            //Properties.Settings.Default.setDelay_camera  = tbarDelayCamera.Value; 
            //Properties.Settings.Default.setDelay_section = tbarDelaySection.Value;
            //Properties.Settings.Default.setDelay_vehicle = tbarDelayVehicle.Value;
            Properties.Settings.Default.Save();

            //back to FormGPS
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }

        #region PortSettings //----------------------------------------------------------------

         private void nudNMEAHz_ValueChanged(object sender, EventArgs e)
        {
            mainForm.rmcUpdateHz = (int)nudNMEAHz.Value;
        }

       // Arduino 
        private void btnRescanArduino_Click(object sender, EventArgs e)
        {
            cboxPortArduino.Items.Clear();
            foreach (String s in System.IO.Ports.SerialPort.GetPortNames()) { cboxPortArduino.Items.Add(s); }

        }

        private void btnOpenSerialArduino_Click(object sender, EventArgs e)
        {
            mainForm.SerialPortOpenArduino();
            if (mainForm.spArduino.IsOpen)
            {
                cboxPortArduino.Enabled = false;
                btnCloseSerialArduino.Enabled = true;
                btnOpenSerialArduino.Enabled = false;
                lblCurrentArduinoPort.Text = mainForm.spArduino.PortName;
            }

            else
            {
                cboxPortArduino.Enabled = true;
                btnCloseSerialArduino.Enabled = false;
                btnOpenSerialArduino.Enabled = true;
            }

        }

        private void btnCloseSerialArduino_Click(object sender, EventArgs e)
        {
            mainForm.SerialPortCloseArduino();
            if (mainForm.spArduino.IsOpen)
            {
                cboxPortArduino.Enabled = false;
                btnCloseSerialArduino.Enabled = true;
                btnOpenSerialArduino.Enabled = false;
            }

            else
            {
                cboxPortArduino.Enabled = true;
                btnCloseSerialArduino.Enabled = false;
                btnOpenSerialArduino.Enabled = true;
            }

        }

        private void cboxPortArduino_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            mainForm.spArduino.PortName = cboxPortArduino.Text;
            FormGPS.portNameArduino = cboxPortArduino.Text;

        }
         
        // GPS Serial Port
 
        private void cboxPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            mainForm.sp.PortName = cboxPort.Text;
            FormGPS.portName = cboxPort.Text;
 
        }

        private void cboxBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            mainForm.sp.BaudRate = Convert.ToInt32(cboxBaud.Text);
            FormGPS.baudRate = Convert.ToInt32(cboxBaud.Text);

        }

        private void btnOpenSerial_Click(object sender, EventArgs e)
        {
            mainForm.SerialPortOpenGPS();
            if (mainForm.sp.IsOpen)
            {
                cboxBaud.Enabled = false;
                cboxPort.Enabled = false;
                btnCloseSerial.Enabled = true;
                btnOpenSerial.Enabled = false;
                lblCurrentBaud.Text = mainForm.sp.BaudRate.ToString();
                lblCurrentPort.Text = mainForm.sp.PortName;
            }

            else
            {
                cboxBaud.Enabled = true;
                cboxPort.Enabled = true;
                btnCloseSerial.Enabled = false;
                btnOpenSerial.Enabled = true;
            }



        }

        private void btnCloseSerial_Click(object sender, EventArgs e)
        {
            mainForm.SerialPortCloseGPS();
            if (mainForm.sp.IsOpen)
            {
                cboxBaud.Enabled = false;
                cboxPort.Enabled = false;
                btnCloseSerial.Enabled = true;
                btnOpenSerial.Enabled = false;
            }

            else
            {
                cboxBaud.Enabled = true;
                cboxPort.Enabled = true;
                btnCloseSerial.Enabled = false;
                btnOpenSerial.Enabled = true;
            }
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            cboxPort.Items.Clear();
            foreach (String s in System.IO.Ports.SerialPort.GetPortNames()) { cboxPort.Items.Add(s); }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //GPS phrase
            textBoxRcv.Text = mainForm.recvSentenceSettings;
            mainForm.recvSentenceSettings = "";

            //Arduino phrase
            txtBoxRecvArduino.Text += mainForm.recvSentenceSettingsArduino;
            mainForm.recvSentenceSettingsArduino = "";

            if (txtBoxRecvArduino.TextLength > 40) txtBoxRecvArduino.Text = "";
            
        }

        #endregion PortSettings

        #region Vehicle //----------------------------------------------------------------

        private void ForeAftUpdate()
         {
             double toFeet = (Convert.ToDouble(nudForeAft.Value) * 3.28084);
             lblForeAftFeet.Text = Convert.ToString((int)toFeet) + "'";
             double temp = Math.Round((toFeet - Math.Truncate(toFeet)) / 0.08333, 0);
             lblForeAftInches.Text = Convert.ToString(temp) + '"';
         }
 
        private void nudForeAft_ValueChanged(object sender, EventArgs e)
        {
            ForeAftUpdate();
            if (nudForeAft.Value < 0) lblAntennaWhere.Text = "AFT (Behind)";
            else                        lblAntennaWhere.Text = "Fore (Ahead)";
        }

        private void AntennaHeightUpdate()
        {
            double toFeet = (Convert.ToDouble(nudAntennaHeight.Value) * 3.28084);
            lblAntennaFeet.Text = Convert.ToString((int)toFeet) + "'";
            double temp = Math.Round((toFeet - Math.Truncate(toFeet)) / 0.08333, 0);
            lblAntennaInches.Text = Convert.ToString(temp) + '"';           
        }

        private void nudAntennaHeight_ValueChanged(object sender, EventArgs e)
        {
            AntennaHeightUpdate();
        }

        private void btnFileOpenVehicle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            mainForm.FileOpenVehicle();
        }

        private void btnFileSaveVehicle_Click(object sender, EventArgs e)
        {
            mainForm.FileSaveVehicle();
        }

        #endregion Vehicle

        #region Guidance //----------------------------------------------------------------

        private void OverlapUpdate()
        {
            double toFeet = (Convert.ToDouble(nudOverlap.Value) * 3.28084);
            lblOverlapFeet.Text = Convert.ToString((int)toFeet) + "'";
            double temp = Math.Round((toFeet - Math.Truncate(toFeet)) / 0.08333, 0);
            lblOverlapInches.Text = Convert.ToString(temp)+'"';
        }

        private void nudOverlap_ValueChanged(object sender, EventArgs e)
        {
            OverlapUpdate();
        }


#endregion Guidance

        #region Sections //----------------------------------------------------------------

        //well that's pretty self explanatory
        private void SectionFeetInchesTotalWidthLabelUpdate()
        {
            double toFeet = (Convert.ToDouble(lblVehicleToolWidth.Text) * 3.28084);
            lblSecTotalWidthFeet.Text = Convert.ToString((int)toFeet) + "'";
            double temp = Math.Round((toFeet - Math.Truncate(toFeet)) / 0.08333, 0);
            lblSecTotalWidthInches.Text = Convert.ToString(temp) + '"';
        }

        //enable or disable section width spinners based on number selected
        public void SectionEnDisSpinners(int i)
        {
            switch (i)
            {
                case 1:
                    {
                        nudSection1.Enabled = true;
                        nudSection2.Enabled = true;
                        nudSection3.Enabled = false;
                        nudSection3.Value = 0;
                        nudSection3.Visible = false;
                        nudSection4.Enabled = false;
                        nudSection4.Value = 0;
                        nudSection4.Visible = false;
                        nudSection5.Enabled = false;
                        nudSection5.Value = 0;
                        nudSection5.Visible = false;
                        nudSection6.Enabled = false;
                        nudSection6.Value = 0;
                        nudSection6.Visible = false;
                        progressBar1.Value = 10 * i;
                        lblVehicleToolWidth.Text = Convert.ToString(nudSection2.Value - nudSection1.Value); 
                        break;
                    }
                case 2:
                    {
                        nudSection1.Enabled = true;
                        nudSection2.Enabled = true;
                        nudSection3.Enabled = true;
                        nudSection3.Visible = true;
                        nudSection4.Enabled = false;
                        nudSection4.Visible = false;
                        nudSection4.Value = 0;
                        nudSection5.Enabled = false;
                        nudSection5.Value = 0;
                        nudSection5.Visible = false;
                        nudSection6.Enabled = false;
                        nudSection6.Value = 0;
                        nudSection6.Visible = false;
                        progressBar1.Value = 10 * i;
                        lblVehicleToolWidth.Text = Convert.ToString(nudSection3.Value - nudSection1.Value);
                        break;
                    }

                case 3:
                    {
                        nudSection1.Enabled = true;
                        nudSection2.Enabled = true;
                        nudSection3.Enabled = true;
                        nudSection3.Visible = true;
                        nudSection4.Visible = true;
                        nudSection4.Enabled = true;
                        nudSection5.Enabled = false;
                        nudSection5.Value = 0;
                        nudSection5.Visible = false;
                        nudSection6.Enabled = false;
                        nudSection6.Value = 0;
                        nudSection6.Visible = false;
                        progressBar1.Value = 10 * i;
                        lblVehicleToolWidth.Text = Convert.ToString(nudSection4.Value - nudSection1.Value);
                        break;
                    }

                case 4:
                    {
                        nudSection1.Enabled = true;
                        nudSection2.Enabled = true;
                        nudSection3.Enabled = true;
                        nudSection4.Enabled = true;
                        nudSection5.Enabled = true;
                        nudSection3.Visible = true;
                        nudSection4.Visible = true;
                        nudSection5.Visible = true;
                        nudSection6.Enabled = false;
                        nudSection6.Visible = false;
                        nudSection6.Value = 0;
                        progressBar1.Value = 10 * i;
                        lblVehicleToolWidth.Text = Convert.ToString(nudSection5.Value - nudSection1.Value);
                        break;
                    }

                case 5:
                    {
                        nudSection1.Enabled = true;
                        nudSection2.Enabled = true;
                        nudSection3.Enabled = true;
                        nudSection4.Enabled = true;
                        nudSection5.Enabled = true;
                        nudSection6.Enabled = true;
                        nudSection3.Visible = true;
                        nudSection4.Visible = true;
                        nudSection5.Visible = true;
                        nudSection6.Visible = true;
                        progressBar1.Value = 10 * i;
                        lblVehicleToolWidth.Text = Convert.ToString(nudSection6.Value - nudSection1.Value);
                        break;
                    }

            }
        }

        //the inch values under the section width spinners
        public void SectionUpdateInchPositionLabels()
        {
            lblSection1Inch.Text = Convert.ToString(Math.Round(Convert.ToDouble(nudSection1.Value) * 39.3701, 0))+'"';
            lblSection2Inch.Text = Convert.ToString(Math.Round(Convert.ToDouble(nudSection2.Value) * 39.3701, 0))+'"';
            lblSection3Inch.Text = Convert.ToString(Math.Round(Convert.ToDouble(nudSection3.Value) * 39.3701, 0)) + '"';
            if (lblSection3Inch.Text == "0" + '"') lblSection3Inch.Text = "";
            lblSection4Inch.Text = Convert.ToString(Math.Round(Convert.ToDouble(nudSection4.Value) * 39.3701, 0)) + '"';
            if (lblSection4Inch.Text == "0" + '"') lblSection4Inch.Text = "";
            lblSection5Inch.Text = Convert.ToString(Math.Round(Convert.ToDouble(nudSection5.Value) * 39.3701, 0)) + '"';
            if (lblSection5Inch.Text == "0" + '"') lblSection5Inch.Text = "";
            lblSection6Inch.Text = Convert.ToString(Math.Round(Convert.ToDouble(nudSection6.Value) * 39.3701, 0)) + '"';
            if (lblSection6Inch.Text == "0"+'"') lblSection6Inch.Text = "";

            //based on number of sections, set accordingly
            int i = (int)nudNumberOfSections.Value;
            switch (i)
            {
                case 1:
                    {
                        lblVehicleToolWidth.Text = Convert.ToString(nudSection2.Value - nudSection1.Value);
                        lblSection2Width.Text = "";
                        lblSection3Width.Text = "";
                        lblSection4Width.Text = "";
                        lblSection5Width.Text = "";
                        lblSection2WidthM.Text = "";
                        lblSection3WidthM.Text = "";
                        lblSection4WidthM.Text = "";
                        lblSection5WidthM.Text = "";
                        lblSection1Width.Text = Convert.ToString(Math.Round(((float)nudSection2.Value - (float)nudSection1.Value) * 39.3701, 0)) + '"';
                        lblSection1WidthM.Text = Convert.ToString(Math.Round(((float)nudSection2.Value - (float)nudSection1.Value), 2));
                        break;
                    }
                case 2:
                    {
                        lblVehicleToolWidth.Text = Convert.ToString(nudSection3.Value - nudSection1.Value);
                        lblSection1Width.Text = Convert.ToString(Math.Round(((float)nudSection2.Value - (float)nudSection1.Value) * 39.3701, 0)) + '"';
                        lblSection2Width.Text = Convert.ToString(Math.Round(((float)nudSection3.Value - (float)nudSection2.Value) * 39.3701, 0)) + '"';
                        lblSection3Width.Text = "";
                        lblSection4Width.Text = "";
                        lblSection5Width.Text = "";
                        lblSection3WidthM.Text = "";
                        lblSection4WidthM.Text = "";
                        lblSection5WidthM.Text = "";
                        lblSection1WidthM.Text = Convert.ToString(Math.Round(((float)nudSection2.Value - (float)nudSection1.Value), 2));
                        lblSection2WidthM.Text = Convert.ToString(Math.Round(((float)nudSection3.Value - (float)nudSection2.Value), 2));
                        break;
                    }
                case 3:
                    {
                        lblVehicleToolWidth.Text = Convert.ToString(nudSection4.Value - nudSection1.Value);
                        lblSection1Width.Text = Convert.ToString(Math.Round(((float)nudSection2.Value - (float)nudSection1.Value) * 39.3701, 0)) + '"';
                        lblSection2Width.Text = Convert.ToString(Math.Round(((float)nudSection3.Value - (float)nudSection2.Value) * 39.3701, 0)) + '"';
                        lblSection3Width.Text = Convert.ToString(Math.Round(((float)nudSection4.Value - (float)nudSection3.Value) * 39.3701, 0)) + '"';
                        lblSection4Width.Text = "";
                        lblSection5Width.Text = "";
                        lblSection4WidthM.Text = "";
                        lblSection5WidthM.Text = "";
                        lblSection1WidthM.Text = Convert.ToString(Math.Round(((float)nudSection2.Value - (float)nudSection1.Value), 2));
                        lblSection2WidthM.Text = Convert.ToString(Math.Round(((float)nudSection3.Value - (float)nudSection2.Value), 2));
                        lblSection3WidthM.Text = Convert.ToString(Math.Round(((float)nudSection4.Value - (float)nudSection3.Value), 2));
                        break;
                    }
                case 4:
                    {
                        lblVehicleToolWidth.Text = Convert.ToString(nudSection5.Value - nudSection1.Value);
                        lblSection1Width.Text = Convert.ToString(Math.Round(((float)nudSection2.Value - (float)nudSection1.Value) * 39.3701, 0)) + '"';
                        lblSection2Width.Text = Convert.ToString(Math.Round(((float)nudSection3.Value - (float)nudSection2.Value) * 39.3701, 0)) + '"';
                        lblSection3Width.Text = Convert.ToString(Math.Round(((float)nudSection4.Value - (float)nudSection3.Value) * 39.3701, 0)) + '"';
                        lblSection4Width.Text = Convert.ToString(Math.Round(((float)nudSection5.Value - (float)nudSection4.Value) * 39.3701, 0)) + '"';
                        lblSection5Width.Text = "";
                        lblSection5WidthM.Text = "";
                        lblSection1WidthM.Text = Convert.ToString(Math.Round(((float)nudSection2.Value - (float)nudSection1.Value), 2));
                        lblSection2WidthM.Text = Convert.ToString(Math.Round(((float)nudSection3.Value - (float)nudSection2.Value), 2));
                        lblSection3WidthM.Text = Convert.ToString(Math.Round(((float)nudSection4.Value - (float)nudSection3.Value), 2));
                        lblSection4WidthM.Text = Convert.ToString(Math.Round(((float)nudSection5.Value - (float)nudSection4.Value), 2));
                        break;
                    }
                case 5:
                    {
                        lblVehicleToolWidth.Text = Convert.ToString(nudSection6.Value - nudSection1.Value);
                        lblSection1Width.Text = Convert.ToString(Math.Round(((float)nudSection2.Value - (float)nudSection1.Value) * 39.3701, 0)) + '"';
                        lblSection2Width.Text = Convert.ToString(Math.Round(((float)nudSection3.Value - (float)nudSection2.Value) * 39.3701, 0)) + '"';
                        lblSection3Width.Text = Convert.ToString(Math.Round(((float)nudSection4.Value - (float)nudSection3.Value) * 39.3701, 0)) + '"';
                        lblSection4Width.Text = Convert.ToString(Math.Round(((float)nudSection5.Value - (float)nudSection4.Value) * 39.3701, 0)) + '"';
                        lblSection5Width.Text = Convert.ToString(Math.Round(((float)nudSection6.Value - (float)nudSection5.Value) * 39.3701, 0)) + '"';
                        lblSection1WidthM.Text = Convert.ToString(Math.Round(((float)nudSection2.Value - (float)nudSection1.Value), 2));
                        lblSection2WidthM.Text = Convert.ToString(Math.Round(((float)nudSection3.Value - (float)nudSection2.Value), 2));
                        lblSection3WidthM.Text = Convert.ToString(Math.Round(((float)nudSection4.Value - (float)nudSection3.Value), 2));
                        lblSection4WidthM.Text = Convert.ToString(Math.Round(((float)nudSection5.Value - (float)nudSection4.Value), 2));
                        lblSection5WidthM.Text = Convert.ToString(Math.Round(((float)nudSection6.Value - (float)nudSection5.Value), 2));
                        break;
                    }
            }

            //update in settings dialog ONLY total tool width
            SectionFeetInchesTotalWidthLabelUpdate();

        }

        //Every time the # of Sections is spun
        private void nudNumberOfSections_ValueChanged(object sender, EventArgs e)
        {
            SectionEnDisSpinners((int)nudNumberOfSections.Value);
            SectionUpdateInchPositionLabels();
            lblTractor.Left = ((int)nudNumberOfSections.Value-1) * 81 + 76;

        }

        //Did user spin a section distance spinner?
        private void nudSection1_ValueChanged(object sender, EventArgs e)
        {
            SectionUpdateInchPositionLabels();
        }

        private void nudSection2_ValueChanged(object sender, EventArgs e)
        {
            SectionUpdateInchPositionLabels();
        }

        private void nudSection3_ValueChanged(object sender, EventArgs e)
        {
            SectionUpdateInchPositionLabels();
        }

        private void nudSection4_ValueChanged(object sender, EventArgs e)
        {
            SectionUpdateInchPositionLabels();
        }

        private void nudSection5_ValueChanged(object sender, EventArgs e)
        {
            SectionUpdateInchPositionLabels();
        }

        private void nudSection6_ValueChanged(object sender, EventArgs e)
        {
            SectionUpdateInchPositionLabels();
        }

        #endregion Sections

 
 
    }
}
 