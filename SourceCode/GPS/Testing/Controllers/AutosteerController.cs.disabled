using System;
using AgOpenGPS.Core.Testing;

namespace AgOpenGPS.Testing.Controllers
{
    public class AutosteerController : IAutosteerController
    {
        private readonly FormGPS mf;

        public bool IsEnabled => mf.isBtnAutoSteerOn;

        public AutosteerController(FormGPS formGPS)
        {
            mf = formGPS ?? throw new ArgumentNullException(nameof(formGPS));
        }

        public void Enable()
        {
            if (mf.isBtnAutoSteerOn)
            {
                return;
            }

            // Check if we have a valid guidance line (either contour or track)
            if (mf.ct.isContourBtnOn || mf.trk.idx > -1)
            {
                mf.isBtnAutoSteerOn = true;
                if (mf.yt.isYouTurnBtnOn)
                {
                    mf.yt.ResetYouTurn();
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot enable autosteer without a valid guidance line (contour or track)");
            }
        }

        public void Disable()
        {
            if (!mf.isBtnAutoSteerOn)
            {
                return;
            }

            mf.isBtnAutoSteerOn = false;
        }

        public void SetGuidanceMode(GuidanceMode mode)
        {
            mf.isStanleyUsed = (mode == GuidanceMode.Stanley);
        }

        public AutosteerState GetState()
        {
            GuidanceMode currentMode = mf.isStanleyUsed ? GuidanceMode.Stanley : GuidanceMode.PurePursuit;

            return new AutosteerState
            {
                IsActive = mf.isBtnAutoSteerOn,
                CrossTrackError = mf.ABLine.distanceFromCurrentLine,
                SteerAngleDemand = mf.mc.actualSteerAngleDegrees,
                Mode = currentMode,
                GoalPointDistance = mf.ABLine.goalPointDistance
            };
        }
    }
}
