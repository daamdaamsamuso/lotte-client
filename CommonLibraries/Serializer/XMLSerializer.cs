using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Serializer
{
    public class XMLSerializer
    {
        public static void Serializer<T>(string xmlPath, T value)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

            using (var xw = XmlWriter.Create(xmlPath, settings))
            {
                serializer.Serialize(xw, value);
                xw.Close();
            }
        }

        public static T Deserializer<T>(string xmlPath)
        {
            var data = Activator.CreateInstance<T>();

            if (File.Exists(xmlPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (var xmlReader = XmlReader.Create(xmlPath))
                {
                    data = (T)serializer.Deserialize(xmlReader);
                }
            }

            return data;
        }

        public static T DeserializerFromString<T>(string str)
        {
            var data = Activator.CreateInstance<T>();

            if (!string.IsNullOrWhiteSpace(str))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (StringReader xmlReader = new StringReader(str))
                {
                    data = (T)serializer.Deserialize(xmlReader);
                }
            }

            return data;
        }
    }
}