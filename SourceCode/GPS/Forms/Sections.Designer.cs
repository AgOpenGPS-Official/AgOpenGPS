using System;
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

        private void markAsWorkedTrack()
        {
            //is it an AB Line or a Curve?
            if (trk.gArr[this.trk.idx].mode == TrackMode.AB)
            {
                //AB Line
                if (!this.trk.gArr[this.trk.idx].workedTracks.Contains(this.ABLine.howManyPathsAway))
                    this.trk.gArr[this.trk.idx].workedTracks.Add(this.ABLine.howManyPathsAway);

            }
            else if (trk.gArr[this.trk.idx].mode == TrackMode.Curve)
            {
                //Curve
                if (!this.trk.gArr[this.trk.idx].workedTracks.Contains(this.curve.howManyPathsAway))
                    this.trk.gArr[this.trk.idx].workedTracks.Add(this.curve.howManyPathsAway);

            }
        }


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

                    //add current Lane when it doesnt exist in the worked lane list
                    markAsWorkedTrack();

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

                    //add current Lane when it doesnt exist in the worked lane list
                    markAsWorkedTrack();

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
            int sectionX = int.Parse(((Button)sender).Text);
            btnStates state = GetNextState(section[sectionX - 1].sectionBtnState);
            IndividualSectionAndButonToState(state, sectionX - 1, (Button)sender);
        }


        //Section buttons************************8
        public void AllSectionsAndButtonsToState(btnStates state)
        {
            for (int i = 1; i <= 16; i++)
            {
                IndividualSectionAndButonToState(state, i - 1, this.Controls.Find("btnSection" + i.ToString() + "Man", true).First() as Button);
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

        public void HideSections()
        {
            for (int i = 1; i <= 16; i++)
                (this.Controls.Find("btnSection" + i.ToString() + "Man", true).First() as Button).Visible = false;
        }
        public void HideZones()
        {
            for (int i = 1; i <= 8; i++)
                (this.Controls.Find("btnZone" + i.ToString(), true).First() as Button).Visible = false;
        }

        public void LineUpIndividualSectionBtns()
        {
            if (!isJobStarted)
            {
                HideSections();
                HideZones();
                return;
            }
            HideZones();

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

                int oglButtonWidth = oglMain.Width * 3 / 4;

                int buttonWidth = Math.Min(oglButtonWidth / tool.numOfSections, buttonMaxWidth);

                Size size = new System.Drawing.Size(buttonWidth, buttonHeight);
                for (int i = 1; i <= 16; i++)
                {
                    Button btn = this.Controls.Find("btnSection" + i.ToString() + "Man", true).First() as Button;
                    btn.Size = size;
                    btn.Top = top;
                    if (i == 1)
                    {
                        btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                    }
                    else
                    {
                        Button btnPrev = this.Controls.Find("btnSection" + (i - 1).ToString() + "Man", true).First() as Button;
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
                        this.Controls.Find("btnZone" + i.ToString(), true).First() as Button
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
                HideSections();
                HideZones();
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
            int oglButtonWidth = oglMain.Width * 3 / 4;
            int buttonWidth = Math.Min(oglButtonWidth / tool.zones,buttonMaxWidth);
            Size size = new System.Drawing.Size(buttonWidth, buttonHeight);

            for (int i = 1; i <= 8; i++)
            {
                Button btn = this.Controls.Find("btnZone" + i.ToString(), true).First() as Button;
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
                    btn.Left = this.Controls.Find("btnZone" + (i - 1).ToString(), true).First().Left + btnZone1.Size.Width;
                }
            }
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
            //MTZ8302 Feb 2020 and hagre 2024
            if (isJobStarted)
            {
                //check if third bit in the pgn234 received Main-Byte is set to indicate the use of buttons (0) or switches (1) in the SC hardware
                if ((mc.ss[mc.swMain] & (1 << 2)) == 0) // Button hardware by MTZ8302 Feb 2020 (3dr bit - check)
                {
                    HandleButtonHardware();
                }
                else  // Switch hardware by hagre 05 2024
                {
                    HandleSwitchHardware();
                }
            }
        }

        private void HandleButtonHardware()
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

            if (tool.isSectionsNotZones)  // NO Zones
            {
                if (mc.ss[mc.swOnGr0] != 0)
                {
                    // ON Signal from Arduino 
                    for (int i = 0; i < 8; i++)
                    {
                        if (((mc.ss[mc.swOnGr0] & (1 << i)) == (1 << i)) && (tool.numOfSections > i))
                        {
                            if (section[i].sectionBtnState != btnStates.Auto)
                            {
                                section[i].sectionBtnState = btnStates.Auto;
                            }
                            PerformSectionClick(i);
                        }
                    }
                    mc.ssP[mc.swOnGr0] = mc.ss[mc.swOnGr0];

                } //if swONLo != 0 
                else
                {
                    if (mc.ssP[mc.swOnGr0] != 0)
                    {
                        mc.ssP[mc.swOnGr0] = 0;
                    }
                }


                if (mc.ss[mc.swOnGr1] != 0)
                {
                    // sections ON signal from Arduino  
                    for (int i = 0; i < 8; i++)
                    {
                        if (((mc.ss[mc.swOnGr1] & (1 << i)) == (1 << i)) && (tool.numOfSections > i + 8))
                        {
                            if (section[i + 8].sectionBtnState != btnStates.Auto)
                            {
                                section[i + 8].sectionBtnState = btnStates.Auto;
                            }
                            PerformSectionClick(i + 8);
                        }
                    }
                    mc.ssP[mc.swOnGr1] = mc.ss[mc.swOnGr1];

                } //if swONHi != 0   
                else
                {
                    if (mc.ssP[mc.swOnGr1] != 0)
                    {
                        mc.ssP[mc.swOnGr1] = 0;
                    }
                }

                // Switches have changed
                if (mc.ss[mc.swOffGr0] != mc.ssP[mc.swOffGr0])
                {
                    //if Main = Auto then change section to Auto if Off signal from Arduino stopped
                    if (autoBtnState == btnStates.Auto)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if (((mc.ssP[mc.swOffGr0] & (1 << i)) == (1 << i)) && ((mc.ss[mc.swOffGr0] & (1 << i)) != (1 << i)) && (section[i].sectionBtnState == btnStates.Off))
                            {
                                PerformSectionClick(i);
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
                        for (int i = 0; i < 8; i++)
                        {
                            if (((mc.ssP[mc.swOffGr1] & (1 << i)) == (1 << i)) && ((mc.ss[mc.swOffGr1] & (1 << i)) != (1 << i)) && (section[i + 8].sectionBtnState == btnStates.Off))
                            {
                                PerformSectionClick(i + 8);
                            }
                        }
                    }
                    mc.ssP[mc.swOffGr1] = mc.ss[mc.swOffGr1];
                }

                // OFF Signal from Arduino
                if (mc.ss[mc.swOffGr0] != 0)
                {
                    //if section SW in Arduino is switched to OFF; check always, if switch is locked to off GUI should not change
                    for (int i = 0; i < 8; i++)
                    {
                        if (((mc.ss[mc.swOffGr0] & (1 << i)) == (1 << i)) && (section[i].sectionBtnState != btnStates.Off))
                        {
                            section[i].sectionBtnState = btnStates.On;
                            PerformSectionClick(i);
                        }
                    }

                } // if swOFFLo !=0

                if (mc.ss[mc.swOffGr1] != 0)
                {
                    //if section SW in Arduino is switched to OFF; check always, if switch is locked to off GUI should not change
                    for (int i = 0; i < 8; i++)
                    {
                        if (((mc.ss[mc.swOffGr1] & (1 << i)) == (1 << i)) && (section[i + 8].sectionBtnState != btnStates.Off))
                        {
                            section[i + 8].sectionBtnState = btnStates.On;
                            PerformSectionClick(i + 8);
                        }
                    }
                } // if swOFFHi !=0
            }
            else// zones to on
            {
                if (mc.ss[mc.swOnGr0] != 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if ((tool.zoneRanges[i + 1] > 0) && ((mc.ss[mc.swOnGr0] & (1 << i)) == (1 << i)))
                        {
                            if (section[tool.zoneRanges[i + 1] - 1].sectionBtnState != btnStates.Auto)
                            {
                                section[tool.zoneRanges[i + 1] - 1].sectionBtnState = btnStates.Auto;
                                PerformZoneClick(i);
                            }
                        }
                    }
                    mc.ssP[mc.swOnGr0] = mc.ss[mc.swOnGr0];
                }
                else
                {
                    if (mc.ssP[mc.swOnGr0] != 0)
                    {
                        mc.ssP[mc.swOnGr0] = 0;
                    }
                }

                // zones to auto
                if (mc.ss[mc.swOffGr0] != mc.ssP[mc.swOffGr0])
                {
                    if (autoBtnState == btnStates.Auto)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if ((tool.zoneRanges[i + 1] > 0) && ((mc.ssP[mc.swOffGr0] & (1 << i)) == (1 << i)) && ((mc.ss[mc.swOffGr0] & (1 << i)) != (1 << i)) && (section[tool.zoneRanges[i + 1] - 1].sectionBtnState == btnStates.Off))
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
                        if ((tool.zoneRanges[i + 1] > 0) && ((mc.ss[mc.swOffGr0] & (1 << i)) == (1 << i)) && (section[tool.zoneRanges[i + 1] - 1].sectionBtnState != btnStates.Off))
                        {
                            section[tool.zoneRanges[i + 1] - 1].sectionBtnState = btnStates.On;
                            PerformZoneClick(i);
                        }
                    }
                }
            }
        }

        private void HandleSwitchHardware()
        {
            //MainSW Byte is AUTO
            if ((autoBtnState != btnStates.Auto) && ((mc.ss[mc.swMain] & 1) == 1))
            {
                //set button off and then press it = ON
                autoBtnState = btnStates.Off;
                btnSectionMasterAuto.PerformClick();
            }
            //MainSW Byte is OFF
            else if ((autoBtnState != btnStates.Off) && ((mc.ss[mc.swMain] & 2) == 2))
            {
                //set button on and then press it = OFF
                autoBtnState = btnStates.Auto;
                btnSectionMasterAuto.PerformClick();
            }

            if (tool.isSectionsNotZones) // NO Zones
            {
                if (mc.ss[mc.swAutoGr0] != 0)
                {
                    // AUTO Signal from Arduino Gr0
                    for (int i = 0; i < 8; i++)
                    {
                        if ((section[i].sectionBtnState != btnStates.Auto) && ((mc.ss[mc.swAutoGr0] & (1 << i)) == (1 << i)) && (tool.numOfSections > i) && (mc.ss[mc.swNumSections] > i))
                        {
                            section[i].sectionBtnState = btnStates.Off;
                            PerformSectionClick(i);
                        }
                    }
                }
                if (mc.ss[mc.swAutoGr1] != 0)
                {
                    // AUTO Signal from Arduino Gr1
                    for (int i = 0; i < 8; i++)
                    {
                        if ((section[i + 8].sectionBtnState != btnStates.Auto) && ((mc.ss[mc.swAutoGr1] & (1 << i)) == (1 << i)) && (tool.numOfSections > i + 8) && (mc.ss[mc.swNumSections] > i + 8))
                        {
                            PerformSectionClick(i + 8);
                        }
                    }
                }

                if (mc.ss[mc.swOnGr0] != 0)
                {
                    // ON Signal from Arduino Gr0
                    for (int i = 0; i < 8; i++)
                    {
                        if (((section[i].sectionBtnState != btnStates.On) && (mc.ss[mc.swOnGr0] & (1 << i)) == (1 << i)) && (tool.numOfSections > i) && (mc.ss[mc.swNumSections] > i))
                        {
                            section[i].sectionBtnState = btnStates.Auto;
                            PerformSectionClick(i);
                        }
                    }
                }
                if (mc.ss[mc.swOnGr1] != 0)
                {
                    // ON Signal from Arduino Gr1
                    for (int i = 0; i < 8; i++)
                    {
                        if ((section[i + 8].sectionBtnState != btnStates.On) && ((mc.ss[mc.swOnGr1] & (1 << i)) == (1 << i)) && (tool.numOfSections > i + 8) && (mc.ss[mc.swNumSections] > i + 8))
                        {
                            section[i + 8].sectionBtnState = btnStates.Auto;
                            PerformSectionClick(i + 8);
                        }
                    }
                }


                if (mc.ss[mc.swOffGr0] != 0)
                {
                    // OFF Signal from Arduino Gr0
                    for (int i = 0; i < 8; i++)
                    {
                        if ((section[i].sectionBtnState != btnStates.Off) && ((mc.ss[mc.swOffGr0] & (1 << i)) == (1 << i)) && (tool.numOfSections > i))  // !check mc.ss[tool.numOfSections] => to be on the save side by switching eveything off 
                        {
                            section[i].sectionBtnState = btnStates.On;
                            PerformSectionClick(i);
                        }
                    }
                }
                if (mc.ss[mc.swOffGr1] != 0)
                {
                    // OFF Signal from Arduino Gr1
                    for (int i = 0; i < 8; i++)
                    {
                        if ((section[i + 8].sectionBtnState != btnStates.Off) && ((mc.ss[mc.swOffGr1] & (1 << i)) == (1 << i)) && (tool.numOfSections > i + 8)) // !check mc.ss[tool.numOfSections] => to be on the save side by switching eveything off
                        {
                            PerformSectionClick(i + 8);
                        }
                    }
                }
            }
            else
            {
                // zones to auto
                if (mc.ss[mc.swAutoGr0] != 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if ((tool.zoneRanges[i + 1] > 0) && ((mc.ss[mc.swAutoGr0] & (1 << i)) == (1 << i)) && ((mc.ss[mc.swOnGr0] & (1 << i)) != (1 << i)) && ((mc.ss[mc.swOffGr0] & (1 << i)) != (1 << i)) && (section[tool.zoneRanges[i + 1] - 1].sectionBtnState != btnStates.Auto))
                        {
                            section[tool.zoneRanges[i + 1] - 1].sectionBtnState = btnStates.Off;
                            PerformZoneClick(i);
                        }
                    }

                }

                // zones to on
                if (mc.ss[mc.swOnGr0] != 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if ((tool.zoneRanges[i + 1] > 0) && ((mc.ss[mc.swOnGr0] & (1 << i)) == (1 << i)) && ((mc.ss[mc.swOffGr0] & (1 << i)) != (1 << i)) && (section[tool.zoneRanges[i + 1] - 1].sectionBtnState != btnStates.On))
                        {
                            section[tool.zoneRanges[i + 1] - 1].sectionBtnState = btnStates.Auto;
                            PerformZoneClick(i);

                        }
                    }
                }

                // zones to off
                if (mc.ss[mc.swOffGr0] != 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if ((tool.zoneRanges[i + 1] > 0) && ((mc.ss[mc.swOffGr0] & (1 << i)) == (1 << i)) && (section[tool.zoneRanges[i + 1] - 1].sectionBtnState != btnStates.Off))
                        {
                            section[tool.zoneRanges[i + 1] - 1].sectionBtnState = btnStates.On;
                            PerformZoneClick(i);
                        }
                    }
                }
            }
        }


        private void PerformZoneClick(int Btn)
        {
            (this.Controls.Find("btnZone" + (Btn + 1).ToString(), true).First() as Button).PerformClick();
        }

        private void PerformSectionClick(int Btn)
        {
            (this.Controls.Find("btnSection" + (Btn + 1).ToString() + "Man", true).First() as Button).PerformClick();
        }
    }
}
