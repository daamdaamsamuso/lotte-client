
namespace LotteCinemaService.WebAPI.Helper
{
    public class WebApiDSQuery
    {
        public static string GetSkinInfoList(string theater)
        {
            return string.Format("DS/SkinInfoList?theater={0}", theater);
        }

        public static string GetNoticeInfoList(string theater)
        {
            return string.Format("DS/NoticeInfoList?theater={0}", theater);
        }

        public static string UpdateNoticeSkinInfo(string id)
        {
            return string.Format("DS/NoticeSkinInfo?id={0}", id);
        }
    }
}