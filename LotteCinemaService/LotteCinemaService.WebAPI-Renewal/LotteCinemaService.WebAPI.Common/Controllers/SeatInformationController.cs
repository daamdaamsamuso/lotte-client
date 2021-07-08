using AttributeRouting.Web.Http;
using LotteCinemaService.Database.Manager;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.WebAPI.Common.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace LotteCinemaService.WebAPI.Common.Controllers
{
    public class SeatInformationController : ApiController
    {
        private SeatInformationManager _siManager;

        public SeatInformationController()
        {
            this._siManager = new SeatInformationManager(Settings.SERVER_DID_CONNECTION_STRING);
        }

        [GET("SeatInformation/ScreenSeatInfoRawList?{cinemaCode}&{screenCode}")]
        public List<ScreenSeatInfoRaw> GetScreenSeatInfoRawList(string cinemaCode, string screenCode)
        {
            var screenSeatInfoList = this._siManager.GetScreenSeatInfoRawList(cinemaCode, screenCode);
            return screenSeatInfoList;
        }

        [GET("SeatInformation/ScreenDspInfoRawList?{cinemaCode}&{screenCode}")]
        public List<ScreenDspInfoRaw> GetScreenDspInfoRawList(string cinemaCode, string screenCode)
        {
            var screenDspInfoList = this._siManager.GetScreenDspInfoRawList(cinemaCode, screenCode);
            return screenDspInfoList;
        }

        [GET("SeatInformation/TicketInfoRawList?{cinemaCode}&{playDate}&{ticketNo}")]
        public List<TicketInfoRaw> GetTicketInfoRawList(string cinemaCode, string playDate, string ticketNo)
        {
            var ticketInfoList = this._siManager.GetTicketInfoRawList(cinemaCode, playDate, ticketNo);

            return ticketInfoList;
        }

        [GET("SeatInformation/TicketInfoRawList?{cinemaCode}&{ticketNo}")]
        public List<TicketInfoRaw> GetTicketInfoRawList(string cinemaCode, string ticketNo)
        {
            var ticketInfoList = this._siManager.GetTicketInfoRawList(cinemaCode, ticketNo);

            return ticketInfoList;
        }

        [GET("SeatInformation/MobileTicketInfoRawList?{cinemaCode}&{bookingNo}")]
        public List<TicketInfoRaw> GetMobileTicketInfoRawList(string cinemaCode, string bookingNo)
        {
            var ticketInfoList = this._siManager.GetMobileTicketInfoRawList(cinemaCode, bookingNo);

            return ticketInfoList;
        }
    }
}