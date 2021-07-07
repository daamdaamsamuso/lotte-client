using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FTP.Config
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

        public string FTP_SERVER_ID;
        public string FTP_SERVER_PASSWORD;
        public string FTP_SERVER_URI;

        private ConfigValue()
        {
            _config = new Configs(
                Path.Combine(Directory.GetCurrentDirectory(), "Config"),
                "Ftp_Config.xml");
        }

        public void LoadConfigValue()
        {
            FTP_SERVER_ID = _config.Get("FTP_SERVER_ID");
            FTP_SERVER_PASSWORD = _config.Get("FTP_SERVER_PASSWORD");
            FTP_SERVER_URI = _config.Get("FTP_SERVER_URI");
        }
        public void SaveConfigValue(
            string FTP_SERVER_ID,
            string FTP_SERVER_PASSWORD,
            string FTP_SERVER_URI)
        {
            _config.Set("FTP_SERVER_ID", FTP_SERVER_ID);
            _config.Set("FTP_SERVER_PASSWORD", FTP_SERVER_PASSWORD);
            _config.Set("FTP_SERVER_URI", FTP_SERVER_URI);
        }
    }
}
