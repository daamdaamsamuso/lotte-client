using System;

namespace LotteCinemaService.Model.Common.Raw
{
    public class InfoRawBase
    {
        /// <summary>
        /// 등록 일
        /// </summary>
        public DateTime RegDate;

        /// <summary>
        /// 등록 아이디
        /// </summary>
        public string RegID;

        /// <summary>
        /// 갱신 일
        /// </summary>
        public DateTime UpdateDate;

        /// <summary>
        /// 갱신 아이디
        /// </summary>
        public string UpdateRegID;
    }
}