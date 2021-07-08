using LotteCinemaLibraries.Database;
using LotteCinemaService.Database.Helper;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.WebAPI.Helper;
using LotteCinemaService.WebAPI.Helper.LCSM;
using System;
using System.Collections.Generic;
using System.Data;

namespace LotteCinemaService.Database.Manager
{
    public class SeatInformationManager : DatabaseManager
    {
        public SeatInformationManager(string server)
            : base(server)
        {
        }

        #region GetScreenSeatInfoRawList
        public List<ScreenSeatInfoRaw> GetScreenSeatInfoRawList(string cinemaCode, string screenCode)
        {
            var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetScreenSeatInfoRawList(cinemaCode, screenCode), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            var items = datasets.Tables[0].DataTableToList<MappedScreenSeatInfoRaw>();
            List<ScreenSeatInfoRaw> returnValue = new List<ScreenSeatInfoRaw>();

            foreach(var item in items)
            {
                returnValue.Add(new ScreenSeatInfoRaw()
                {
                  ScreenCode = item.screenCode,
                  SeatGroup = item.seatGroup,
                  SeatLenX = item.seatLenX,
                  SeatLenY = item.seatLenY,
                  SeatLine = item.seatLine,
                  SeatNo = item.seatNo,
                  SeatPosX = item.seatPosX,
                  SeatPosY = item.seatPosY
                });
            }
            return returnValue;
        } 
        #endregion

        #region GetScreenDspInfoRawList
        public List<ScreenDspInfoRaw> GetScreenDspInfoRawList(string cinemaCode, string screenCode)
        {
            var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetScreenDspInfoRawList(cinemaCode, screenCode), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            var items = datasets.Tables[0].DataTableToList<MappedScreenDspInfoRaw>();
            List<ScreenDspInfoRaw> returnValue = new List<ScreenDspInfoRaw>();

            foreach (var item in items)
            {
                returnValue.Add(new ScreenDspInfoRaw()
                {
                    DspCaption = item.dspCaption,
                    DspHeight = item.dspHight,
                    DspLeft = item.dsLeft,
                    DspSeq = 0,
                    DspTop = item.dsTop,
                    DspType = item.DspType,
                    DspWidth = item.dspWidth,
                    ScreenCode = item.ScreenCode
                });
            }
            return returnValue;
        } 
        #endregion

        #region GetTicketInfoRawList
        public List<TicketInfoRaw> GetTicketInfoRawList(string cinemaCode, string playDate, string ticketNo)
        {
            var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetTicketInfoRawList(cinemaCode, playDate, ticketNo), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            var items = datasets.Tables[0].DataTableToList<MappedTicketInfoRaw>();
            List<TicketInfoRaw> returnValue = new List<TicketInfoRaw>();

            foreach (var item in items)
            {
                returnValue.Add(new TicketInfoRaw()
                {
                    ScreenCode = item.screenCode,
                    SeatGroup = item.seatGroup,
                    SeatNo = item.seatNo
                });
            }

            return returnValue;
        } 
        #endregion

        #region GetTicketInfoRawList
        public List<TicketInfoRaw> GetTicketInfoRawList(string cinemaCode, string ticketNo)
        {
            var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetTicketInfoRawList(cinemaCode, ticketNo), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            var items = datasets.Tables[0].DataTableToList<MappedTicketInfoRaw>();
            List<TicketInfoRaw> returnValue = new List<TicketInfoRaw>();

            foreach (var item in items)
            {
                returnValue.Add(new TicketInfoRaw()
                {
                    ScreenCode = item.screenCode,
                    SeatGroup = item.seatGroup,
                    SeatNo = item.seatNo
                });
            }

            return returnValue;
        }
        #endregion

        #region GetMobileTicketInfoRawList
        public List<TicketInfoRaw> GetMobileTicketInfoRawList(string cinemaCode, string bookingNo)
        {
            var datasets = WebApiHelper.GetResultDataSet(WebApiLCSMQuery.GetMobileTicketInfoRawList(cinemaCode, bookingNo), new Uri(LCSettings.SERVICE_LCSM, UriKind.RelativeOrAbsolute));
            var items = datasets.Tables[0].DataTableToList<MappedTicketInfoRaw>();
            List<TicketInfoRaw> returnValue = new List<TicketInfoRaw>();

            foreach (var item in items)
            {
                returnValue.Add(new TicketInfoRaw()
                    {
                        ScreenCode = item.screenCode,
                        SeatGroup = item.seatGroup,
                        SeatNo = item.seatNo
                    });
            }

            return returnValue;
        } 
        #endregion

        #region LCSM - 리뉴얼



        #endregion
    }
}
