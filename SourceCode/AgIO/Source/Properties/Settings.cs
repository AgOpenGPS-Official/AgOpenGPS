using AgLibrary.Logging;
using AgLibrary.Settings;
using System.Collections.Generic;
using System.IO;

namespace AgIO.Properties
{
    internal sealed class Settings
    {
        private static Settings settings_ = new Settings();

        public static Settings Default
        {
            get
            {
                return settings_;
            }
        }

        public string setPort_portNameGPS = "GPS**";
        public int setPort_baudRateGPS = 9600;
        public bool setUDP_isOn = false;
        public bool setPort_wasSteerModuleConnected = false;
        public string setPort_portNameSteer = "Steer*";
        public bool setPort_wasModule3Connected = false;
        public string setPort_portNameTool = "Tool**";
        public int setIP_thisPort = 9999;
        public int setIP_autoSteerPort = 8888;
        public string setNTRIP_casterIP = "69.75.31.235";
        public int setNTRIP_casterPort = 2101;
        public string setNTRIP_mount = "SCSC";
        public string setNTRIP_userName = "";
        public string setNTRIP_userPassword = "";
        public bool setNTRIP_isOn = false;
        public int setNTRIP_sendGGAInterval = 10;
        public int setNTRIP_sendToUDPPort = 2233;
        public double setNTRIP_manualLat = 53;
        public double setNTRIP_manualLon = -111;
        public string setNTRIP_casterURL = "NTRIP.itsware.net";
        public bool setNTRIP_isGGAManual = false;
        public bool setNTRIP_isTCP = false;
        public bool setNTRIP_isHTTP10 = false;
        public bool setPgm_isFirstRun = true;
        public string setPort_portNameGPS2 = "GPS2";
        public int setPort_baudRateGPS2 = 9600;
        public string setPort_portNameMachine = "Mach**";
        public bool setPort_wasMachineModuleConnected = false;
        public bool setPort_wasIMUConnected = false;
        public string setPort_portNameIMU = "IMU";
        public bool setPort_wasGPSConnected = false;
        public string setPort_portNameRadio = "***";
        public string setPort_baudRateRadio = "9600";
        public string setPort_radioChannel = "439.000";
        public bool setRadio_isOn = false;
        public List<CRadioChannel> setRadio_Channels = new List<CRadioChannel>();
        public bool setUDP_isSendNMEAToUDP = false;
        public string setPort_portNameRtcm = "RTCM";
        public int setPort_baudRateRtcm = 9600;
        public bool setPort_wasRtcmConnected = false;
        public bool setNTRIP_sendToSerial = true;
        public bool setNTRIP_sendToUDP = true;
        public int setNTRIP_packetSize = 256;
        public bool setMod_isIMUConnected = true;
        public bool setMod_isMachineConnected = true;
        public bool setMod_isSteerConnected = true;
        public bool setPass_isOn = false;
        public byte etIP_SubnetOne = 192;
        public byte etIP_SubnetTwo = 168;
        public byte etIP_SubnetThree = 5;
        public byte eth_loopOne = 127;
        public byte eth_loopTwo = 255;
        public byte eth_loopThree = 255;
        public byte eth_loopFour = 255;
        public bool setDisplay_isAutoRunGPS_Out = false;
        public bool setDisplay_StartMinimized = false;
        public LoadResult Load()
        {
            string path = Path.Combine(RegistrySettings.profileDirectory, RegistrySettings.profileName + ".XML");
            var result = XmlSettingsHandler.LoadXMLFile(path, this);
            if (result == LoadResult.MissingFile)
            {
                if (RegistrySettings.profileName != "")
                {
                    RegistrySettings.Save(RegKeys.profileName, "");
                    Log.EventWriter("Profile file does not exist, Default Profile used");
                }
                else
                {
                    Log.EventWriter("Default Profile used");
                }
            }

            return result;
        }

        public void Save()
        {
            string path = Path.Combine(RegistrySettings.profileDirectory, RegistrySettings.profileName + ".XML");

            if (RegistrySettings.profileName != "")
                XmlSettingsHandler.SaveXMLFile(path, this);
            else
            {
                Log.EventWriter("Default Profile Not saved to Profiles");
            }
        }

        public void Reset()
        {
            settings_ = new Settings();
            settings_.Save();
        }
    }
}