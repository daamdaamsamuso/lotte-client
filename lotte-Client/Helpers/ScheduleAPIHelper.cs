using lotte_Client.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotte_Client.Helpers
{
    public static class ScheduleAPIHelper
    {
        public static string ServiceURL = "http://localhost:3000";
        public static string ServiceName = "api";
        public static List<AdInfoRaw> list = new List<AdInfoRaw>();
        public static void Run()
        {
            list = WebApiHelper.GetResultJson<List<AdInfoRaw>>(ServiceName, new Uri(ServiceURL, UriKind.RelativeOrAbsolute)); ;
        }
    }
}
