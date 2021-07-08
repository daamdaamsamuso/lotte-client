using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common.Raw
{
    public class ISMenuInfoRaw : InfoRawBase
    {
        public string ISID;
        public string Theater;
        public string ItemID;
        public string ISName;
        public string HomeMainType;
        public bool FloorPageVisible;
        public bool NoticePageVisible;
        public int IdleInterval;
        public int NoticeExposureTime;
        public string Location;
    }
}