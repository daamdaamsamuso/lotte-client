using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace lotte_Client.Config
{
    public class ConfigValue
    {
        #region ConfigValue Instance
        private static ConfigValue _instance;

        public static ConfigValue Instance
        {
            get
            {
                return (_instance) ?? (_instance = new ConfigValue());
            }
        }
        #endregion

        private readonly Configs _config;

        public string WebServerUri;
        public string CinemaCode;
        public List<string> CinemaCodesList;
        public int StatusNumber;
        public int DayBaseTimeHour;
        public int DurationTimeSec;
        public bool IsLocalVersion;
        public bool IsServer;
        public int ProgramNumber;
        public string GroupIp;
        public int GroupPort;
        public double SizeWidth;
        public double SizeHeight;

        private ConfigValue()
        {
            _config = new Configs(
                Path.Combine(Directory.GetCurrentDirectory(), "Config"), 
                "Config.xml");
        }

        public void LoadConfigValue()
        {
            //WebServerUri = _config.Get("WebServerUri");
            WebServerUri = "http://localhost:3000";
            CinemaCode = _config.Get("CinemaCode");
            StatusNumber = Convert.ToInt32(_config.Get("StatusNumber"));
            DayBaseTimeHour = Convert.ToInt32(_config.Get("DayBaseTimeHour"));
            DurationTimeSec = Convert.ToInt32(_config.Get("DurationTimeSec"));
            IsLocalVersion = Convert.ToBoolean(_config.Get("IsLocalVersion"));
            IsServer = Convert.ToBoolean(_config.Get("IsServer"));
            ProgramNumber = Convert.ToInt32(_config.Get("ProgramNumber"));
            GroupIp = Convert.ToString(_config.Get("GroupIp"));
            GroupPort = Convert.ToInt32(_config.Get("GroupPort"));
            SizeWidth = Convert.ToDouble(_config.Get("SizeWidth"));
            SizeHeight = Convert.ToDouble(_config.Get("SizeHeight"));

            var regex = new Regex(",");
            string[] cinemaCodes = regex.Split(CinemaCode);
            CinemaCodesList = new List<string>();

            for (int i = 0; i < 10; i++)
            {
                string cinemaCode = "";
                if (i < cinemaCodes.Count())
                {
                    cinemaCode = cinemaCodes[i];
                }

                CinemaCodesList.Add(cinemaCode);
            }
        }

        public void SaveConfigValue(
            string webServerUri,
            string cinemaCode,
            int statusNumber,
            bool isLocalVersion,
            bool isServer,
            string programNumber,
            string groupIp,
            string groupPort,
            string sizeWidth,
            string sizeHeight)
        {
            _config.Set("WebServerUri", webServerUri);
            _config.Set("CinemaCode", cinemaCode);
            _config.Set("StatusNumber", statusNumber.ToString());
            _config.Set("IsLocalVersion", isLocalVersion.ToString());
            _config.Set("IsServer", isServer.ToString());
            _config.Set("ProgramNumber", programNumber);
            _config.Set("GroupIp", groupIp);
            _config.Set("GroupPort", groupPort);
            _config.Set("SizeWidth", sizeWidth);
            _config.Set("SizeHeight", sizeHeight);
        }
    }
}
