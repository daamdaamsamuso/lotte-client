
namespace LotteCinemaService.WebAPI.Helper
{
    public class WebApiDCQuery
    {
        public static string GetDigitalCurtainInfoList(int contentType)
        {
            return string.Format("DC/DigitalCurtainInfoList?contentType={0}", contentType);
        }
    }
}