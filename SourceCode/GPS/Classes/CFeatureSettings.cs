namespace AgOpenGPS
{
    public class CFeatureSettings
    {
        public CFeatureSettings()
        { }

        //public bool ;
        public bool isHeadlandOn = true;

        public bool isTramOn = false;
        public bool isBoundaryOn = true;
        public bool isBndContourOn = false;
        public bool isRecPathOn = false;
        public bool isABSmoothOn = false;

        public bool isHideContourOn = false;
        public bool isWebCamOn = false;
        public bool isOffsetFixOn = false;
        public bool isAgIOOn = true;

        public bool isContourOn = true;
        public bool isYouTurnOn = true;
        public bool isSteerModeOn = true;

        public bool isManualSectionOn = true;
        public bool isAutoSectionOn = true;
        public bool isCycleLinesOn = true;
        public bool isABLineOn = true;
        public bool isCurveOn = true;
        public bool isAutoSteerOn = true;

        public bool isUTurnOn = true;
        public bool isLateralOn = true;

        public CFeatureSettings(CFeatureSettings _feature)
        {
            isHeadlandOn = _feature.isHeadlandOn;
            isTramOn = _feature.isTramOn;
            isBoundaryOn = _feature.isBoundaryOn;
            isBndContourOn = _feature.isBndContourOn;
            isRecPathOn = _feature.isRecPathOn;

            isABSmoothOn = _feature.isABSmoothOn;
            isHideContourOn = _feature.isHideContourOn;
            isWebCamOn = _feature.isWebCamOn;
            isOffsetFixOn = _feature.isOffsetFixOn;
            isAgIOOn = _feature.isAgIOOn;

            isContourOn = _feature.isContourOn;
            isYouTurnOn = _feature.isYouTurnOn;
            isSteerModeOn = _feature.isSteerModeOn;

            isManualSectionOn = _feature.isManualSectionOn;
            isAutoSectionOn = _feature.isAutoSectionOn;
            isCycleLinesOn = _feature.isCycleLinesOn;
            isABLineOn = _feature.isABLineOn;
            isCurveOn = _feature.isCurveOn;

            isAutoSteerOn = _feature.isAutoSteerOn;
            isLateralOn = _feature.isLateralOn;
            isUTurnOn = _feature.isUTurnOn;
        }
    }
}