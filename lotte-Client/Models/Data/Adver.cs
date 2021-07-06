using LotteCinemaService.Model.Common;
using LotteCinemaService.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotte_Client.Models.Data
{
    public class Adver : ISchedule
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
        /// 파일명.확장자
        /// </summary>
        public List<ContentsInfo> Contents;

        public List<ContentsInfo> Skins;

        /// <summary>
        /// 레이아웃 타입
        /// </summary>
        public LayoutType LayoutType;

        /// <summary>
        /// 사운드 재생 위치
        /// </summary>
        public int SoundPosition;
    }

    public interface ISchedule
    {
    }
}
