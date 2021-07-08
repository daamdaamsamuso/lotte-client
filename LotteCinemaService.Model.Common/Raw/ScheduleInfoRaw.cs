using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common.Raw
{
    public class ScheduleInfoRaw : InfoRawBase
    {
        public int Seq;
        public string ID;
        public string ContentsType;
        public string ItemID;
        public int OrderNo;
        public string Theater;
        public string Title;
        public bool IsFull;
    }
}