using LotteCinemaService.Model.Common.Raw;
using System;

namespace LotteCinemaService.Model.Common
{
    public class MovieTimeCellInfo : InfoRawBase
    {
        public string CinemaCode;
        public string PlayDate;
        public string MovieCode;
        public string ShowSeq;
        public string MovieKoreaName;
        public string ScreenDivCode;
        public string ScreenCode;
        public string ScreenName;
        public int ScreenNumber;
        public int ScreenFloor;
        public string ContentsCode;
        public string StartTime;
        public string EndTime;
        public string EventCode;
        public string TicketCode;
        public string FilmCode;
        public int SeatCount;
        public int LeftSeat;
        public string ID;
        public bool IsCheck;
        public string PosterFtpFilePath;
        public string PosterLocalFilePath;
        public int FourDTypeCode;
    }

    public class MappedMovieTimeCellInfo
    {
        public int cinemaCode { get; set; }
        public DateTime playDate { get; set; }
        public int EventCode{ get; set; }
        public string movieCode{ get; set; }
        public string movieKoreaName{ get; set; }
        public string ScreenDivCd { get; set; }
        public int screenCode{ get; set; }
        public string screenName { get; set; }
        public int screenFloor { get; set; }
        public int captionCode { get; set; }
        public int filmCode { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string contentsCode { get; set; }
        public string ticketCode { get; set; }
        public string permissionLevel { get; set; }
        public int showSeq { get; set; }
        public int seatCnt { get; set; }
        public int leftseat { get; set; }
        public string id { get; set; }
        public string isCheck { get; set; }
        public int FourDTypeCode {get; set;}    }
}