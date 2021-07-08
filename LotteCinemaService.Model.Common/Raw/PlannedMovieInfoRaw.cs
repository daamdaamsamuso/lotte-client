
using System;

namespace LotteCinemaService.Model.Common.Raw
{
    public class PlannedMovieInfoRaw : InfoRawBase
    {
        public int Seq;
        public int OrderNo;
        public string Theater;
        public string MovieCode;
        public string ContentsCode;
        public string Title;
        public string ItemID;
        public bool LargePoster;
        public bool SmallPoster;
        public bool Video;
    }

    public class MappedPlannedMovieInfoRaw_MovieShowingContent
    {
        public string MovieCode { get; set; }
        public string ContentsCode { get; set; }
        public string MovieShortName { get; set; }
    }

    public class MappedPlannedMovieInfoRaw_MovieInfo
    {
        public string MovieCode { get; set; }
        public string ContentsCode { get; set; }
        public string Title { get; set; }
    }
}