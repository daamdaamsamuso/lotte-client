using LotteCinemaService.Model.Enum;

namespace LotteCinemaService.Model.Common.Raw
{
    public class SkinMapInfoRaw : InfoRawBase
    {
        public int Seq;

        /// <summary>
        /// 스킨 코드
        /// </summary>
        public string SkinID;

        /// <summary>
        /// 미디어/광고 코드
        /// </summary>
        public string GroupID;

        /// <summary>
        /// 아이템 코드
        /// </summary>
        public string ItemID;

        /// <summary>
        /// 미디어/광고 구분
        /// </summary>
        public ContentsType ContentsType;
    }
}