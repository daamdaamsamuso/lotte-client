
using System;
namespace LotteCinemaService.Model.Common
{
    public class MovieContentsUploadAvailableInfo
    {
        public string TitleKor;
        public string MovieInfoCode;
        public bool IsPoster1;
        public bool IsPoster2;
        public bool IsVideo;
        public DateTime OpenDate;
    }

    public class MappedMovieContentsUploadAvailableInfo
    {
        public string TitleKor { get; set; }
        public string MovieInfoCode { get; set; }
        public bool IsPoster1 { get; set; }
        public bool IsPoster2 { get; set; }
        public bool IsVideo { get; set; }
        public DateTime _OpenDate;

        public DateTime OpenDate
        {
            get
            {
                return _OpenDate;
            }
            set
            {
                this._OpenDate = value.AddHours(9);
            }

        }
    }
}