
namespace LotteCinemaService.Model.Common.Raw
{
    public class ScreenSeatInfoRaw
    {
        public string ScreenCode;
        public string SeatGroup;
        public int SeatNo;
        public int SeatLine;
        public int SeatPosX;
        public int SeatPosY;
        public int SeatLenX;
        public int SeatLenY;
    }

    public class MappedScreenSeatInfoRaw
    {
        public string screenCode { get; set; }
        public string seatGroup { get; set; }
        public int seatNo { get; set; }
        public int seatLine { get; set; }
        public int seatPosX { get; set; }
        public int seatPosY { get; set; }
        public int seatLenX { get; set; }
        public int seatLenY { get; set; }
    }
}
