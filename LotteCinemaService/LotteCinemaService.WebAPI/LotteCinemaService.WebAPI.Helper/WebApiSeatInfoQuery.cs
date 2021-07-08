
namespace LotteCinemaService.WebAPI.Helper
{
    public class WebApiSeatInfoQuery
    {
        public static string GetScreenSeatInfoRawList(string cinemaCode, string screenCode)
        {
            return string.Format("SeatInformation/ScreenSeatInfoRawList?cinemaCode={0}&screenCode={1}", cinemaCode, screenCode);
        }

        public static string GetScreenDspInfoRawList(string cinemaCode, string screenCode)
        {
            return string.Format("SeatInformation/ScreenDspInfoRawList?cinemaCode={0}&screenCode={1}", cinemaCode, screenCode);
        }

        public static string GetTicketInfoRawList(string cinemaCode, string playDate, int ticketNo)
        {
            return string.Format("SeatInformation/TicketInfoRawList?cinemaCode={0}&playDate={1}&ticketNo={2}", cinemaCode, playDate, ticketNo);
        }

        public static string GetTicketInfoRawList(string cinemaCode, string barcode)
        {
            return string.Format("SeatInformation/TicketInfoRawList?cinemaCode={0}&ticketNo={1}", cinemaCode, barcode);
        }

        public static string GetMobileTicketInfoRawList(string cinemaCode, string bookingNo)
        {
            return string.Format("SeatInformation/MobileTicketInfoRawList?cinemaCode={0}&bookingNo={1}", cinemaCode, bookingNo);
        }
    }
}
