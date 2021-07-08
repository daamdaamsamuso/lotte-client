

namespace LotteCinemaService.WebAPI.Helper
{
    public class WebApiNSQuery
    {
        public static string GetMovieShowingInfoList(string theater, int itemID)
        {
            return string.Format("NS/MovieShowingInfoList?theater={0}&itemID={1}", theater, itemID);
        }

        public static string GetMovieShowingInfoAutoList(string theater, int itemID, string playDate)
        {
            return string.Format("NS/MovieShowingInfoAutoList?theater={0}&itemID={1}&playDate={2}", theater, itemID, playDate);
        }

        public static string GetMovistInfoList(string contentsCode)
        {
            return string.Format("NS/MovistInfoList?contentsCode={0}", contentsCode);
        }
    }
}
