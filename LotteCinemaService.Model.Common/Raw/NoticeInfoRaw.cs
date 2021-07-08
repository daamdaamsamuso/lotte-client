
namespace LotteCinemaService.Model.Common.Raw
{
    /// <summary>
    /// 한줄 공지
    /// </summary>
    public class NoticeInfoRaw : InfoRawBase
    {
        /// <summary>
        /// 시퀀스
        /// </summary>
        public string NoticeID;

        /// <summary>
        /// 아이템 코드
        /// </summary>
        public string ItemID;

        /// <summary>
        /// 관 코드
        /// </summary>
        public string Theater;

        /// <summary>
        /// 알림 내용
        /// </summary>
        public string Text;

        /// <summary>
        /// 노출 여부
        /// </summary>
        public char IsVisible;
    }
}