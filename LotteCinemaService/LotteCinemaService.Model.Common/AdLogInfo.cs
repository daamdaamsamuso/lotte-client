using System;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common
{
    public class AdLogInfo
    {
        /// <summary>
        /// 영화관 코드
        /// </summary>
        public string CinemaCode;

        /// <summary>
        /// 매체코드
        /// </summary>
        public string ScreenCode;

        /// <summary>
        /// 컨텐츠 표출 시작 시간
        /// </summary>
        public DateTime BeginTime;

        /// <summary>
        /// 표출 컨텐츠 ID
        /// </summary>
        public string ContentsID;

        /// <summary>
        /// 컨텐츠 표출 종료시간
        /// </summary>
        public DateTime EndTime;

        /// <summary>
        /// 광고 코드
        /// </summary>
        public string AdID;
    }
}