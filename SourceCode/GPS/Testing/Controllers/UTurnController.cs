using System;
using AgOpenGPS.Core.Testing;

namespace AgOpenGPS.Testing.Controllers
{
    public class UTurnController : IUTurnController
    {
        private readonly FormGPS mf;

        public bool IsEnabled => mf.yt.isYouTurnBtnOn;

        public UTurnController(FormGPS formGPS)
        {
            mf = formGPS ?? throw new ArgumentNullException(nameof(formGPS));
        }

        public void Enable()
        {
            if (mf.yt.isYouTurnBtnOn)
            {
                return;
            }

            // Check if we have a boundary
            if (mf.bnd.bndList.Count == 0)
            {
                throw new InvalidOperationException("Cannot enable U-turn without a boundary");
            }

            // Check if we have a track selected
            if (mf.trk.idx == -1)
            {
                throw new InvalidOperationException("Cannot enable U-turn without a selected track");
            }

            mf.yt.ResetCreatedYouTurn();
            mf.yt.isYouTurnBtnOn = true;
            mf.yt.isTurnCreationTooClose = false;
            mf.yt.isTurnCreationNotCrossingError = false;
            mf.yt.ResetYouTurn();
        }

        public void Disable()
        {
            if (!mf.yt.isYouTurnBtnOn)
            {
                return;
            }

            mf.yt.isYouTurnBtnOn = false;
            mf.yt.ResetYouTurn();
            mf.yt.ResetCreatedYouTurn();
        }

        public void SetDistanceFromBoundary(double distanceMeters)
        {
            // Set the U-turn trigger distance from boundary
            mf.yt.uturnDistanceFromBoundary = distanceMeters;
        }

        public void SetTurnMode(UTurnMode mode)
        {
            // Currently not implemented in base system
            // Could be used to set different turn patterns in future
        }

        public UTurnState GetState()
        {
            return new UTurnState
            {
                IsActive = mf.yt.isYouTurnBtnOn,
                IsTriggered = mf.yt.isYouTurnTriggered,
                IsInTurn = mf.yt.isYouTurnTriggered && mf.yt.isTurnLeft, // Check if actively in turn
                DistanceFromBoundary = mf.yt.uturnDistanceFromBoundary
            };
        }
    }
}
