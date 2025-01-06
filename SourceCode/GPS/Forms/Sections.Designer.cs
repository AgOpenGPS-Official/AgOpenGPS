﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AgOpenGPS.Properties;
using System.Globalization;
using System.IO;
using System.Media;
using System.Linq;

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
                    if (sounds.isSectionsSoundOn) sounds.sndSectionOn.Play();
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
        private void btnZoneX_Click(object sender, EventArgs e)
        {
            int zoneIndex = int.Parse(((Button)sender).Text);
            btnStates state = GetNextState(section[tool.zoneRanges[zoneIndex] - 1].sectionBtnState);
            if (zoneIndex == 1)
            {
                IndividualZoneAndButtonToState(state, 0, tool.zoneRanges[1], (Button)sender);
            }
            else
            {
                IndividualZoneAndButtonToState(state, tool.zoneRanges[zoneIndex - 1], tool.zoneRanges[zoneIndex], (Button)sender);
            }
        }

        //individual buttons for sections
        private void btnSectionXMan_Click(object sender, EventArgs e)
        {
            int sectionX = Convert.ToInt32(((Button)sender).Text);
            btnStates state = GetNextState(section[sectionX - 1].sectionBtnState);
            IndividualSectionAndButonToState(state, sectionX - 1, (Button)sender);
        }


        //Section buttons************************8
        public void AllSectionsAndButtonsToState(btnStates state)
        {
            for (int i = 1; i <= 16; i++)
            {
                IndividualSectionAndButonToState(state, i - 1, this.Controls.Find("btnSection" + i.ToString() + "Man", true).FirstOrDefault() as Button);
            }
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

        public enum SectionAndZoneVisibility { Zones, Sections, Both }
        public void HideSectionsAndZones(SectionAndZoneVisibility szv)
        {
            for (int i = 1; i <= 16; i++)
            {
                if (szv == SectionAndZoneVisibility.Sections || szv == SectionAndZoneVisibility.Both)
                {
                    (this.Controls.Find("btnSection" + i.ToString() + "Man", true).FirstOrDefault() as Button).Visible = false;
                }
                if (i <= 8 && (szv == SectionAndZoneVisibility.Zones || szv == SectionAndZoneVisibility.Both))
                {
                    (this.Controls.Find("btnZone" + i.ToString(), true).FirstOrDefault() as Button).Visible = false;
                }
            }
        }
        public void LineUpIndividualSectionBtns()
        {
            if (!isJobStarted)
            {
                HideSectionsAndZones(SectionAndZoneVisibility.Both);
                return;
            }
            HideSectionsAndZones(SectionAndZoneVisibility.Zones);

            int oglCenter = isPanelBottomHidden ? oglCenter = oglMain.Width / 2 + 30 : statusStripLeft.Width + oglMain.Width / 2;

            //Nozzz
            if (tlpNozzle.Visible) oglCenter += tlpNozzle.Width;

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

                int oglButtonWidth = oglMain.Width * 3 / 4;

                int buttonWidth = Math.Min(oglButtonWidth / tool.numOfSections, buttonMaxWidth);
                //if (buttonWidth > buttonMaxWidth) buttonWidth = buttonMaxWidth;

                Button btnPrev = this.Controls.Find("btnSection1Man", true).FirstOrDefault() as Button;
                Size size = new System.Drawing.Size(buttonWidth, buttonHeight);
                for (int i = 1; i <= 16; i++)
                {
                    Button btn = this.Controls.Find("btnSection" + i.ToString() + "Man", true).FirstOrDefault() as Button;
                    btn.Size = size;
                    btn.Top = top;
                    if (i == 1)
                    {
                        btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                    }
                    else
                    {
                        btnPrev = this.Controls.Find("btnSection" + (i - 1).ToString() + "Man", true).FirstOrDefault() as Button;
                        btn.Left = btnPrev.Left + btnPrev.Size.Width;
                    }
                    btn.Visible = tool.numOfSections > (i - 1);
                }

            }
        }

        //Zone buttons ************************************
        public void AllZonesAndButtonsToState(btnStates state)
        {
            if (tool.zoneRanges[0] == 0) return;
            if (tool.zoneRanges[1] != 0) IndividualZoneAndButtonToState(state, 0, tool.zoneRanges[1], btnZone1);

            for (int i = 2; i <= 8; i++)
            {
                if (tool.zoneRanges[i] != 0)
                {
                    IndividualZoneAndButtonToState(state,
                        tool.zoneRanges[i - 1],
                        tool.zoneRanges[i],
                        this.Controls.Find("btnZone" + i.ToString(), true).FirstOrDefault() as Button
                    );
                }
            }
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
                HideSectionsAndZones(SectionAndZoneVisibility.Both);
                return;
            }

            int oglCenter = isPanelBottomHidden ? oglCenter = oglMain.Width / 2 + 30 : statusStripLeft.Width + oglMain.Width / 2;

            //Nozzz
            if (tlpNozzle.Visible) oglCenter += tlpNozzle.Width;

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
            int oglButtonWidth = oglMain.Width * 3 / 4;
            int buttonWidth = Math.Min(oglButtonWidth / tool.zones,buttonMaxWidth);
            Button btnPrev = null;
            Size size = new System.Drawing.Size(buttonWidth, buttonHeight);

            for (int i = 1; i <= 8; i++)
            {
                Button btn = this.Controls.Find("btnZone" + i.ToString(), true).FirstOrDefault() as Button;
                btn.Visible = tool.zones > (i - 1);
                btn.Top = top;
                btn.Size = size;
                if (isJobStarted)
                {
                    btn.BackColor = Color.Red;
                } else
                {
                    btn.BackColor = Color.Silver;
                }
                if (i == 1)
                {
                    btn.Left = (oglCenter) - (tool.zones * btn.Size.Width) / 2;
                }
                else
                {
                    btn.Left = this.Controls.Find("btnZone" + (i - 1).ToString(), true).FirstOrDefault().Left + btnZone1.Size.Width;
                }
                btnPrev = this.Controls.Find("btnZone" + i.ToString(), true).FirstOrDefault() as Button;

            }
        }

        //function to set section positions
        public void SectionSetPosition()
        {
            if (tool.isSectionsNotZones)
            {
                // optimisation fail - for now
                //for (int i = 0; i < 15; i++)
                //{
                //    PropertyInfo prop = typeof(Settings).GetProperty($"setSection_position{i + 1}");
                //    //section[i].positionLeft = prop.GetValue(Settings.Default) + Settings.Default.setVehicle_toolOffset;
                //    section[i].positionLeft = Convert.ToDouble(typeof(Settings).
                //        GetProperty($"setSection_position{i + 1}").GetValue(Settings.Default)) + 
                //            Settings.Default.setVehicle_toolOffset;
                //    section[i].positionRight = Convert.ToDouble(typeof(Settings).
                //        GetProperty($"setSection_position{i + 2}").GetValue(Settings.Default)) + 
                //            Settings.Default.setVehicle_toolOffset;
                //}
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
            section[0].positionLeft = leftside + offset;

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
                        number |= 1 << (j - 8);
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
                        for (int i = 7; i >= 0; i--)
                        {
                            if (tool.numOfSections > i && (mc.ss[mc.swOnGr0] & (1 << i)) == (1 << i)) // changed order, and introduced short-circuit evaluation
                            {
                                if (section[i].sectionBtnState != btnStates.Auto) section[i].sectionBtnState = btnStates.Auto;
                                (this.Controls.Find("btnSection" + (i + 1).ToString() + "Man", true).FirstOrDefault() as Button).PerformClick();
                            }
                        }
                        mc.ssP[mc.swOnGr0] = mc.ss[mc.swOnGr0];
                    } //if swONLo != 0 
                    else { if (mc.ssP[mc.swOnGr0] != 0) { mc.ssP[mc.swOnGr0] = 0; } }


                    if (mc.ss[mc.swOnGr1] != 0)
                    {
                        // sections ON signal from Arduino  
                        for (int i = 7; i >= 0; i--)
                        {
                            if (tool.numOfSections > (i + 8) && (mc.ss[mc.swOnGr1] & (1 << i)) == (1 << i))
                            {
                                if (section[i + 8].sectionBtnState != btnStates.Auto) section[i + 8].sectionBtnState = btnStates.Auto;
                                (this.Controls.Find("btnSection" + (i + 9).ToString() + "Man", true).FirstOrDefault() as Button).PerformClick();
                            }
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
                            for (int i = 7; i >= 0; i--)
                            {
                                if ((section[i].sectionBtnState == btnStates.Off) && (mc.ssP[mc.swOffGr0] & (1 << i)) == (1 << i) & ((mc.ss[mc.swOffGr0] & (1 << i)) != (1 << i)))
                                {
                                    (this.Controls.Find("btnSection" + (i + 1).ToString() + "Man", true).FirstOrDefault() as Button).PerformClick();
                                }
                            }
                        }
                        mc.ssP[mc.swOffGr0] = mc.ss[mc.swOffGr0];
                    }

                    if (mc.ss[mc.swOffGr1] != mc.ssP[mc.swOffGr1])
                    {
                        //if Main = Auto then change section to Auto if Off signal from Arduino stopped
                        if (autoBtnState == btnStates.Auto)
                        {
                            for (int i = 7; i >= 0; i--)
                            {
                                if ((section[i + 8].sectionBtnState == btnStates.Off) && ((mc.ssP[mc.swOffGr1] & (1 << i)) == (1 << i)) & ((mc.ss[mc.swOffGr1] & (1 << i)) == (1 << i)))
                                {
                                    (this.Controls.Find("btnSection" + (i + 9).ToString() + "Man", true).FirstOrDefault() as Button).PerformClick();
                                }
                            }
                        }
                        mc.ssP[mc.swOffGr1] = mc.ss[mc.swOffGr1];
                    }

                    // OFF Signal from Arduino
                    if (mc.ss[mc.swOffGr0] != 0)
                    {
                        //if section SW in Arduino is switched to OFF; check always, if switch is locked to off GUI should not change
                        for (int i = 7; i >= 0; i--)
                        {
                            if ((section[i].sectionBtnState != btnStates.Off) && (mc.ss[mc.swOffGr0] & (1 << i)) == (1 << i))
                            {
                                section[i].sectionBtnState = btnStates.On;
                                (this.Controls.Find("btnSection" + (i + 1).ToString() + "Man", true).FirstOrDefault() as Button).PerformClick();
                            }
                        }
                    } // if swOFFLo !=0

                    if (mc.ss[mc.swOffGr1] != 0)
                    {
                        //if section SW in Arduino is switched to OFF; check always, if switch is locked to off GUI should not change
                        for (int i = 7; i >= 0; i--)
                        {
                            if ((section[i + 8].sectionBtnState != btnStates.Off) && (mc.ss[mc.swOffGr1] & (1 << i)) == (1 << i))
                            {
                                section[i + 8].sectionBtnState = btnStates.On;
                                (this.Controls.Find("btnSection" + (i + 9).ToString() + "Man", true).FirstOrDefault() as Button).PerformClick();
                            }
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
            (this.Controls.Find("btnZone" + (Btn + 1).ToString(), true).FirstOrDefault() as Button).PerformClick();
        }
    }
}
