using System;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        /// <summary>
        /// Updates section control logic - determines which sections should be on/off
        /// and manages mapping status. Extracted from oglBack_Paint to allow operation
        /// in headless mode without requiring Paint events.
        /// </summary>
        public void UpdateSectionControl()
        {
            // Local variables for section control calculations
            int start = 0, end = 0, tagged = 0, totalPixel = 0;
            double mOn = 0, mOff = 0;
            int endHeight = 1, startHeight = 1;

            ///////////////////////////////////////////   Section control        ssssssssssssssssssssss

            if (bnd.isHeadlandOn && bnd.isSectionControlledByHeadland) bnd.WhereAreToolLookOnPoints();

            for (int j = 0; j < tool.numOfSections; j++)
            {
                //Off or too slow or going backwards
                if (section[j].sectionBtnState == btnStates.Off || avgSpeed < vehicle.slowSpeedCutoff || section[j].speedPixels < 0)
                {
                    section[j].sectionOnRequest = false;
                    section[j].sectionOffRequest = true;

                    // Manual on, force the section On
                    if (section[j].sectionBtnState == btnStates.On)
                    {
                        section[j].sectionOnRequest = true;
                        section[j].sectionOffRequest = false;
                        continue;
                    }
                    continue;
                }

                // Manual on, force the section On
                if (section[j].sectionBtnState == btnStates.On)
                {
                    section[j].sectionOnRequest = true;
                    section[j].sectionOffRequest = false;
                    continue;
                }

                //AutoSection - If any nowhere applied, send OnRequest, if its all green send an offRequest
                section[j].isSectionRequiredOn = false;

                //calculate the slopes of the lines
                mOn = (tool.lookAheadDistanceOnPixelsRight - tool.lookAheadDistanceOnPixelsLeft) / tool.rpWidth;
                mOff = (tool.lookAheadDistanceOffPixelsRight - tool.lookAheadDistanceOffPixelsLeft) / tool.rpWidth;

                start = section[j].rpSectionPosition - section[0].rpSectionPosition;
                end = section[j].rpSectionWidth - 1 + start;

                if (end >= tool.rpWidth)
                    end = tool.rpWidth - 1;

                totalPixel = 1;
                tagged = 0;

                for (int pos = start; pos <= end; pos++)
                {
                    startHeight = (int)(tool.lookAheadDistanceOffPixelsLeft + (mOff * pos)) * tool.rpWidth + pos;
                    endHeight = (int)(tool.lookAheadDistanceOnPixelsLeft + (mOn * pos)) * tool.rpWidth + pos;

                    for (int a = startHeight; a <= endHeight; a += tool.rpWidth)
                    {
                        totalPixel++;
                        if (grnPixels[a] == 0) tagged++;
                    }
                }

                //determine if meeting minimum coverage
                section[j].isSectionRequiredOn = ((tagged * 100) / totalPixel > (100 - tool.minCoverage));

                //logic if in or out of boundaries or headland
                if (bnd.bndList.Count > 0)
                {
                    //if out of boundary, turn it off
                    if (!section[j].isInBoundary)
                    {
                        section[j].isSectionRequiredOn = false;
                        section[j].sectionOffRequest = true;
                        section[j].sectionOnRequest = false;
                        section[j].sectionOffTimer = 0;
                        section[j].sectionOnTimer = 0;
                        continue;
                    }
                    else
                    {
                        //is headland coming up
                        if (bnd.isHeadlandOn && bnd.isSectionControlledByHeadland)
                        {
                            bool isHeadlandInLookOn = false;

                            //is headline in off to on area
                            mOn = (tool.lookAheadDistanceOnPixelsRight - tool.lookAheadDistanceOnPixelsLeft) / tool.rpWidth;
                            mOff = (tool.lookAheadDistanceOffPixelsRight - tool.lookAheadDistanceOffPixelsLeft) / tool.rpWidth;

                            start = section[j].rpSectionPosition - section[0].rpSectionPosition;

                            end = section[j].rpSectionWidth - 1 + start;

                            if (end >= tool.rpWidth)
                                end = tool.rpWidth - 1;

                            tagged = 0;

                            for (int pos = start; pos <= end; pos++)
                            {
                                startHeight = (int)(tool.lookAheadDistanceOffPixelsLeft + (mOff * pos)) * tool.rpWidth + pos;
                                endHeight = (int)(tool.lookAheadDistanceOnPixelsLeft + (mOn * pos)) * tool.rpWidth + pos;

                                for (int a = startHeight; a <= endHeight; a += tool.rpWidth)
                                {
                                    if (a < 0)
                                        mOn = 0;
                                    if (grnPixels[a] == 250)
                                    {
                                        isHeadlandInLookOn = true;
                                        goto GetOutHdOn;
                                    }
                                }
                            }
                        GetOutHdOn:

                            //determine if look ahead points are completely in headland
                            if (section[j].isSectionRequiredOn && section[j].isLookOnInHeadland && !isHeadlandInLookOn)
                            {
                                section[j].isSectionRequiredOn = false;
                                section[j].sectionOffRequest = true;
                                section[j].sectionOnRequest = false;
                            }

                            if (section[j].isSectionRequiredOn && !section[j].isLookOnInHeadland && isHeadlandInLookOn)
                            {
                                section[j].isSectionRequiredOn = true;
                                section[j].sectionOffRequest = false;
                                section[j].sectionOnRequest = true;
                            }
                        }
                    }
                }


                //global request to turn on section
                section[j].sectionOnRequest = section[j].isSectionRequiredOn;
                section[j].sectionOffRequest = !section[j].sectionOnRequest;

            }  // end of go thru all sections "for"

            //Set all the on and off times based from on off section requests
            for (int j = 0; j < tool.numOfSections; j++)
            {
                //SECTION timers

                if (section[j].sectionOnRequest)
                    section[j].isSectionOn = true;

                //turn off delay
                if (tool.turnOffDelay > 0)
                {
                    if (!section[j].sectionOffRequest) section[j].sectionOffTimer = (int)(gpsHz / 2.0 * tool.turnOffDelay);

                    if (section[j].sectionOffTimer > 0) section[j].sectionOffTimer--;

                    if (section[j].sectionOffRequest && section[j].sectionOffTimer == 0)
                    {
                        if (section[j].isSectionOn) section[j].isSectionOn = false;
                    }
                }
                else
                {
                    if (section[j].sectionOffRequest)
                        section[j].isSectionOn = false;
                }

                //Mapping timers
                if (section[j].sectionOnRequest && !section[j].isMappingOn && section[j].mappingOnTimer == 0)
                {
                    section[j].mappingOnTimer = (int)(tool.lookAheadOnSetting * (gpsHz / 2) - 1);
                }
                else if (section[j].sectionOnRequest && section[j].isMappingOn && section[j].mappingOffTimer > 1)
                {
                    section[j].mappingOffTimer = 0;
                    section[j].mappingOnTimer = (int)(tool.lookAheadOnSetting * (gpsHz / 2) - 1);
                }

                if (tool.lookAheadOffSetting > 0)
                {
                    if (section[j].sectionOffRequest && section[j].isMappingOn && section[j].mappingOffTimer == 0)
                    {
                        section[j].mappingOffTimer = (int)(tool.lookAheadOffSetting * (gpsHz / 2) + 4);
                    }
                }
                else if (tool.turnOffDelay > 0)
                {
                    if (section[j].sectionOffRequest && section[j].isMappingOn && section[j].mappingOffTimer == 0)
                        section[j].mappingOffTimer = (int)(tool.turnOffDelay * gpsHz / 2);
                }
                else
                {
                    section[j].mappingOffTimer = 0;
                }

                //MAPPING - Not the making of triangle patches - only status - on or off
                if (section[j].sectionOnRequest)
                {
                    section[j].mappingOffTimer = 0;
                    if (section[j].mappingOnTimer > 1)
                        section[j].mappingOnTimer--;
                    else
                    {
                        section[j].isMappingOn = true;
                    }
                }

                if (section[j].sectionOffRequest)
                {
                    section[j].mappingOnTimer = 0;
                    if (section[j].mappingOffTimer > 1)
                        section[j].mappingOffTimer--;
                    else
                    {
                        section[j].isMappingOn = false;
                    }
                }
            }

            //Checks the workswitch or steerSwitch if required
            if (ahrs.isAutoSteerAuto || mc.isRemoteWorkSystemOn)
                mc.CheckWorkAndSteerSwitch();

            // check if any sections have changed status
            number = 0;

            for (int j = 0; j < tool.numOfSections; j++)
            {
                if (section[j].isMappingOn)
                {
                    number |= 1ul << j;
                }
            }

            //there has been a status change of section on/off
            if (number != lastNumber)
            {
                int sectionOnOffZones = 0, patchingZones = 0;

                //everything off
                if (number == 0)
                {
                    for (int j = 0; j < triStrip.Count; j++)
                    {
                        if (triStrip[j].isDrawing)
                            triStrip[j].TurnMappingOff();
                    }
                }
                else if (!tool.isMultiColoredSections)
                {
                    //set the start and end positions from section points
                    for (int j = 0; j < tool.numOfSections; j++)
                    {
                        //skip till first mapping section
                        if (!section[j].isMappingOn) continue;

                        //do we need more patches created
                        if (triStrip.Count < sectionOnOffZones + 1)
                            triStrip.Add(new CPatches(this));

                        //set this strip start edge to edge of this section
                        triStrip[sectionOnOffZones].newStartSectionNum = j;

                        while ((j + 1) < tool.numOfSections && section[j + 1].isMappingOn)
                        {
                            j++;
                        }

                        //set the edge of this section to be end edge of strp
                        triStrip[sectionOnOffZones].newEndSectionNum = j;
                        sectionOnOffZones++;
                    }

                    //countExit current patch strips being made
                    for (int j = 0; j < triStrip.Count; j++)
                    {
                        if (triStrip[j].isDrawing) patchingZones++;
                    }

                    //tests for creating new strips or continuing
                    bool isOk = (patchingZones == sectionOnOffZones && sectionOnOffZones < 3);

                    if (isOk)
                    {
                        for (int j = 0; j < sectionOnOffZones; j++)
                        {
                            if (triStrip[j].newStartSectionNum > triStrip[j].currentEndSectionNum
                                || triStrip[j].newEndSectionNum < triStrip[j].currentStartSectionNum)
                                isOk = false;
                        }
                    }

                    if (isOk)
                    {
                        for (int j = 0; j < sectionOnOffZones; j++)
                        {
                            if (triStrip[j].newStartSectionNum != triStrip[j].currentStartSectionNum
                                || triStrip[j].newEndSectionNum != triStrip[j].currentEndSectionNum)
                            {
                                //if (tool.isSectionsNotZones)
                                {
                                    triStrip[j].AddMappingPoint(0);
                                }

                                triStrip[j].currentStartSectionNum = triStrip[j].newStartSectionNum;
                                triStrip[j].currentEndSectionNum = triStrip[j].newEndSectionNum;
                                triStrip[j].AddMappingPoint(0);
                            }
                        }
                    }
                    else
                    {
                        //too complicated, just make new strips
                        for (int j = 0; j < triStrip.Count; j++)
                        {
                            if (triStrip[j].isDrawing)
                                triStrip[j].TurnMappingOff();
                        }

                        for (int j = 0; j < sectionOnOffZones; j++)
                        {
                            triStrip[j].currentStartSectionNum = triStrip[j].newStartSectionNum;
                            triStrip[j].currentEndSectionNum = triStrip[j].newEndSectionNum;
                            triStrip[j].TurnMappingOn(0);
                        }
                    }
                }
                else if (tool.isMultiColoredSections) //could be else only but this is more clear
                {
                    //set the start and end positions from section points
                    for (int j = 0; j < tool.numOfSections; j++)
                    {
                        //do we need more patches created
                        if (triStrip.Count < sectionOnOffZones + 1)
                            triStrip.Add(new CPatches(this));

                        //set this strip start edge to edge of this section
                        triStrip[sectionOnOffZones].newStartSectionNum = j;

                        //set the edge of this section to be end edge of strp
                        triStrip[sectionOnOffZones].newEndSectionNum = j;
                        sectionOnOffZones++;

                        if (!section[j].isMappingOn)
                        {
                            if (triStrip[j].isDrawing)
                                triStrip[j].TurnMappingOff();
                        }
                        else
                        {
                            triStrip[j].currentStartSectionNum = triStrip[j].newStartSectionNum;
                            triStrip[j].currentEndSectionNum = triStrip[j].newEndSectionNum;
                            triStrip[j].TurnMappingOn(j);
                        }
                    }
                }


                lastNumber = number;
            }

            //send the byte out to section machines
            BuildMachineByte();
        }
    }
}
