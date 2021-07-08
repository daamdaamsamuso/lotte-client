using System;
using System.Runtime.InteropServices;
using LotteCinemaService.WebAPI.Helper;

namespace LotteCinemaLibraries.Common.Class
{
    public static class Win32API
    {
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        } 

        [DllImport("kernel32.dll")]
        public extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);

        public static bool SyncDBTime(DateTime dateTime)
        {
            var serverTime = dateTime.ToUniversalTime();
            
            SYSTEMTIME systime = new SYSTEMTIME
            {
                wYear = (ushort)serverTime.Year,
                wMonth = (ushort)serverTime.Month,
                wDayOfWeek = (ushort)serverTime.DayOfWeek,
                wDay = (ushort)serverTime.Day,
                wHour = (ushort)serverTime.Hour,
                wMinute = (ushort)serverTime.Minute,
                wSecond = (ushort)serverTime.Second,
                wMilliseconds = (ushort)serverTime.Millisecond
            };

            return Win32API.SetSystemTime(ref systime) != 0;
        }
                                                        
        public static void SyncDBTime(string query, Uri serverUri)
        {

            
            //var beginTime = DateTime.Now;
            //var serverTime = WebApiHelper.GetResultJson<DateTime>(query, serverUri);
            //var endTime = DateTime.Now;

            //var gapTime = endTime - beginTime;

            //serverTime = serverTime.Add(gapTime);

            //return Win32API.SyncDBTime(serverTime);
        }
    }
}