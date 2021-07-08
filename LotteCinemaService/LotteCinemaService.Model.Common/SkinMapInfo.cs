using System.Collections.Generic;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common
{
    public class SkinMapInfo
    { 
        /// <summary>
        /// 스킨 코드
        /// </summary>
        public string SkinID;

        /// <summary>
        /// 미디어/광고 코드
        /// </summary>
        public string GroupID;

        /// <summary>
        /// 컨텐츠 코드
        /// </summary>
        public string ContentsID;

        /// <summary>
        /// 미디어/광고 구분
        /// </summary>
        public ContentsType ContentsType;

        /// <summary>
        /// 파일 리스트
        /// </summary>
        public List<ContentsInfo> ContentsList;
    }
}