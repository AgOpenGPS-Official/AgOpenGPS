//Please, if you use this, share the improvements

using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CPatches
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        //list of patch data individual triangles
        public List<vec3> triangleList = new List<vec3>();

        //list of the list of patch data individual triangles for that entire section activity
        public List<List<vec3>> patchList = new List<List<vec3>>();

        //mapping
        public bool isDrawing = false;

        //points in world space that start and end of section are in
        public vec2 leftPoint, rightPoint;

        public int numTriangles = 0;
        public int currentStartSectionNum, currentEndSectionNum;
        public int newStartSectionNum, newEndSectionNum;

        //simple constructor, position is set in GPSWinForm_Load in FormGPS when creating new object
        public CPatches(FormGPS _f)
        {
            //constructor
            mf = _f;
            patchList.Capacity = 2048;
            //triangleList.Capacity =
        }

        public void TurnMappingOn(int j)
        {
            numTriangles = 0;

            //do not tally square meters on inital point, that would be silly
            if (!isDrawing)
            {
                //set the section bool to on
                isDrawing = true;

                //starting a new patch chunk so create a new triangle list
                triangleList = new List<vec3>(64);

                patchList.Add(triangleList);

                if (!mf.tool.isMultiColoredSections)
                {
                    triangleList.Add(new vec3(mf.sectionColorDay.R, mf.sectionColorDay.G, mf.sectionColorDay.B));
                }
                else
                {
                    if (mf.tool.isSectionsNotZones)
                        triangleList.Add(new vec3(mf.tool.secColors[j].R, mf.tool.secColors[j].G, mf.tool.secColors[j].B));
                    else
                        triangleList.Add(new vec3(mf.sectionColorDay.R, mf.sectionColorDay.G, mf.sectionColorDay.B));
                }

                leftPoint = mf.section[currentStartSectionNum].leftPoint;
                rightPoint = mf.section[currentEndSectionNum].rightPoint;

                //left side of triangle
                triangleList.Add(new vec3(leftPoint.easting, leftPoint.northing, 0));

                //Right side of triangle
                triangleList.Add(new vec3(rightPoint.easting, rightPoint.northing, 0));

                mf.patchCounter++;
            }
        }

        public void TurnMappingOff()
        {
            AddMappingPoint(0);

            isDrawing = false;
            numTriangles = 0;

            if (triangleList.Count > 4)
            {
                //save the triangle list in a patch list to add to saving file
                mf.patchSaveList.Add(triangleList);
            }
            else
            {
                triangleList.Clear();
                if (patchList.Count > 0) patchList.RemoveAt(patchList.Count - 1);
            }
        }

        //every time a new fix, a new patch point from last point to this point
        //only need prev point on the first points of triangle strip that makes a box (2 triangles)

        public void AddMappingPoint(int j)
        {
            leftPoint = mf.section[currentStartSectionNum].leftPoint;
            rightPoint = mf.section[currentEndSectionNum].rightPoint;

            //add two triangles for next step.
            //left side

            //add the point to List
            triangleList.Add(new vec3(leftPoint.easting, leftPoint.northing, 0));

            //Right side
            triangleList.Add(new vec3(rightPoint.easting, rightPoint.northing, 0));

            //countExit the triangle pairs
            numTriangles++;

            //quick countExit
            int c = triangleList.Count - 1;

            //when closing a job the triangle patches all are emptied but the section delay keeps going.
            //Prevented by quick check. 4 points plus colour
            //if (c >= 5)
            {
                //calculate area of these 2 new triangles - AbsoluteValue of (Ax(By-Cy) + Bx(Cy-Ay) + Cx(Ay-By)/2)
                {
                    double temp = Math.Abs((triangleList[c].easting * (triangleList[c - 1].northing - triangleList[c - 2].northing))
                              + (triangleList[c - 1].easting * (triangleList[c - 2].northing - triangleList[c].northing))
                                  + (triangleList[c - 2].easting * (triangleList[c].northing - triangleList[c - 1].northing)));

                    temp += Math.Abs((triangleList[c - 1].easting * (triangleList[c - 2].northing - triangleList[c - 3].northing))
                              + (triangleList[c - 2].easting * (triangleList[c - 3].northing - triangleList[c - 1].northing))
                                  + (triangleList[c - 3].easting * (triangleList[c - 1].northing - triangleList[c - 2].northing)));

                    temp *= 0.5;
                    mf.fd.workedAreaTotal += temp;
                    mf.fd.workedAreaTotalUser += temp;
                }
            }

            if (numTriangles > 61)
            {
                numTriangles = 0;

                //save the cutoff patch to be saved later
                mf.patchSaveList.Add(triangleList);

                triangleList = new List<vec3>(64);

                patchList.Add(triangleList);

                //Add Patch colour
                if (!mf.tool.isMultiColoredSections)
                    triangleList.Add(new vec3(mf.sectionColorDay.R, mf.sectionColorDay.G, mf.sectionColorDay.B));
                else
                    triangleList.Add(new vec3(mf.tool.secColors[j].R, mf.tool.secColors[j].G, mf.tool.secColors[j].B));

                //add the points to List, yes its more points, but breaks up patches for culling
                triangleList.Add(new vec3(leftPoint.easting, leftPoint.northing, 0));
                triangleList.Add(new vec3(rightPoint.easting, rightPoint.northing, 0));
            }
        }
    }
}