using System.IO;
using System.Xml;

namespace lotte_Client.Config
{
    public sealed class Configs
    {
        private readonly string _filename;
        private readonly XmlDocument _doc = new XmlDocument();

        private const string EmptyFile =
            @"<?xml version=""1.0"" encoding=""utf-8"" ?>
          <configuration>
              <appSettings>
                  <add key=""WebServerUri"" value=""http://localhost:55260"" />
                  <add key=""CinemaCode"" value=""1004,0003"" />
                  <add key=""DayBaseTimeHour"" value=""4"" />
                  <add key=""DurationTimeSec"" value=""30"" />
                  <add key=""IsLocalVersion"" value=""false"" />
                  <add key=""IsServer"" value=""false"" />
                  <add key=""ProgramNumber"" value=""1"" />
                  <add key=""GroupIp"" value=""224.0.0.201"" />
                  <add key=""GroupPort"" value=""8080"" />
                  <add key=""SizeWidth"" value=""1080"" />
                  <add key=""SizeHeight"" value=""1920"" />
              </appSettings>
          </configuration>";

        public Configs(string path, string filename)
        {
            // strip any trailing backslashes...
            while (path.Length > 0 && path.EndsWith("\\"))
            {
                path = path.Remove(path.Length - 1, 1);
            }

            _filename = Path.Combine(path, filename);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(_filename))
            {
                // Create it...
                _doc.LoadXml(EmptyFile);
                _doc.Save(_filename);
            }
            else
            {
                _doc.Load(_filename);
            }

        }

        #region Get
        /// <summary>
        /// Retrieve a value by name.
        /// Returns the supplied DefaultValue if not found.
        /// </summary>
        public string Get(string key, string defaultValue)
        {
            XmlNode node = _doc.SelectSingleNode("configuration/appSettings/add[@key='" + key + "']");

            if (node == null)
            {
                return defaultValue;
            }
            return node.Attributes["value"].Value ?? defaultValue;
        }

        public string Get(string key)
        {
            XmlNode node = _doc.SelectSingleNode("configuration/appSettings/add[@key='" + key + "']");

            if (node == null)
            {
                return null;
            }
            return node.Attributes["value"].Value ?? null;
        } 
        #endregion

        #region void Set(string key, string value)
        /// <summary>
        /// Write a config value by key
        /// </summary>
        public void Set(string key, string value)
        {
            XmlNode node = _doc.SelectSingleNode("configuration/appSettings/add[@key='" + key + "']");

            if (node != null)
            {
                node.Attributes["value"].Value = value;

                _doc.Save(_filename);
            }
        } 
        #endregion
    }
}
