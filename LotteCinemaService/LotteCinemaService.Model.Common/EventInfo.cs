using System.Collections.Generic;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common
{
    public class EventInfo
    {
        /// <summary>
        /// 이벤트 코드
        /// </summary>
        public string EventID;

        /// <summary>
        /// 이벤트 명
        /// </summary>
        public string Title;

        /// <summary>
        /// 레이아웃 타입
        /// </summary>
        public LayoutType LayoutType;

        /// <summary>
        /// 사운드 재생 위치
        /// </summary>
        public int SoundPosition;

        /// <summary>
        /// 컨텐츠
        /// </summary>
        public List<ContentsInfo> ContentsList;
    }
}