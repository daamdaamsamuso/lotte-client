using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotteCinemaService.Model.Common
{
    public class ADInfoContentsInfo : GroupInfo
    {
        /// <summary>
        /// 구좌
        /// </summary>
        public int Account;

        public string TheaterID;
        public string TheaterName;
        public string AdvertiserID;
        public string AdvertiserName;
        public string ContractID;
        public string ADAgentID;
        public string AgentName;

        /// <summary>
        /// 시작 시간
        /// </summary>
        public DateTime BeginDate;

        /// <summary>
        /// 종료 시간
        /// </summary>
        public DateTime EndDate;
    }
}
