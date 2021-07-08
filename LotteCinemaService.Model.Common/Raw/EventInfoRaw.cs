using System;

namespace LotteCinemaService.Model.Common.Raw
{
    public class EventInfoRaw : InfoRawBase
    {
        public string EventID;
        public string ItemID;
        public string Title;
        public string Theater;
        public char UseYN;
        public int LayoutType;
    }
}