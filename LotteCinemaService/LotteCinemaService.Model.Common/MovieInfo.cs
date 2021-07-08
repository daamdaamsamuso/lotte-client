using System.IO;

namespace LotteCinemaService.Model.Common
{
    public class MovieInfo
    {
        public string MovieCode;
        public string ContentsCode;
        public string MovieName;
        public string PermissionLevel;
        public string ScreenDivCode;
        public string ScreenName;
        public string ScreenCode;
        public int ScreenNumber;
        public int ScreenFloor;
        public string FilmCode;
        public string CaptionCode;
        public string ID;
        public int WatchCount;
        public int BookingCount;
        public string PosterLocalFilePath;
        public string PosterFtpFilePath;
        public int FourDTypeCode;
    }

    public class MappedMovieInfo
    {
        public string MovieCode { get; set; }
        public string ContentsCode { get; set; }
        public string MovieName { get; set; }
        public string PermissionLevel { get; set; }
        public string ScreenName { get; set; }
        public string ScreenCode { get; set; }
        public int ScreenNumber { get; set; }
        public int ScreenFloor { get; set; }
        public string FilmCode { get; set; }
        public string CaptionCode { get; set; }
        public string ID { get; set; }
        public int WatchCount { get; set; }
        public int BookingCount { get; set; }
        public string PosterLocalFilePath { get; set; }
        public string PosterFtpFilePath { get; set; }
    }

    public class MappedMovieShowingInfo
    {
        public string movieCode { get; set; }
        public string contentsCode { get; set; }
        public string movieKoreaName { get; set; }
        public string ScreenDivCd { get; set; }
        public string screenName { get; set; }
        public string screenCode { get; set; }
        public int screenFloor { get; set; }
        public string permissionLevel { get; set; }
        public int captionCode { get; set; }
        public int filmCode { get; set; }
        public string id { get; set; }
        public string WatchCount { get; set; }
        public string BookingCount { get; set; }
        public int FourDTypeCode { get; set; }    
    }
}