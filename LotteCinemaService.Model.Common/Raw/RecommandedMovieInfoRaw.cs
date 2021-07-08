using System;

namespace LotteCinemaService.Model.Common.Raw
{
    public class RecommandedMovieInfoRaw : InfoRawBase
    {
        public int Seq;
        public int OrderNo;
        public string Theater;
        public string MovieCode;
        public string Title;
        public string ItemID;
    }
}