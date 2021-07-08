using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteCinemaService.Model.Common.Raw
{
    public class ESEventInfomationRawInfo
    {
        public int Seq;
        public string TransNo;
        public string UserID;
        public string UserPhone;
        public string UserEmail;
        public string CinemaCode;
        public DateTime EventStartTime;
        public DateTime EventEndTime;
        public DateTime EventEnterTime;
        public string FTPFilePath;
        public bool isComplete;
        public bool isOpen;
        public bool isDisplayed;
        public bool isCompleteNull;
        public bool isOpenNull;
        public bool isDisplayedNull;
        public int BgmIndex;
        public DateTime EventDeleteTimer;
    }
}
