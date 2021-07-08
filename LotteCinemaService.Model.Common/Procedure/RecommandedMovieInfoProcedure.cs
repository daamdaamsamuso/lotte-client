using System;

namespace LotteCinemaService.Model.Common.Procedure
{
    public class RecommandedMovieInfoProcedure
    {
        public int OrderNo;
        public string MovieCode;
        public string Title;
        public int RunningTime;
        public string Country;
        public string Genre;
        public string Direction;
        public string Casts;
        public string WatchClass;
        public DateTime OpenDate;
    }

    public class MappedRecommandedMovieInfoProcedure
    {
        public string TitleKor { get; set; }
        public int PlayTime { get; set; }
        public string CommCdNm { get; set; }
        public string CommCdNm1 { get; set; }
        public string Direction { get; set; }
        public string CastsProducer { get; set; }
        public string ViwGrdCd { get; set; }
        public string RelsDt { get; set; }
    }
}