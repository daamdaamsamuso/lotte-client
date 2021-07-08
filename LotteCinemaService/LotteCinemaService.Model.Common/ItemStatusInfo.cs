using System;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common
{
    public class ItemStatusInfo
    {
        /// <summary>
        /// 영화관 코드
        /// </summary>
        public int CinemaCode;

        /// <summary>
        /// 매체 코드
        /// </summary>
        public int ScreenCode;

        /// <summary>
        /// IP Address
        /// </summary>
        public string IP;

        /// <summary>
        /// 컨텐츠 표출 시작 시간
        /// </summary>
        public DateTime BeginTime;

        /// <summary>
        /// 컨텐츠 고유 번호
        /// </summary>
        public int ContentsID;
    }
}