using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteCinemaService.Model.Common.Raw
{
    public class MovieShowingInfoRaw : InfoRawBase
    {
        public int Seq;
        public int OrderNo;
        public string Theater;
        public string ContentsCode;
        public string Title;
        public string ItemID;
        public string RunningTime;
        public string WatchClass;
        public string OpenDate;
        public string Synopsis;
        public string Country;
        public string Genre;
        public string Casts;
        public string Direction;
    }

    public class MappedMovieShowingInfoRaw
    {
        public string ContentsCode { get; set; }
        public string Title { get; set; }
        public string ItemID { get; set; }
        public string RunningTime { get; set; }
        public string WatchClass { get; set; }
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
        public string Synopsis { get; set; }
        public string Country { get; set; }
        public string Genre { get; set; }
        public string Casts { get; set; }
        public string Direction { get; set; }
    }

    public class MappedMovieShowingInfoRaw_MovieInfo
    {
        public string RunningTime { get; set; }
        public string WatchClass { get; set; }

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


        public string Synopsis { get; set; }
        public string Country { get; set; }
        public string Genre { get; set; }
        public string Casts { get; set; }
        public string Direction { get; set; }
    }
}
