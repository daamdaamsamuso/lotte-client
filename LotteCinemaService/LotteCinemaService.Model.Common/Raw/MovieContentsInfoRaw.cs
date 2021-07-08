using System;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common.Raw
{
    public class MovieContentsInfoRaw
    {
        public int Seq;
        public string ContentsCode;
        public string MovieShortName;
        public string LargePosterFileName;
        public string SmallPosterFileName;
        public string MovieFileName;
        public DateTime RegDate;
        public string RegID;
    }
}