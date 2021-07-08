
namespace LotteCinemaService.Model.Common
{
    public class TMBItemStatusInfo
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
        public string BeginTime;

        /// <summary>
        /// 컨텐츠 고유 번호
        /// </summary>
        public int ContentsID;
    }
}