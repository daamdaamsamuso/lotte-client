using System;
using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common.Raw
{
    public class AdInfoRaw : InfoRawBase
    {
        /// <summary>
        /// 광고코드
        /// </summary>
       public string ID;

       /// <summary>
       /// 계약 코드
       /// </summary>
       public string ContractID;

       /// <summary>
       /// 극장코드
       /// </summary>
       public string Theater;

        /// <summary>
        /// 극장 이름
        /// </summary>
       public string TheaterName;

       /// <summary>
       /// 아이템 코드
       /// </summary>
       public string ItemID;

        /// <summary>
        /// 광고 타이틀
        /// </summary>
       public string Title;

       /// <summary>
       /// 광고 타입
       /// </summary>
       public ContentsType ContentsType;

        /// <summary>
        /// 광고 레이아웃 타입
        /// </summary>
       public LayoutType LayoutType;

        /// <summary>
        /// 사운드 재생 위치
        /// </summary>
       public int SoundPosition;

        /// <summary>
        /// 광고 시작 시간
        /// </summary>
       public DateTime BeginDate;

        /// <summary>
        /// 광고 종료 시간
        /// </summary>
       public DateTime EndDate;

        /// <summary>
        /// 광고 구좌
        /// </summary>
       public string Account;
    }
}
