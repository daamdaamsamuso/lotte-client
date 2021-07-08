using LotteCinemaService.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteCinemaService.Model.Common.Raw
{
    public class ESEventLogInfo
    {
        public string UserID;
        public string WorkName;
        public EventLogLocationType Location;
        public string LocationName;
        public DateTime RegDateTime;
        public string RegDateTimeToText;
    }
}
