using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteCinemaService.WebAPI.Helper.LCSM
{
    public static class WebApiLCSMQuery
    {

        public static string GetMovieShowingList(string PlayDate, string CinemaCode1, string CinemaCode2)
        {
            StringBuilder str = new StringBuilder();
            str.Append("LCHI/DID/Request?MT=GetMovieShowingList&PV=");
            str.Append(PlayDate);
            str.Append("|`|");
            str.Append(CinemaCode1);
            str.Append("|`|");
            str.Append(CinemaCode2);
            return str.ToString();
        }

        public static string GetMovieTimeCellInfoList(string playdate, string theater1, string theater2)
        {
            StringBuilder str = new StringBuilder();
            str.Append("LCHI/DID/Request?MT=GetMovieShowingDetailList&PV=");
            str.Append(playdate);
            str.Append("|`|");
            str.Append(theater1);
            str.Append("|`|");
            str.Append(theater2);
            return str.ToString();
        }

        public static string GetMobileTicketInfoRawList(string cinemaCode, string bookingNo)
        {
            StringBuilder str = new StringBuilder();
            str.Append("LCHI/DID/Request?MT=GetMobileTicketInfoList&PV=");
            str.Append(cinemaCode);
            str.Append("|`|");
            str.Append(bookingNo);
            return str.ToString();
        }

        public static string GetTicketInfoRawList(string cinemaCode, string playDate, string ticketNo)
        {
            StringBuilder str = new StringBuilder();
            str.Append("LCHI/DID/Request?MT=GetTicketInfoList&PV=");
            str.Append(cinemaCode);
            str.Append("|`|");
            str.Append(playDate);
            str.Append("|`|");
            str.Append(ticketNo);
            return str.ToString();
        }

        public static string GetTicketInfoRawList(string cinemaCode, string ticketNo)
        {
            StringBuilder str = new StringBuilder();
            str.Append("LCHI/DID/Request?MT=GetTicketInfoList&PV=");
            str.Append(cinemaCode);
            str.Append("|`|");
            str.Append(ticketNo);
            return str.ToString();
        }

        public static string GetScreenSeatInfoRawList(string cinemaCode, string screenCode)
        {
            StringBuilder str = new StringBuilder();
            str.Append("LCHI/DID/Request?MT=GetScreenSeatList&PV=");
            str.Append(cinemaCode);
            str.Append("|`|");
            str.Append(screenCode);
            return str.ToString();
        }

        public static string GetMovistInfoRaw(string contentsCode)
        {
            StringBuilder str = new StringBuilder();
            str.Append("LCHI/DID/Request?MT=GetMovistInfoList&PV=");
            str.Append(contentsCode);
            return str.ToString();
        }

        public static string GetScreenDspInfoRawList(string cinemaCode, string screenCode)
        {
            StringBuilder str = new StringBuilder();
            str.Append("LCHI/DID/Request?MT=GetScreenDspList&PV=");
            str.Append(cinemaCode);
            str.Append("|`|");
            str.Append(screenCode);
            return str.ToString();
        }



        public static string GetRecommandMovieInfoList(string MovieInfoCode)
        {
            StringBuilder str = new StringBuilder();
            str.Append("LCHI/DID/Request?MT=GetRecommandedMovieInfoList&PV=");
            str.Append(MovieInfoCode);
            return str.ToString();
        }

        public static string GetBoxOfficeList()
        {
            return "LCHI/DID/Request?MT=GetBoxOfficeList";
        }

        public static string GetMovieContentsUploadAvailableInfoList(DateTime startDate, DateTime endDate)
        {
            StringBuilder str = new StringBuilder();
            str.Append("LCHI/DID/Request?MT=GetMovieContentsUploadAvailableList&PV=");
            str.Append(startDate.ToShortDateString());
            str.Append("|`|");
            str.Append(endDate.ToShortDateString());
            str.Append("|`|");
            return str.ToString();
        }

        /// <summary>
        /// 새로 요청해야함(이름검색)
        /// </summary>
        /// <param name="searchTitle"></param>
        /// <returns></returns>
        public static string GetMovieContentsUploadAvailableInfoList(DateTime startDate, DateTime endDate,string searchTitle)
        {
            StringBuilder str = new StringBuilder();
            str.Append("LCHI/DID/Request?MT=GetMovieContentsUploadAvailableList&PV=");
            str.Append(startDate.ToShortDateString());
            str.Append("|`|");
            str.Append(endDate.ToShortDateString());
            str.Append("|`|");
            str.Append(searchTitle);
            return str.ToString();
        }

        public static string GetMovieInfo()
        {
            return "LCHI/DID/Request?MT=GetMovieInfo";
        }

        public static string GetMovieShowingContent(string theater, string playdate, string itemID)
        {
            StringBuilder str = new StringBuilder();
            str.Append("LCHI/DID/Request?MT=GetMovieShowingContents&PV=");
            str.Append(theater);
            str.Append("|`|");
            str.Append(playdate);
            str.Append("|`|");
            str.Append(itemID);
            return str.ToString();
        }

        public static string GetMovieShowingInfoAutoList(string theater, int itemID, string playDate)
        {
            StringBuilder str = new StringBuilder();
            str.Append("LCHI/DID/Request?MT=GetMovieShowingInfoAutoList&PV=");
            str.Append(theater);
            str.Append("|`|");
            str.Append(playDate);
            str.Append("|`|");
            str.Append(itemID);
            return str.ToString();
        }

        public static string GetMovieShowingInfoList_GetMovieInfo(string contentsCode)
        {
            StringBuilder str = new StringBuilder();
            str.Append("LCHI/DID/Request?MT=GetMovieShowingInfoList&PV=");
            str.Append(contentsCode);
            return str.ToString();
        }
    }
}
