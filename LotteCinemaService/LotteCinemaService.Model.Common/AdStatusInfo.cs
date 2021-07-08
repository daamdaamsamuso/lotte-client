using LotteCinemaService.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteCinemaService.Model.Common
{
    public class AdStatusInfo
    {
      public string ADCode;
      public string Title;
      public string Theater;
      public string TheaterName;
      public string Advertiser;
      public string AdvertiserID;
      public string Period;
      public DateTime StartDate;
      public DateTime EndDate;
      public string Status;
      public ContentsType ADType;
    }
}
