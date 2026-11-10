using OpenTK.Graphics.OpenGL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public enum TrackMode { None = 0, AB = 2, Curve = 4, bndTrackOuter = 8, bndTrackInner = 16, bndCurve = 32, waterPivot = 64 };//, Heading, Circle, Spiral

    public class CTrack
    {
        //pointers to mainform controls
        private readonly FormGPS mf;

        public List<CTrk> gArr = new List<CTrk>();

        public int idx;

        public bool isAutoTrack = false, isAutoSnapToPivot = false, isAutoSnapped;

        public CTrack(FormGPS _f)
        {
            //constructor
            mf = _f;
            idx = -1;
        }

        #region Auto track

        // Picks the track the vehicle drives along while autosteer is off: heading gate first, then
        // cross track error to the nearest pass. Held at standstill; when reversing only a clear heading
        // match can switch, so backing into a corner never flips back to the previous track.
        private const double AutoTrackGate = 25.0 * Math.PI / 180.0;
        private const double AutoTrackSnap = 20.0 * Math.PI / 180.0;
        private const double AutoTrackSnapReverse = 10.0 * Math.PI / 180.0;
        private const double AutoTrackSnapBetter = 15.0 * Math.PI / 180.0;
        private const double AutoTrackMargin = 1.0;          // m
        private const double AutoTrackConfirmDist = 2.0;     // m
        private const double AutoTrackMinSpeed = 1.0;        // km/h
        private const double AutoTrackPenaltyPerDeg = 0.1;   // m per degree
        private const double AutoTrackEndMargin = 10.0;      // m past curve ends
        private const double AutoTrackFirstPassWeight = 0.5;
        private const double AutoTrackNearMin = 50.0;        // m

        //true while turning away from the selected track: AB line and curve keep their pass
        public bool isAutoTrackHoldingPass;

        private int autoTrackPending = -1;
        private double autoTrackTravel;
        private vec2 autoTrackLastPos;
        private bool autoTrackHasLastPos;

        public int UpdateAutoTrack(vec3 pivot, bool isReverse, double speedKmh)
        {
            double step = autoTrackHasLastPos ? glm.Distance(autoTrackLastPos, new vec2(pivot.easting, pivot.northing)) : 0;
            autoTrackLastPos = new vec2(pivot.easting, pivot.northing);
            autoTrackHasLastPos = true;

            if (idx < 0 || gArr.Count == 0 || idx >= gArr.Count) { ResetAutoTrackPending(); isAutoTrackHoldingPass = false; return -1; }

            AutoTrackScore(gArr[idx], pivot, true, out _, out double selectedDiff, out _);
            isAutoTrackHoldingPass = selectedDiff > AutoTrackGate;

            if (Math.Abs(speedKmh) < AutoTrackMinSpeed) { ResetAutoTrackPending(); return idx; }

            double nearLimit = Math.Max(2.0 * mf.tool.width, AutoTrackNearMin);
            int best = -1;
            double bestScore = double.MaxValue, bestDiff = Math.PI;
            double curScore = double.MaxValue, curDiff = Math.PI;
            bool curEligible = false;

            for (int i = 0; i < gArr.Count; i++)
            {
                if (!gArr[i].isVisible) continue;
                if (!AutoTrackScore(gArr[i], pivot, i == idx, out double score, out double diff, out double firstPassDist))
                {
                    if (i == idx) curDiff = diff;
                    continue;
                }
                if (i != idx && firstPassDist > nearLimit) continue;

                if (i == idx) { curEligible = true; curScore = score; curDiff = diff; }
                if (score < bestScore) { bestScore = score; best = i; bestDiff = diff; }
            }

            if (best < 0 || best == idx) { ResetAutoTrackPending(); return idx; }

            //heading clearly fits another track better: switch now
            if (bestDiff <= (isReverse ? AutoTrackSnapReverse : AutoTrackSnap) && curDiff >= bestDiff + AutoTrackSnapBetter)
            {
                ResetAutoTrackPending();
                isAutoTrackHoldingPass = false;
                return best;
            }

            if (isReverse) { ResetAutoTrackPending(); return idx; }

            if (curEligible && bestScore > curScore - AutoTrackMargin) { ResetAutoTrackPending(); return idx; }

            //otherwise switch when clearly better over a short distance forward
            if (best != autoTrackPending) { autoTrackPending = best; autoTrackTravel = 0; }
            autoTrackTravel += step;
            if (autoTrackTravel >= AutoTrackConfirmDist)
            {
                ResetAutoTrackPending();
                isAutoTrackHoldingPass = false;
                return best;
            }
            return idx;
        }

        public void ResetAutoTrackPending()
        {
            autoTrackPending = -1;
            autoTrackTravel = 0;
        }

        //false when outside the heading gate; diff is always set
        private bool AutoTrackScore(CTrk trk, vec3 pivot, bool isSelected, out double score, out double diff, out double firstPassDist)
        {
            score = double.MaxValue;
            diff = Math.PI;
            firstPassDist = double.MaxValue;
            double lateral, beyond, trackHeading;
            double margin = AutoTrackEndMargin;

            if (trk.mode == TrackMode.AB)
            {
                trackHeading = trk.heading;
                double dx = trk.ptB.easting - trk.ptA.easting;
                double dy = trk.ptB.northing - trk.ptA.northing;
                double len = Math.Sqrt(dx * dx + dy * dy);
                if (len < 0.01) return false;
                dx /= len; dy /= len;
                double px = pivot.easting - trk.ptA.easting, py = pivot.northing - trk.ptA.northing;
                lateral = Math.Abs(dx * py - dy * px);
                beyond = 0;
            }
            else
            {
                int n = trk.curvePts.Count;
                if (n < 2) return false;
                double best = double.MaxValue;
                lateral = beyond = 0;
                trackHeading = 0;
                for (int j = 0; j < n - 1; j++)
                {
                    vec3 a = trk.curvePts[j], b = trk.curvePts[j + 1];
                    double dx = b.easting - a.easting, dy = b.northing - a.northing;
                    double l2 = dx * dx + dy * dy;
                    if (l2 < 1e-6) continue;
                    double l = Math.Sqrt(l2);
                    double px = pivot.easting - a.easting, py = pivot.northing - a.northing;
                    double t = (px * dx + py * dy) / l2;
                    double ext = margin / l;
                    double tc = Math.Max(j == 0 ? -ext : 0, Math.Min(j == n - 2 ? 1 + ext : 1, t));
                    double d = glm.Distance(pivot.easting, pivot.northing, a.easting + tc * dx, a.northing + tc * dy);
                    if (d < best)
                    {
                        best = d;
                        trackHeading = Math.Atan2(dx, dy);
                        lateral = Math.Abs(dx * py - dy * px) / l;
                        beyond = Math.Sqrt(Math.Max(0, d * d - lateral * lateral));
                    }
                }
                if (best == double.MaxValue) return false;
            }

            if (trackHeading < 0) trackHeading += glm.twoPI;
            double raw = Math.PI - Math.Abs(Math.Abs(pivot.heading - trackHeading) - Math.PI);
            diff = Math.Min(raw, Math.PI - raw);
            if (diff > AutoTrackGate) return false;

            //passes lie at +-W/2 + k*W from the reference line; other tracks also pay for the distance
            //to their first pass, and only compete when that first pass is near
            double w = mf.tool.width - mf.tool.overlap;
            double xte = lateral, firstPass = lateral;
            if (w > 0.1)
            {
                xte = Math.Abs((lateral % w) - 0.5 * w);
                firstPass = Math.Abs(lateral - 0.5 * w);
            }

            firstPassDist = Math.Sqrt(firstPass * firstPass + beyond * beyond);
            score = Math.Sqrt(xte * xte + beyond * beyond) + AutoTrackPenaltyPerDeg * glm.toDegrees(diff);
            if (!isSelected) score += AutoTrackFirstPassWeight * firstPass;
            return true;
        }

        #endregion

        public void NudgeTrack(double dist)
        {
            if (idx > -1)
            {
                if (gArr[idx].mode == TrackMode.AB)
                {
                    mf.ABLine.isABValid = false;
                    gArr[idx].nudgeDistance += mf.ABLine.isHeadingSameWay ? dist : -dist;
                }
                else
                {
                    mf.curve.isCurveValid = false;
                    gArr[idx].nudgeDistance += mf.curve.isHeadingSameWay ? dist : -dist;

                }

                // Rebuild uturn after nudge to reflect new track position
                mf.yt.RebuildAfterNudge();

                //if (gArr[idx].nudgeDistance > 0.5 * mf.tool.width) gArr[idx].nudgeDistance -= mf.tool.width;
                //else if (gArr[idx].nudgeDistance < -0.5 * mf.tool.width) gArr[idx].nudgeDistance += mf.tool.width;
            }
        }

        public void NudgeDistanceReset()
        {
            if (idx > -1 && gArr.Count > 0)
            {
                if (gArr[idx].mode == TrackMode.AB)
                {
                    mf.ABLine.isABValid = false;
                }
                else
                {
                    mf.curve.isCurveValid = false;
                }

                gArr[idx].nudgeDistance = 0;

                // Rebuild uturn after reset to reflect new track position
                mf.yt.RebuildAfterNudge();
            }
        }

        public void SnapToPivot()
        {
            if (idx > -1)
            {
                NudgeTrack(gArr[idx].mode == TrackMode.AB ? mf.ABLine.distanceFromCurrentLinePivot : mf.curve.distanceFromCurrentLinePivot);
            }
        }

        public void NudgeRefTrack(double dist)
        {
            if (idx > -1)
            {
                if (gArr[idx].mode == TrackMode.AB)
                {
                    mf.ABLine.isABValid = false;
                    NudgeRefABLine(mf.ABLine.isHeadingSameWay ? dist : -dist);
                }
                else
                {
                    mf.curve.isCurveValid = false;
                    NudgeRefCurve(mf.curve.isHeadingSameWay ? dist : -dist);
                }
            }
        }

        public void NudgeRefABLine(double dist)
        {
            double head = gArr[idx].heading;

            gArr[idx].ptA.easting += (Math.Sin(head + glm.PIBy2) * (dist));
            gArr[idx].ptA.northing += (Math.Cos(head + glm.PIBy2) * (dist));

            gArr[idx].ptB.easting += (Math.Sin(head + glm.PIBy2) * (dist));
            gArr[idx].ptB.northing += (Math.Cos(head + glm.PIBy2) * (dist));
        }

        public void NudgeRefCurve(double distAway)
        {
            mf.curve.isCurveValid = false;

            List<vec3> curList = new List<vec3>();

            double distSqAway = (distAway * distAway) - 0.01;
            vec3 point;

            for (int i = 0; i < gArr[idx].curvePts.Count; i++)
            {
                point = new vec3(
                gArr[idx].curvePts[i].easting + (Math.Sin(glm.PIBy2 + gArr[idx].curvePts[i].heading) * distAway),
                gArr[idx].curvePts[i].northing + (Math.Cos(glm.PIBy2 + gArr[idx].curvePts[i].heading) * distAway),
                gArr[idx].curvePts[i].heading);
                bool Add = true;

                for (int t = 0; t < gArr[idx].curvePts.Count; t++)
                {
                    double dist = ((point.easting - gArr[idx].curvePts[t].easting) * (point.easting - gArr[idx].curvePts[t].easting))
                        + ((point.northing - gArr[idx].curvePts[t].northing) * (point.northing - gArr[idx].curvePts[t].northing));
                    if (dist < distSqAway)
                    {
                        Add = false;
                        break;
                    }
                }

                if (Add)
                {
                    if (curList.Count > 0)
                    {
                        double dist = ((point.easting - curList[curList.Count - 1].easting) * (point.easting - curList[curList.Count - 1].easting))
                            + ((point.northing - curList[curList.Count - 1].northing) * (point.northing - curList[curList.Count - 1].northing));
                        if (dist > 1.0)
                            curList.Add(point);
                    }
                    else curList.Add(point);
                }
            }

            int cnt = curList.Count;
            if (cnt > 6)
            {
                // Set ptA and ptB from the shifted raw points before Catmull-Rom extension
                gArr[idx].ptA = new vec2(curList[0].easting, curList[0].northing);
                gArr[idx].ptB = new vec2(curList[cnt - 1].easting, curList[cnt - 1].northing);

                vec3[] arr = new vec3[cnt];
                curList.CopyTo(arr);

                curList.Clear();

                for (int i = 0; i < (arr.Length - 1); i++)
                {
                    arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                    if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                    if (arr[i].heading >= glm.twoPI) arr[i].heading -= glm.twoPI;
                }

                arr[arr.Length - 1].heading = arr[arr.Length - 2].heading;

                //replace the array
                cnt = arr.Length;
                double distance;
                double spacing = 1.2;

                //add the first point of loop - it will be p1
                curList.Add(arr[0]);

                for (int i = 0; i < cnt - 3; i++)
                {
                    // add p2
                    curList.Add(arr[i + 1]);

                    distance = glm.Distance(arr[i + 1], arr[i + 2]);

                    if (distance > spacing)
                    {
                        int loopTimes = (int)(distance / spacing + 1);
                        for (int j = 1; j < loopTimes; j++)
                        {
                            vec3 pos = new vec3(glm.Catmull(j / (double)(loopTimes), arr[i], arr[i + 1], arr[i + 2], arr[i + 3]));
                            curList.Add(pos);
                        }
                    }
                }

                curList.Add(arr[cnt - 2]);
                curList.Add(arr[cnt - 1]);

                CABCurve.CalculateHeadings(ref curList);

                gArr[idx].curvePts.Clear();

                foreach (var item in curList)
                {
                    gArr[idx].curvePts.Add(new vec3(item));
                }

                //for (int i = 0; i < cnt; i++)
                //{
                //    arr[i].easting += Math.Cos(arr[i].heading) * (dist);
                //    arr[i].northing -= Math.Sin(arr[i].heading) * (dist);
                //    gArr[idx].curvePts.Add(arr[i]);
                //}
            }
        }
    }

    public class CTrk
    {
        public List<vec3> curvePts = new List<vec3>();
        public double heading;
        public string name;
        public bool isVisible;
        public vec2 ptA;
        public vec2 ptB;
        public vec2 endPtA;
        public vec2 endPtB;
        public TrackMode mode;
        public double nudgeDistance;
        public HashSet<int> workedTracks = new HashSet<int>();

        public CTrk()
        {
            curvePts = new List<vec3>();
            heading = 3;
            name = "New Track";
            isVisible = true;
            ptA = new vec2();
            ptB = new vec2();
            endPtA = new vec2();
            endPtB = new vec2();
            mode = TrackMode.None;
            nudgeDistance = 0;
        }

        public CTrk(CTrk _trk)
        {
            curvePts = new List<vec3>(_trk.curvePts);
            heading = _trk.heading;
            name = _trk.name;
            isVisible = _trk.isVisible;
            ptA = _trk.ptA;
            ptB = _trk.ptB;
            endPtA = new vec2();
            endPtB = new vec2();
            mode = _trk.mode;
            nudgeDistance = _trk.nudgeDistance;
        }
    }
}
