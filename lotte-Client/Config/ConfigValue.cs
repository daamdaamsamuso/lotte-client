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
        public int DurationTimeSec;
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
            DurationTimeSec = Convert.ToInt32(_config.Get("DurationTimeSec"));
            GroupIp = Convert.ToString(_config.Get("GroupIp"));
            GroupPort = Convert.ToInt32(_config.Get("GroupPort"));
            SizeWidth = Convert.ToDouble(_config.Get("SizeWidth"));
            SizeHeight = Convert.ToDouble(_config.Get("SizeHeight"));
        }

        public void SaveConfigValue(
            string webServerUri,
            int statusNumber,
            string groupIp,
            string groupPort,
            string sizeWidth,
            string sizeHeight)
        {
            _config.Set("WebServerUri", webServerUri);
            _config.Set("GroupIp", groupIp);
            _config.Set("GroupPort", groupPort);
            _config.Set("SizeWidth", sizeWidth);
            _config.Set("SizeHeight", sizeHeight);
        }
    }
}
