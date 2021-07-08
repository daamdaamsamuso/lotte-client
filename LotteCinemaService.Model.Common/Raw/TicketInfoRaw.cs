
namespace LotteCinemaService.Model.Common.Raw
{
    public class TicketInfoRaw
    {
        public string ScreenCode;
        public string SeatGroup;
        public string SeatNo;
    }

    public class MappedTicketInfoRaw
    {
        public string screenCode { get; set; }
        public string seatGroup { get; set; }
        public string seatNo { get; set; }
    }
}
