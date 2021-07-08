using System;

namespace LotteCinemaService.WebAPI.Helper
{
    public static class WebApiTBAQuery
    {
        public static string GetCurrentTime()
        {
            return "TBA/CurrentTime";
        }

        public static string GetMovieInfoList(string cinemaCode1, string cinemaCode2)
        {
            return string.Format("TBA/MovieInfoList?cinemaCode1={0}&cinemaCode2={1}", cinemaCode1, cinemaCode2);
        }

        public static string GetMovieTimeCellInfoList(string cinemaCode1, string cinemaCode2)
        {
            return string.Format("TBA/MovieTimeCellInfoList?cinemaCode1={0}&cinemaCode2={1}", cinemaCode1, cinemaCode2);
        }

        public static string GetAdInfoList(string cinemaCode, bool isSpecial)
        {
            return string.Format("TBA/AdInfoList?cinemaCode={0}&isSpecial={1}", cinemaCode, isSpecial);
        }

        public static string GetTheaterFloorInfoList(string cinemaCode1, string cinemaCode2)
        {
            return string.Format("TBA/TheaterFloorInfoList?cinemaCode1={0}&cinemaCode2={1}", cinemaCode1, cinemaCode2);
        }

        public static string GetNoticeInfoList(string cinemaCode)
        {
            return string.Format("TBA/NoticeInfoList?cinemaCode={0}", cinemaCode);
        }

        public static string GetPopupNoticeInfoList(string cinemaCode)
        {
            return string.Format("TBA/PopupNoticeInfoList?cinemaCode={0}", cinemaCode);
        }

        public static string GetPlannedMovieInfoList(string cinemaCode)
        {
            return string.Format("TBA/PlannedMovieInfoList?cinemaCode={0}", cinemaCode);
        }

        public static string GetAdverTime(string theater)
        {
            return string.Format("TBA/AdverTime?theater={0}", theater);
        }

        public static string GetAdverMainInfoCode(string cinemaCode, bool isBoxOffice)
        {
            return string.Format("TBA/AdverMainInfoCode?cinemaCode={0}&isBoxOffice={1}", cinemaCode, isBoxOffice);
        }

        public static string SetADLogInfo(string adverInfoCode, string cinemaCode, int adverType, DateTime startTime, DateTime endTime, bool isBoxOffice)
        {
            return string.Format("TBA/AdverLogInfo?adverInfoCode={0}&cinemaCode={1}&adverType={2}&startTime={3}&endTime={4}&isBoxOffice={5}",
                adverInfoCode, cinemaCode, adverType, startTime, endTime, isBoxOffice);
        }

        public static string SetADUpdate(string adverInfoCode, string update, bool isBoxOffice)
        {
            return string.Format("TBA/AdverUpdate?adverInfoCode={0}&update={1}&isBoxOffice={2}",
                adverInfoCode, update, isBoxOffice);
        }

        public static string GetBoxOfficeList()
        {
            return "TBA/BoxOfficeList";
        }

        public static string GetOneLineNotices(string cinemaCode)
        {
            return string.Format("TBA/OneLineNotices?cinemaCode={0}", cinemaCode);
        }

        public static string GetGeneralNotice(string cinemaCode)
        {
            return string.Format("TBA/GeneralNotice?cinemaCode={0}", cinemaCode);
        }

        public static string GetForecastList()
        {
            return "TBA/ForecastList";
        }

        public static string GetPosterAdList(string cinemaCode)
        {
            return string.Format("TBA/PosterAdList?cinemaCode={0}", cinemaCode);
        }

        public static string GetTransparentAdInfoList(string cinemaCode)
        {
            return string.Format("TBA/TransparentAdInfoList?cinemaCode={0}", cinemaCode);
        }

        public static string SetItemStatus()
        {
            return "TBA/SetItemStatus";
        }

        public static string SetAdLog()
        {
            return "TBA/SetAdLog";
        }
    }
}