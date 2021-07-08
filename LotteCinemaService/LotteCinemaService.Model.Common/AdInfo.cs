using System;

namespace LotteCinemaService.Model.Common
{
    public class AdInfo : GroupInfo
    {
        /// <summary>
        /// 구좌
        /// </summary>
        public int Account;

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