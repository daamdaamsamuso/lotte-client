using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotte_Client.Utils
{
    public class Log
    {
        public static void WriteLine(string text)
        {
            if (!Directory.Exists("Log"))
            {
                Directory.CreateDirectory("Log");
            }

            var fs = new FileStream(Path.Combine("Log", DateTime.Now.ToString("yyyy.MM.dd") + "_Log.txt"), FileMode.Append);
            var sw = new StreamWriter(fs, Encoding.Default, text.Length);
            sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + text);
            sw.Close();
            fs.Close();
        }
    }
}
