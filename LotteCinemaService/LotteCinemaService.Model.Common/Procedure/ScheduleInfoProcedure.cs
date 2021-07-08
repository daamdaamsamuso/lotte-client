using System;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common.Procedure
{
    public class ScheduleInfoProcedure
    {
        public int Seq;
        public string ID;
        public ContentsType ContentsType;
        public string ItemID;
        public int OrderNo;
        public string Theater;
        public DateTime RegDate;
        public string RegID;
        public DateTime UpdateDate;
        public string UpdateRegID;
    }
}