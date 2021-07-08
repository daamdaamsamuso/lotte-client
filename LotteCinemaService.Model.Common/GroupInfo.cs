using System.Collections.Generic;
using System.Xml.Serialization;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common
{
    public class GroupInfo
    {    
        /// <summary>
        /// 순서
        /// </summary>
        public int OrderNo;

        /// <summary>
        /// 코드
        /// </summary>
        public string ID;

        /// <summary>
        /// 타이틀
        /// </summary>
        public string Title;

        /// <summary>
        /// 파일 리스트
        /// </summary>
        public List<ContentsInfo> ContentsList;

        /// <summary>
        /// 레이아웃 타입
        /// </summary>
        public LayoutType LayoutType;

        /// <summary>
        /// 사운드 재생 위치
        /// </summary>
        public int SoundPosition;
    }
}