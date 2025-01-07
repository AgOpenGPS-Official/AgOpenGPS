using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AgOpenGPS.Properties;
using System.Globalization;
using System.IO;
using System.Media;

namespace AgOpenGPS
{
    public enum btnStates { Off, Auto, On }

    public partial class FormGPS
    {
        //Off, Manual, and Auto, 3 states possible
        public btnStates manualBtnState = btnStates.Off;
        public btnStates autoBtnState = btnStates.Off;
        
        //Section Manual and Auto buttons on right side
        private void btnSectionMasterManual_Click(object sender, EventArgs e)
        {
            //System.Media.SystemSounds.Asterisk.Play();
            if (sounds.isSectionsSoundOn) sounds.sndSectionOff.Play();

            //if Auto is on, turn it off
            autoBtnState = btnStates.Off;
            btnSectionMasterAuto.Image = Properties.Resources.SectionMasterOff;

            switch (manualBtnState)
            {
                case btnStates.Off:
                    manualBtnState = btnStates.On;
                    btnSectionMasterManual.Image = Properties.Resources.ManualOn;
                    break;

                case btnStates.On:
                    manualBtnState = btnStates.Off;
                    btnSectionMasterManual.Image = Properties.Resources.ManualOff;
                    break;
            }

            //go set the butons and section states
            if (tool.isSectionsNotZones)
                AllSectionsAndButtonsToState(manualBtnState);
            else
                AllZonesAndButtonsToState(manualBtnState);
        }
        private void btnSectionMasterAuto_Click(object sender, EventArgs e)
        {
            //turn off manual if on
            manualBtnState = btnStates.Off;
            btnSectionMasterManual.Image = Properties.Resources.ManualOff;

            switch (autoBtnState)
            {

                case btnStates.Off:

                    autoBtnState = btnStates.Auto;
                    btnSectionMasterAuto.Image = Properties.Resources.SectionMasterOn;
                    if (sounds.isSectionsSoundOn)sounds.sndSectionOn.Play();
                    break;

                case btnStates.Auto:

                    autoBtnState = btnStates.Off;
                    btnSectionMasterAuto.Image = Properties.Resources.SectionMasterOff;
                    if (sounds.isSectionsSoundOn) sounds.sndSectionOn.Play();
                    break;
            }

            //go set the butons and section states
            if (tool.isSectionsNotZones)
                AllSectionsAndButtonsToState(autoBtnState);
            else
                AllZonesAndButtonsToState(autoBtnState);

        }

        //cycle thru states - Off,Auto,On
        private btnStates GetNextState(btnStates state)
        {
            if (state == btnStates.Off) return btnStates.Auto;
            else if (state == btnStates.Auto) return btnStates.On;
            else if (state == btnStates.On) return btnStates.Off;
            return btnStates.Off;
        }

        //zone buttons
        private void btnZone1_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[tool.zoneRanges[1]-1].sectionBtnState);
            IndividualZoneAndButtonToState(state, 0, tool.zoneRanges[1], btnZone1);
        }
        private void btnZone2_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[tool.zoneRanges[2]-1].sectionBtnState);
            IndividualZoneAndButtonToState(state, tool.zoneRanges[1], tool.zoneRanges[2], btnZone2);
        }
        private void btnZone3_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[tool.zoneRanges[3]-1].sectionBtnState);
            IndividualZoneAndButtonToState(state, tool.zoneRanges[2], tool.zoneRanges[3], btnZone3);
        }
        private void btnZone4_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[tool.zoneRanges[4]-1].sectionBtnState);
            IndividualZoneAndButtonToState(state, tool.zoneRanges[3], tool.zoneRanges[4], btnZone4);
        }
        private void btnZone5_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[tool.zoneRanges[5]-1].sectionBtnState);
            IndividualZoneAndButtonToState(state, tool.zoneRanges[4], tool.zoneRanges[5], btnZone5);
        }
        private void btnZone6_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[tool.zoneRanges[6]-1].sectionBtnState);
            IndividualZoneAndButtonToState(state, tool.zoneRanges[5], tool.zoneRanges[6], btnZone6);
        }
        private void btnZone7_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[tool.zoneRanges[7] - 1].sectionBtnState);
            IndividualZoneAndButtonToState(state, tool.zoneRanges[6], tool.zoneRanges[7], btnZone7);
        }
        private void btnZone8_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[tool.zoneRanges[8] - 1].sectionBtnState);
            IndividualZoneAndButtonToState(state, tool.zoneRanges[7], tool.zoneRanges[8], btnZone8);
        }

        //individual buttons for sections
        private void btnSection1Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[0].sectionBtnState);
            IndividualSectionAndButonToState(state, 0, btnSection1Man);
        }
        private void btnSection2Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[1].sectionBtnState);
            IndividualSectionAndButonToState(state, 1, btnSection2Man);
        }
        private void btnSection3Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[2].sectionBtnState);
            IndividualSectionAndButonToState(state, 2, btnSection3Man);
        }
        private void btnSection4Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[3].sectionBtnState);
            IndividualSectionAndButonToState(state, 3, btnSection4Man);
        }
        private void btnSection5Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[4].sectionBtnState);
            IndividualSectionAndButonToState(state, 4, btnSection5Man);
        }
        private void btnSection6Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[5].sectionBtnState);
            IndividualSectionAndButonToState(state, 5, btnSection6Man);
        }
        private void btnSection7Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[6].sectionBtnState);
            IndividualSectionAndButonToState(state, 6, btnSection7Man);
        }
        private void btnSection8Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[7].sectionBtnState);
            IndividualSectionAndButonToState(state, 7, btnSection8Man);
        }
        private void btnSection9Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[8].sectionBtnState);
            IndividualSectionAndButonToState(state, 8, btnSection9Man);
        }
        private void btnSection10Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[9].sectionBtnState);
            IndividualSectionAndButonToState(state, 9, btnSection10Man);
        }
        private void btnSection11Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[10].sectionBtnState);
            IndividualSectionAndButonToState(state, 10, btnSection11Man);
        }
        private void btnSection12Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[11].sectionBtnState);
            IndividualSectionAndButonToState(state, 11, btnSection12Man);
        }
        private void btnSection13Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[12].sectionBtnState);
            IndividualSectionAndButonToState(state, 12, btnSection13Man);
        }
        private void btnSection14Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[13].sectionBtnState);
            IndividualSectionAndButonToState(state, 13, btnSection14Man);
        }
        private void btnSection15Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[14].sectionBtnState);
            IndividualSectionAndButonToState(state, 14, btnSection15Man);
        }
        private void btnSection16Man_Click(object sender, EventArgs e)
        {
            btnStates state = GetNextState(section[15].sectionBtnState);
            IndividualSectionAndButonToState(state, 15, btnSection16Man);
        }

        //Section buttons************************8
        public void AllSectionsAndButtonsToState(btnStates state)
        {
            IndividualSectionAndButonToState(state, 0, btnSection1Man);
            IndividualSectionAndButonToState(state, 1, btnSection2Man);
            IndividualSectionAndButonToState(state, 2, btnSection3Man);
            IndividualSectionAndButonToState(state, 3, btnSection4Man);
            IndividualSectionAndButonToState(state, 4, btnSection5Man);
            IndividualSectionAndButonToState(state, 5, btnSection6Man);
            IndividualSectionAndButonToState(state, 6, btnSection7Man);
            IndividualSectionAndButonToState(state, 7, btnSection8Man);
            IndividualSectionAndButonToState(state, 8, btnSection9Man);
            IndividualSectionAndButonToState(state, 9, btnSection10Man);
            IndividualSectionAndButonToState(state, 10, btnSection11Man);
            IndividualSectionAndButonToState(state, 11, btnSection12Man);
            IndividualSectionAndButonToState(state, 12, btnSection13Man);
            IndividualSectionAndButonToState(state, 13, btnSection14Man);
            IndividualSectionAndButonToState(state, 14, btnSection15Man);
            IndividualSectionAndButonToState(state, 15, btnSection16Man);

        }

        private void IndividualSectionAndButonToState(btnStates state, int sectNumber, Button btn)
        {
            section[sectNumber].sectionBtnState = state;

            switch (section[sectNumber].sectionBtnState)
            {
                case btnStates.Off:
                    if (isDay)
                    {
                        btn.ForeColor = Color.Black;
                        btn.BackColor = Color.Red;
                    }
                    else
                    {
                        btn.BackColor = Color.Crimson;
                        btn.ForeColor = Color.White;
                    }
                    break;

                case btnStates.Auto:
                    if (isDay)
                    {
                        btn.BackColor = Color.Lime;
                        btn.ForeColor = Color.Black;
                    }
                    else
                    {
                        btn.BackColor = Color.ForestGreen;
                        btn.ForeColor = Color.White;
                    }
                    break;            

                case btnStates.On:
                    if (isDay)
                    {
                        btn.BackColor = Color.Yellow;
                        btn.ForeColor = Color.Black;
                    }
                    else
                    {
                        btn.BackColor = Color.DarkGoldenrod;
                        btn.ForeColor = Color.White;
                    }
                    break;
            }
        }

        public void LineUpIndividualSectionBtns()
        {
            //var matches = this.Controls.Find("btnZone1", true);

            if (!isJobStarted)
            {
                btnSection1Man.Visible = false;
                btnSection2Man.Visible = false;
                btnSection3Man.Visible = false;
                btnSection4Man.Visible = false;
                btnSection5Man.Visible = false;
                btnSection6Man.Visible = false;
                btnSection7Man.Visible = false;
                btnSection8Man.Visible = false;
                btnSection9Man.Visible = false;
                btnSection10Man.Visible = false;
                btnSection11Man.Visible = false;
                btnSection12Man.Visible = false;
                btnSection13Man.Visible = false;
                btnSection14Man.Visible = false;
                btnSection15Man.Visible = false;
                btnSection16Man.Visible = false;

                btnZone1.Visible = false;
                btnZone2.Visible = false;
                btnZone3.Visible = false;
                btnZone4.Visible = false;
                btnZone5.Visible = false;
                btnZone6.Visible = false;
                btnZone7.Visible = false;
                btnZone8.Visible = false;
                return;
            }


            btnZone1.Visible = false;
            btnZone2.Visible = false;
            btnZone3.Visible = false;
            btnZone4.Visible = false;
            btnZone5.Visible = false;
            btnZone6.Visible = false;
            btnZone7.Visible = false;
            btnZone8.Visible = false;


            int oglCenter = isPanelBottomHidden ? oglCenter = oglMain.Width / 2 + 30 : statusStripLeft.Width + oglMain.Width / 2;

            int top = 140;

            int buttonMaxWidth = 360, buttonHeight = 35;

            if ((Height - oglMain.Height) < 80) //max size - buttons hid
            {
                top = Height - 85;
                if (panelSim.Visible == true)
                {
                    top = Height - 120;
                    panelSim.Top = Height - 78;
                }
            }
            else //buttons exposed
            {
                top = Height - 135;
                if (panelSim.Visible == true)
                {
                    top = Height - 185;
                    panelSim.Top = Height - 128;
                }
            }

            if (tool.isSectionsNotZones)
            {
                //if (!isJobStarted) top = Height - 40;

                btnSection1Man.Top = btnSection2Man.Top = btnSection3Man.Top =
                btnSection4Man.Top = btnSection5Man.Top = btnSection6Man.Top =
                btnSection7Man.Top = btnSection8Man.Top = btnSection9Man.Top =
                btnSection10Man.Top = btnSection11Man.Top = btnSection12Man.Top =
                btnSection13Man.Top = btnSection14Man.Top = btnSection15Man.Top =
                btnSection16Man.Top = top;

                int oglButtonWidth = oglMain.Width * 3 / 4;

                int buttonWidth = oglButtonWidth / tool.numOfSections;
                if (buttonWidth > buttonMaxWidth) buttonWidth = buttonMaxWidth;

                btnSection1Man.Size = btnSection2Man.Size = btnSection3Man.Size =
                btnSection4Man.Size = btnSection5Man.Size = btnSection6Man.Size =
                btnSection7Man.Size = btnSection8Man.Size = btnSection9Man.Size =
                btnSection10Man.Size = btnSection11Man.Size = btnSection12Man.Size =
                btnSection13Man.Size = btnSection14Man.Size = btnSection15Man.Size =
                btnSection16Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);

                btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                btnSection4Man.Left = btnSection3Man.Left + btnSection1Man.Size.Width;
                btnSection5Man.Left = btnSection4Man.Left + btnSection1Man.Size.Width;
                btnSection6Man.Left = btnSection5Man.Left + btnSection1Man.Size.Width;
                btnSection7Man.Left = btnSection6Man.Left + btnSection1Man.Size.Width;
                btnSection8Man.Left = btnSection7Man.Left + btnSection1Man.Size.Width;
                btnSection9Man.Left = btnSection8Man.Left + btnSection1Man.Size.Width;
                btnSection10Man.Left = btnSection9Man.Left + btnSection1Man.Size.Width;
                btnSection11Man.Left = btnSection10Man.Left + btnSection1Man.Size.Width;
                btnSection12Man.Left = btnSection11Man.Left + btnSection1Man.Size.Width;
                btnSection13Man.Left = btnSection12Man.Left + btnSection1Man.Size.Width;
                btnSection14Man.Left = btnSection13Man.Left + btnSection1Man.Size.Width;
                btnSection15Man.Left = btnSection14Man.Left + btnSection1Man.Size.Width;
                btnSection16Man.Left = btnSection15Man.Left + btnSection1Man.Size.Width;

                btnSection1Man.Visible = tool.numOfSections > 0;
                btnSection2Man.Visible = tool.numOfSections > 1;
                btnSection3Man.Visible = tool.numOfSections > 2;
                btnSection4Man.Visible = tool.numOfSections > 3;
                btnSection5Man.Visible = tool.numOfSections > 4;
                btnSection6Man.Visible = tool.numOfSections > 5;
                btnSection7Man.Visible = tool.numOfSections > 6;
                btnSection8Man.Visible = tool.numOfSections > 7;
                btnSection9Man.Visible = tool.numOfSections > 8;
                btnSection10Man.Visible = tool.numOfSections > 9;
                btnSection11Man.Visible = tool.numOfSections > 10;
                btnSection12Man.Visible = tool.numOfSections > 11;
                btnSection13Man.Visible = tool.numOfSections > 12;
                btnSection14Man.Visible = tool.numOfSections > 13;
                btnSection15Man.Visible = tool.numOfSections > 14;
                btnSection16Man.Visible = tool.numOfSections > 15;
            }
        }

        //Zone buttons ************************************
        public void AllZonesAndButtonsToState(btnStates state)
        {
            if (tool.zoneRanges[0] == 0) return;
            if (tool.zoneRanges[1] != 0) IndividualZoneAndButtonToState(state, 0, tool.zoneRanges[1], btnZone1);
            if (tool.zoneRanges[2] != 0) IndividualZoneAndButtonToState(state, tool.zoneRanges[1], tool.zoneRanges[2], btnZone2);
            if (tool.zoneRanges[3] != 0) IndividualZoneAndButtonToState(state, tool.zoneRanges[2], tool.zoneRanges[3], btnZone3);
            if (tool.zoneRanges[4] != 0) IndividualZoneAndButtonToState(state, tool.zoneRanges[3], tool.zoneRanges[4], btnZone4);
            if (tool.zoneRanges[5] != 0) IndividualZoneAndButtonToState(state, tool.zoneRanges[4], tool.zoneRanges[5], btnZone5);
            if (tool.zoneRanges[6] != 0) IndividualZoneAndButtonToState(state, tool.zoneRanges[5], tool.zoneRanges[6], btnZone6);
            if (tool.zoneRanges[7] != 0) IndividualZoneAndButtonToState(state, tool.zoneRanges[6], tool.zoneRanges[7], btnZone7);
            if (tool.zoneRanges[8] != 0) IndividualZoneAndButtonToState(state, tool.zoneRanges[7], tool.zoneRanges[8], btnZone8);
        }

        private void IndividualZoneAndButtonToState(btnStates state, int sectionStartNumber, int sectionEndNumber, Button btn)
        {
            for (int i = sectionStartNumber; i < sectionEndNumber; i++)
            {
                section[i].sectionBtnState = state;
            }

            //update zone buttons
            switch (state)
            {
                case btnStates.Auto:
                    if (isDay)
                    {
                        btn.BackColor = Color.Lime;
                        btn.ForeColor = Color.Black;
                    }
                    else
                    {
                        btn.BackColor = Color.ForestGreen;
                        btn.ForeColor = Color.White;
                    }
                    break;


                case btnStates.On:
                    if (isDay)
                    {
                        btn.BackColor = Color.Yellow;
                        btn.ForeColor = Color.Black;
                    }
                    else
                    {
                        btn.BackColor = Color.DarkGoldenrod;
                        btn.ForeColor = Color.White;
                    }
                    break;

                case btnStates.Off:
                    if (isDay)
                    {
                        btn.ForeColor = Color.Black;
                        btn.BackColor = Color.Red;
                    }
                    else
                    {
                        btn.BackColor = Color.Crimson;
                        btn.ForeColor = Color.White;
                    }
                    break;
            }
        }

        public void LineUpAllZoneButtons()
        {
            if (!isJobStarted)
            {
                btnSection1Man.Visible = false;
                btnSection2Man.Visible = false;
                btnSection3Man.Visible = false;
                btnSection4Man.Visible = false;
                btnSection5Man.Visible = false;
                btnSection6Man.Visible = false;
                btnSection7Man.Visible = false;
                btnSection8Man.Visible = false;
                btnSection9Man.Visible = false;
                btnSection10Man.Visible = false;
                btnSection11Man.Visible = false;
                btnSection12Man.Visible = false;
                btnSection13Man.Visible = false;
                btnSection14Man.Visible = false;
                btnSection15Man.Visible = false;
                btnSection16Man.Visible = false;

                btnZone1.Visible = false;
                btnZone2.Visible = false;
                btnZone3.Visible = false;
                btnZone4.Visible = false;
                btnZone5.Visible = false;
                btnZone6.Visible = false;
                btnZone7.Visible = false;
                btnZone8.Visible = false;
                return;
            }

            int oglCenter = isPanelBottomHidden ? oglCenter = oglMain.Width / 2 + 30 : statusStripLeft.Width + oglMain.Width / 2;

            int top = 130;

            int buttonMaxWidth = 400, buttonHeight = 30;


            if ((Height - oglMain.Height) < 80) //max size - buttons hid
            {
                top = Height - 70;
                if (panelSim.Visible == true)
                {
                    top = Height - 100;
                    panelSim.Top = Height - 60;
                }
            }
            else //buttons exposed
            {
                top = Height - 130;
                if (panelSim.Visible == true)
                {
                    top = Height - 160;
                    panelSim.Top = Height - 120;
                }
            }

            //if (tool.zones == 0) return;
            btnZone1.Visible = tool.zones > 0;
            btnZone2.Visible = tool.zones > 1;
            btnZone3.Visible = tool.zones > 2;
            btnZone4.Visible = tool.zones > 3;
            btnZone5.Visible = tool.zones > 4;
            btnZone6.Visible = tool.zones > 5;
            btnZone7.Visible = tool.zones > 6;
            btnZone8.Visible = tool.zones > 7;

            if (isJobStarted)
            {
                btnZone1.BackColor = Color.Red;
                btnZone2.BackColor = Color.Red;
                btnZone3.BackColor = Color.Red;
                btnZone4.BackColor = Color.Red;
                btnZone5.BackColor = Color.Red;
                btnZone6.BackColor = Color.Red;
                btnZone7.BackColor = Color.Red;
                btnZone8.BackColor = Color.Red;
            }
            else
            {
                btnZone1.BackColor = Color.Silver;
                btnZone2.BackColor = Color.Silver;
                btnZone3.BackColor = Color.Silver;
                btnZone4.BackColor = Color.Silver;
                btnZone5.BackColor = Color.Silver;
                btnZone6.BackColor = Color.Silver;
                btnZone7.BackColor = Color.Silver;
                btnZone8.BackColor = Color.Silver;
            }


            btnZone1.Top = btnZone2.Top = btnZone3.Top =
            btnZone4.Top = btnZone5.Top = btnZone6.Top =
            btnZone7.Top = btnZone8.Top = top;

            int oglButtonWidth = oglMain.Width * 3 / 4;
            int buttonWidth = oglButtonWidth / tool.zones;
            if (buttonWidth > buttonMaxWidth) buttonWidth = buttonMaxWidth;

            btnZone1.Size = btnZone2.Size = btnZone3.Size = btnZone4.Size 
                = btnZone5.Size = btnZone6.Size = btnZone7.Size = btnZone8.Size
                = new System.Drawing.Size(buttonWidth, buttonHeight);

            btnZone1.Left = (oglCenter) - (tool.zones * btnZone1.Size.Width) / 2;
            btnZone2.Left = btnZone1.Left + btnZone1.Size.Width;
            btnZone3.Left = btnZone2.Left + btnZone1.Size.Width;
            btnZone4.Left = btnZone3.Left + btnZone1.Size.Width;
            btnZone5.Left = btnZone4.Left + btnZone1.Size.Width;
            btnZone6.Left = btnZone5.Left + btnZone1.Size.Width;
            btnZone7.Left = btnZone6.Left + btnZone1.Size.Width;
            btnZone8.Left = btnZone7.Left + btnZone1.Size.Width;
        }

        //function to set section positions
        public void SectionSetPosition()
        {
            if (tool.isSectionsNotZones)
            {
                section[0].positionLeft = (double)Settings.Default.setSection_position1 + Settings.Default.setVehicle_toolOffset;
                section[0].positionRight = (double)Settings.Default.setSection_position2 + Settings.Default.setVehicle_toolOffset;

                section[1].positionLeft = (double)Settings.Default.setSection_position2 + Settings.Default.setVehicle_toolOffset;
                section[1].positionRight = (double)Settings.Default.setSection_position3 + Settings.Default.setVehicle_toolOffset;

                section[2].positionLeft = (double)Settings.Default.setSection_position3 + Settings.Default.setVehicle_toolOffset;
                section[2].positionRight = (double)Settings.Default.setSection_position4 + Settings.Default.setVehicle_toolOffset;

                section[3].positionLeft = (double)Settings.Default.setSection_position4 + Settings.Default.setVehicle_toolOffset;
                section[3].positionRight = (double)Settings.Default.setSection_position5 + Settings.Default.setVehicle_toolOffset;

                section[4].positionLeft = (double)Settings.Default.setSection_position5 + Settings.Default.setVehicle_toolOffset;
                section[4].positionRight = (double)Settings.Default.setSection_position6 + Settings.Default.setVehicle_toolOffset;

                section[5].positionLeft = (double)Settings.Default.setSection_position6 + Settings.Default.setVehicle_toolOffset;
                section[5].positionRight = (double)Settings.Default.setSection_position7 + Settings.Default.setVehicle_toolOffset;

                section[6].positionLeft = (double)Settings.Default.setSection_position7 + Settings.Default.setVehicle_toolOffset;
                section[6].positionRight = (double)Settings.Default.setSection_position8 + Settings.Default.setVehicle_toolOffset;

                section[7].positionLeft = (double)Settings.Default.setSection_position8 + Settings.Default.setVehicle_toolOffset;
                section[7].positionRight = (double)Settings.Default.setSection_position9 + Settings.Default.setVehicle_toolOffset;

                section[8].positionLeft = (double)Settings.Default.setSection_position9 + Settings.Default.setVehicle_toolOffset;
                section[8].positionRight = (double)Settings.Default.setSection_position10 + Settings.Default.setVehicle_toolOffset;

                section[9].positionLeft = (double)Settings.Default.setSection_position10 + Settings.Default.setVehicle_toolOffset;
                section[9].positionRight = (double)Settings.Default.setSection_position11 + Settings.Default.setVehicle_toolOffset;

                section[10].positionLeft = (double)Settings.Default.setSection_position11 + Settings.Default.setVehicle_toolOffset;
                section[10].positionRight = (double)Settings.Default.setSection_position12 + Settings.Default.setVehicle_toolOffset;

                section[11].positionLeft = (double)Settings.Default.setSection_position12 + Settings.Default.setVehicle_toolOffset;
                section[11].positionRight = (double)Settings.Default.setSection_position13 + Settings.Default.setVehicle_toolOffset;

                section[12].positionLeft = (double)Settings.Default.setSection_position13 + Settings.Default.setVehicle_toolOffset;
                section[12].positionRight = (double)Settings.Default.setSection_position14 + Settings.Default.setVehicle_toolOffset;

                section[13].positionLeft = (double)Settings.Default.setSection_position14 + Settings.Default.setVehicle_toolOffset;
                section[13].positionRight = (double)Settings.Default.setSection_position15 + Settings.Default.setVehicle_toolOffset;

                section[14].positionLeft = (double)Settings.Default.setSection_position15 + Settings.Default.setVehicle_toolOffset;
                section[14].positionRight = (double)Settings.Default.setSection_position16 + Settings.Default.setVehicle_toolOffset;

                section[15].positionLeft = (double)Settings.Default.setSection_position16 + Settings.Default.setVehicle_toolOffset;
                section[15].positionRight = (double)Settings.Default.setSection_position17 + Settings.Default.setVehicle_toolOffset;
            }
        }

        //function to calculate the width of each section and update
        public void SectionCalcWidths()
        {
            if (tool.isSectionsNotZones)
            {
                for (int j = 0; j < MAXSECTIONS; j++)
                {
                    section[j].sectionWidth = (section[j].positionRight - section[j].positionLeft);
                    section[j].rpSectionPosition = 250 + (int)(Math.Round(section[j].positionLeft * 10, 0, MidpointRounding.AwayFromZero));
                    section[j].rpSectionWidth = (int)(Math.Round(section[j].sectionWidth * 10, 0, MidpointRounding.AwayFromZero));
                }

                //calculate tool width based on extreme right and left values
                tool.width = (section[tool.numOfSections - 1].positionRight) - (section[0].positionLeft);

                //left and right tool position
                tool.farLeftPosition = section[0].positionLeft;
                tool.farRightPosition = section[tool.numOfSections - 1].positionRight;

                //find the right side pixel position
                tool.rpXPosition = 250 + (int)(Math.Round(tool.farLeftPosition * 10, 0, MidpointRounding.AwayFromZero));
                tool.rpWidth = (int)(Math.Round(tool.width * 10, 0, MidpointRounding.AwayFromZero));
            }
        }

        public void SectionCalcMulti()
        {
            double leftside = tool.width / -2.0;
            double defaultSectionWidth = Properties.Settings.Default.setTool_sectionWidthMulti;
            double offset = Settings.Default.setVehicle_toolOffset;
            section[0].positionLeft = leftside+offset;

            for (int i = 0; i < tool.numOfSections - 1; i++)
            {
                leftside += defaultSectionWidth;

                section[i].positionRight = leftside + offset;
                section[i + 1].positionLeft = leftside + offset;
                section[i].sectionWidth = defaultSectionWidth;
                section[i].rpSectionPosition = 250 + (int)(Math.Round(section[i].positionLeft * 10, 0, MidpointRounding.AwayFromZero));
                section[i].rpSectionWidth = (int)(Math.Round(section[i].sectionWidth * 10, 0, MidpointRounding.AwayFromZero));
            }

            leftside += defaultSectionWidth;
            section[tool.numOfSections - 1].positionRight = leftside + offset;
            section[tool.numOfSections - 1].sectionWidth = defaultSectionWidth;
            section[tool.numOfSections - 1].rpSectionPosition = 250 + (int)(Math.Round(section[tool.numOfSections - 1].positionLeft * 10, 0, MidpointRounding.AwayFromZero));
            section[tool.numOfSections - 1].rpSectionWidth = (int)(Math.Round(section[tool.numOfSections - 1].sectionWidth * 10, 0, MidpointRounding.AwayFromZero));

            //calculate tool width based on extreme right and left values
            tool.width = (section[tool.numOfSections - 1].positionRight) - (section[0].positionLeft);

            //left and right tool position
            tool.farLeftPosition = section[0].positionLeft;
            tool.farRightPosition = section[tool.numOfSections - 1].positionRight;

            //find the right side pixel position
            tool.rpXPosition = 250 + (int)(Math.Round(tool.farLeftPosition * 10, 0, MidpointRounding.AwayFromZero));
            tool.rpWidth = (int)(Math.Round(tool.width * 10, 0, MidpointRounding.AwayFromZero));
        }

        private void BuildMachineByte()
        {
            if (tool.isSectionsNotZones)
            {
                p_254.pgn[p_254.sc1to8] = 0;
                p_254.pgn[p_254.sc9to16] = 0;

                int number = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (section[j].isSectionOn)
                        number |= 1 << j;
                }
                p_254.pgn[p_254.sc1to8] = unchecked((byte)number);
                number = 0;

                for (int j = 8; j < 16; j++)
                {
                    if (section[j].isSectionOn)
                        number |= 1 << (j-8);
                }
                p_254.pgn[p_254.sc9to16] = unchecked((byte)number);

                //machine pgn
                p_239.pgn[p_239.sc1to8] = p_254.pgn[p_254.sc1to8];
                p_239.pgn[p_239.sc9to16] = p_254.pgn[p_254.sc9to16];
                p_229.pgn[p_229.sc1to8] = p_254.pgn[p_254.sc1to8];
                p_229.pgn[p_229.sc9to16] = p_254.pgn[p_254.sc9to16];
                p_229.pgn[p_229.toolLSpeed] = unchecked((byte)(tool.farLeftSpeed * 10));
                p_229.pgn[p_229.toolRSpeed] = unchecked((byte)(tool.farRightSpeed * 10));
            }
            else
            {
                //zero all the bytes - set only if on
                for (int i = 5; i < 13; i++)
                {
                    p_229.pgn[i] = 0;
                }

                int number = 0;
                for (int k = 0; k < 8; k++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (section[j + k * 8].isSectionOn)
                            number |= 1 << j;
                    }
                    p_229.pgn[5 + k] = unchecked((byte)number);
                    number = 0;
                }

                //tool speed to calc ramp
                p_229.pgn[p_229.toolLSpeed] = unchecked((byte)(tool.farLeftSpeed * 10));
                p_229.pgn[p_229.toolRSpeed] = unchecked((byte)(tool.farRightSpeed * 10));

                p_239.pgn[p_239.sc1to8] = p_229.pgn[p_229.sc1to8];
                p_239.pgn[p_239.sc9to16] = p_229.pgn[p_229.sc9to16];

                p_254.pgn[p_254.sc1to8] = p_229.pgn[p_229.sc1to8];
                p_254.pgn[p_254.sc9to16] = p_229.pgn[p_229.sc9to16];

            }

            p_239.pgn[p_239.speed] = unchecked((byte)(avgSpeed * 10));
            p_239.pgn[p_239.tram] = unchecked((byte)tram.controlByte);
        }

        private void DoRemoteSwitches()
        {
            //MTZ8302 Feb 2020 
            if (isJobStarted)
            {
                //MainSW was used
                if (mc.ss[mc.swMain] != mc.ssP[mc.swMain])
                {
                    //Main SW pressed
                    if ((mc.ss[mc.swMain] & 1) == 1)
                    {
                        //set butto off and then press it = ON
                        autoBtnState = btnStates.Off;
                        btnSectionMasterAuto.PerformClick();
                    } // if Main SW ON

                    //if Main SW in Arduino is pressed OFF
                    if ((mc.ss[mc.swMain] & 2) == 2)
                    {
                        //set button on and then press it = OFF
                        autoBtnState = btnStates.Auto;
                        btnSectionMasterAuto.PerformClick();
                    } // if Main SW OFF

                    mc.ssP[mc.swMain] = mc.ss[mc.swMain];
                }  //Main or shpList SW

                if (tool.isSectionsNotZones)
                {
                    #region NoZones
                    if (mc.ss[mc.swOnGr0] != 0)
                    {
                        // ON Signal from Arduino 
                        if ((mc.ss[mc.swOnGr0] & 128) == 128 & tool.numOfSections > 7)
                        {
                            if (section[7].sectionBtnState != btnStates.Auto) section[7].sectionBtnState = btnStates.Auto;
                            btnSection8Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOnGr0] & 64) == 64 & tool.numOfSections > 6)
                        {
                            if (section[6].sectionBtnState != btnStates.Auto) section[6].sectionBtnState = btnStates.Auto;
                            btnSection7Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOnGr0] & 32) == 32 & tool.numOfSections > 5)
                        {
                            if (section[5].sectionBtnState != btnStates.Auto) section[5].sectionBtnState = btnStates.Auto;
                            btnSection6Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOnGr0] & 16) == 16 & tool.numOfSections > 4)
                        {
                            if (section[4].sectionBtnState != btnStates.Auto) section[4].sectionBtnState = btnStates.Auto;
                            btnSection5Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOnGr0] & 8) == 8 & tool.numOfSections > 3)
                        {
                            if (section[3].sectionBtnState != btnStates.Auto) section[3].sectionBtnState = btnStates.Auto;
                            btnSection4Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOnGr0] & 4) == 4 & tool.numOfSections > 2)
                        {
                            if (section[2].sectionBtnState != btnStates.Auto) section[2].sectionBtnState = btnStates.Auto;
                            btnSection3Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOnGr0] & 2) == 2 & tool.numOfSections > 1)
                        {
                            if (section[1].sectionBtnState != btnStates.Auto) section[1].sectionBtnState = btnStates.Auto;
                            btnSection2Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOnGr0] & 1) == 1)
                        {
                            if (section[0].sectionBtnState != btnStates.Auto) section[0].sectionBtnState = btnStates.Auto;
                            btnSection1Man.PerformClick();
                        }
                        mc.ssP[mc.swOnGr0] = mc.ss[mc.swOnGr0];
                    } //if swONLo != 0 
                    else { if (mc.ssP[mc.swOnGr0] != 0) { mc.ssP[mc.swOnGr0] = 0; } }

                    if (mc.ss[mc.swOnGr1] != 0)
                    {
                        // sections ON signal from Arduino  
                        if ((mc.ss[mc.swOnGr1] & 128) == 128 & tool.numOfSections > 15)
                        {
                            if (section[15].sectionBtnState != btnStates.Auto) section[15].sectionBtnState = btnStates.Auto;
                            btnSection16Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOnGr1] & 64) == 64 & tool.numOfSections > 14)
                        {
                            if (section[14].sectionBtnState != btnStates.Auto) section[14].sectionBtnState = btnStates.Auto;
                            btnSection15Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOnGr1] & 32) == 32 & tool.numOfSections > 13)
                        {
                            if (section[13].sectionBtnState != btnStates.Auto) section[13].sectionBtnState = btnStates.Auto;
                            btnSection14Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOnGr1] & 16) == 16 & tool.numOfSections > 12)
                        {
                            if (section[12].sectionBtnState != btnStates.Auto) section[12].sectionBtnState = btnStates.Auto;
                            btnSection13Man.PerformClick();
                        }

                        if ((mc.ss[mc.swOnGr1] & 8) == 8 & tool.numOfSections > 11)
                        {
                            if (section[11].sectionBtnState != btnStates.Auto) section[11].sectionBtnState = btnStates.Auto;
                            btnSection12Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOnGr1] & 4) == 4 & tool.numOfSections > 10)
                        {
                            if (section[10].sectionBtnState != btnStates.Auto) section[10].sectionBtnState = btnStates.Auto;
                            btnSection11Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOnGr1] & 2) == 2 & tool.numOfSections > 9)
                        {
                            if (section[9].sectionBtnState != btnStates.Auto) section[9].sectionBtnState = btnStates.Auto;
                            btnSection10Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOnGr1] & 1) == 1 & tool.numOfSections > 8)
                        {
                            if (section[8].sectionBtnState != btnStates.Auto) section[8].sectionBtnState = btnStates.Auto;
                            btnSection9Man.PerformClick();
                        }
                        mc.ssP[mc.swOnGr1] = mc.ss[mc.swOnGr1];
                    } //if swONHi != 0   
                    else { if (mc.ssP[mc.swOnGr1] != 0) { mc.ssP[mc.swOnGr1] = 0; } }

                    // Switches have changed
                    if (mc.ss[mc.swOffGr0] != mc.ssP[mc.swOffGr0])
                    {
                        //if Main = Auto then change section to Auto if Off signal from Arduino stopped
                        if (autoBtnState == btnStates.Auto)
                        {
                            if (((mc.ssP[mc.swOffGr0] & 128) == 128) & ((mc.ss[mc.swOffGr0] & 128) != 128) & (section[7].sectionBtnState == btnStates.Off))
                            {
                                btnSection8Man.PerformClick();
                            }
                            if (((mc.ssP[mc.swOffGr0] & 64) == 64) & ((mc.ss[mc.swOffGr0] & 64) != 64) & (section[6].sectionBtnState == btnStates.Off))
                            {
                                btnSection7Man.PerformClick();
                            }
                            if (((mc.ssP[mc.swOffGr0] & 32) == 32) & ((mc.ss[mc.swOffGr0] & 32) != 32) & (section[5].sectionBtnState == btnStates.Off))
                            {
                                btnSection6Man.PerformClick();
                            }
                            if (((mc.ssP[mc.swOffGr0] & 16) == 16) & ((mc.ss[mc.swOffGr0] & 16) != 16) & (section[4].sectionBtnState == btnStates.Off))
                            {
                                btnSection5Man.PerformClick();
                            }
                            if (((mc.ssP[mc.swOffGr0] & 8) == 8) & ((mc.ss[mc.swOffGr0] & 8) != 8) & (section[3].sectionBtnState == btnStates.Off))
                            {
                                btnSection4Man.PerformClick();
                            }
                            if (((mc.ssP[mc.swOffGr0] & 4) == 4) & ((mc.ss[mc.swOffGr0] & 4) != 4) & (section[2].sectionBtnState == btnStates.Off))
                            {
                                btnSection3Man.PerformClick();
                            }
                            if (((mc.ssP[mc.swOffGr0] & 2) == 2) & ((mc.ss[mc.swOffGr0] & 2) != 2) & (section[1].sectionBtnState == btnStates.Off))
                            {
                                btnSection2Man.PerformClick();
                            }
                            if (((mc.ssP[mc.swOffGr0] & 1) == 1) & ((mc.ss[mc.swOffGr0] & 1) != 1) & (section[0].sectionBtnState == btnStates.Off))
                            {
                                btnSection1Man.PerformClick();
                            }
                        }
                        mc.ssP[mc.swOffGr0] = mc.ss[mc.swOffGr0];
                    }

                    if (mc.ss[mc.swOffGr1] != mc.ssP[mc.swOffGr1])
                    {
                        //if Main = Auto then change section to Auto if Off signal from Arduino stopped
                        if (autoBtnState == btnStates.Auto)
                        {
                            if (((mc.ssP[mc.swOffGr1] & 128) == 128) & ((mc.ss[mc.swOffGr1] & 128) != 128) & (section[15].sectionBtnState == btnStates.Off))
                            { btnSection16Man.PerformClick(); }

                            if (((mc.ssP[mc.swOffGr1] & 64) == 64) & ((mc.ss[mc.swOffGr1] & 64) != 64) & (section[14].sectionBtnState == btnStates.Off))
                            { btnSection15Man.PerformClick(); }

                            if (((mc.ssP[mc.swOffGr1] & 32) == 32) & ((mc.ss[mc.swOffGr1] & 32) != 32) & (section[13].sectionBtnState == btnStates.Off))
                            { btnSection14Man.PerformClick(); }

                            if (((mc.ssP[mc.swOffGr1] & 16) == 16) & ((mc.ss[mc.swOffGr1] & 16) != 16) & (section[12].sectionBtnState == btnStates.Off))
                            { btnSection13Man.PerformClick(); }


                            if (((mc.ssP[mc.swOffGr1] & 8) == 8) & ((mc.ss[mc.swOffGr1] & 8) != 8) & (section[11].sectionBtnState == btnStates.Off))
                            {
                                btnSection12Man.PerformClick();
                            }
                            if (((mc.ssP[mc.swOffGr1] & 4) == 4) & ((mc.ss[mc.swOffGr1] & 4) != 4) & (section[10].sectionBtnState == btnStates.Off))
                            {
                                btnSection11Man.PerformClick();
                            }
                            if (((mc.ssP[mc.swOffGr1] & 2) == 2) & ((mc.ss[mc.swOffGr1] & 2) != 2) & (section[9].sectionBtnState == btnStates.Off))
                            {
                                btnSection10Man.PerformClick();
                            }
                            if (((mc.ssP[mc.swOffGr1] & 1) == 1) & ((mc.ss[mc.swOffGr1] & 1) != 1) & (section[8].sectionBtnState == btnStates.Off))
                            {
                                btnSection9Man.PerformClick();
                            }
                        }
                        mc.ssP[mc.swOffGr1] = mc.ss[mc.swOffGr1];
                    }

                    // OFF Signal from Arduino
                    if (mc.ss[mc.swOffGr0] != 0)
                    {
                        //if section SW in Arduino is switched to OFF; check always, if switch is locked to off GUI should not change
                        if ((mc.ss[mc.swOffGr0] & 128) == 128 & section[7].sectionBtnState != btnStates.Off)
                        {
                            section[7].sectionBtnState = btnStates.On;
                            btnSection8Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOffGr0] & 64) == 64 & section[6].sectionBtnState != btnStates.Off)
                        {
                            section[6].sectionBtnState = btnStates.On;
                            btnSection7Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOffGr0] & 32) == 32 & section[5].sectionBtnState != btnStates.Off)
                        {
                            section[5].sectionBtnState = btnStates.On;
                            btnSection6Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOffGr0] & 16) == 16 & section[4].sectionBtnState != btnStates.Off)
                        {
                            section[4].sectionBtnState = btnStates.On;
                            btnSection5Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOffGr0] & 8) == 8 & section[3].sectionBtnState != btnStates.Off)
                        {
                            section[3].sectionBtnState = btnStates.On;
                            btnSection4Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOffGr0] & 4) == 4 & section[2].sectionBtnState != btnStates.Off)
                        {
                            section[2].sectionBtnState = btnStates.On;
                            btnSection3Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOffGr0] & 2) == 2 & section[1].sectionBtnState != btnStates.Off)
                        {
                            section[1].sectionBtnState = btnStates.On;
                            btnSection2Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOffGr0] & 1) == 1 & section[0].sectionBtnState != btnStates.Off)
                        {
                            section[0].sectionBtnState = btnStates.On;
                            btnSection1Man.PerformClick();
                        }
                    } // if swOFFLo !=0
                    if (mc.ss[mc.swOffGr1] != 0)
                    {
                        //if section SW in Arduino is switched to OFF; check always, if switch is locked to off GUI should not change
                        if ((mc.ss[mc.swOffGr1] & 128) == 128 & section[15].sectionBtnState != btnStates.Off)
                        {
                            section[15].sectionBtnState = btnStates.On;
                            btnSection16Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOffGr1] & 64) == 64 & section[14].sectionBtnState != btnStates.Off)
                        {
                            section[14].sectionBtnState = btnStates.On;
                            btnSection15Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOffGr1] & 32) == 32 & section[13].sectionBtnState != btnStates.Off)
                        {
                            section[13].sectionBtnState = btnStates.On;
                            btnSection14Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOffGr1] & 16) == 16 & section[12].sectionBtnState != btnStates.Off)
                        {
                            section[12].sectionBtnState = btnStates.On;
                            btnSection13Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOffGr1] & 8) == 8 & section[11].sectionBtnState != btnStates.Off)
                        {
                            section[11].sectionBtnState = btnStates.On;
                            btnSection12Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOffGr1] & 4) == 4 & section[10].sectionBtnState != btnStates.Off)
                        {
                            section[10].sectionBtnState = btnStates.On;
                            btnSection11Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOffGr1] & 2) == 2 & section[9].sectionBtnState != btnStates.Off)
                        {
                            section[9].sectionBtnState = btnStates.On;
                            btnSection10Man.PerformClick();
                        }
                        if ((mc.ss[mc.swOffGr1] & 1) == 1 & section[8].sectionBtnState != btnStates.Off)
                        {
                            section[8].sectionBtnState = btnStates.On;
                            btnSection9Man.PerformClick();
                        }
                    } // if swOFFHi !=0
                    #endregion
                }
                else
                {
                    DoZones();
                }
            }//if serial or udp port open
        }
        private void DoZones()
        {
            int Bit;
            // zones to on
            if (mc.ss[mc.swOnGr0] != 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    Bit = (int)Math.Pow(2, i);
                    if ((tool.zoneRanges[i + 1] > 0) && ((mc.ss[mc.swOnGr0] & Bit) == Bit))
                    {
                        if (section[tool.zoneRanges[i + 1] - 1].sectionBtnState != btnStates.Auto) section[tool.zoneRanges[i + 1] - 1].sectionBtnState = btnStates.Auto;
                        PerformZoneClick(i);
                    }
                }

                mc.ssP[mc.swOnGr0] = mc.ss[mc.swOnGr0];
            }
            else { if (mc.ssP[mc.swOnGr0] != 0) { mc.ssP[mc.swOnGr0] = 0; } }

            // zones to auto
            if (mc.ss[mc.swOffGr0] != mc.ssP[mc.swOffGr0])
            {
                if (autoBtnState == btnStates.Auto)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Bit = (int)Math.Pow(2, i);
                        if ((tool.zoneRanges[i + 1] > 0) && ((mc.ssP[mc.swOffGr0] & Bit) == Bit)
                            && ((mc.ss[mc.swOffGr0] & Bit) != Bit) && (section[tool.zoneRanges[i + 1] - 1].sectionBtnState == btnStates.Off))
                        {
                            PerformZoneClick(i);
                        }
                    }
                }
                mc.ssP[mc.swOffGr0] = mc.ss[mc.swOffGr0];
            }

            // zones to off
            if (mc.ss[mc.swOffGr0] != 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    Bit = (int)Math.Pow(2, i);
                    if ((tool.zoneRanges[i + 1] > 0) && ((mc.ss[mc.swOffGr0] & Bit) == Bit) && (section[tool.zoneRanges[i + 1] - 1].sectionBtnState != btnStates.Off))
                    {
                        section[tool.zoneRanges[i + 1] - 1].sectionBtnState = btnStates.On;
                        PerformZoneClick(i);
                    }
                }
            }
        }

        private void PerformZoneClick(int Btn)
        {
            switch (Btn)
            {
                case 0:
                    btnZone1.PerformClick();
                    break;

                case 1:
                    btnZone2.PerformClick();
                    break;

                case 2:
                    btnZone3.PerformClick();
                    break;

                case 3:
                    btnZone4.PerformClick();
                    break;

                case 4:
                    btnZone5.PerformClick();
                    break;

                case 5:
                    btnZone6.PerformClick();
                    break;

                case 6:
                    btnZone7.PerformClick();
                    break;

                case 7:
                    btnZone8.PerformClick();
                    break;
            }
        }
    }
}
