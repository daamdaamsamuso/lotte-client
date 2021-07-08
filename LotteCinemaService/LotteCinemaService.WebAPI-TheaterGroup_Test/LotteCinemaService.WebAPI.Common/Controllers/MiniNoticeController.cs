using AttributeRouting.Web.Http;
using LotteCinemaService.Database.Manager;
using LotteCinemaService.Model.Common.Procedure;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.WebAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LotteCinemaService.WebAPI.Common.Controllers
{
    public class MiniNoticeController : ApiController
    {
        public MiniNoticeManager _manager;

        public MiniNoticeController()
        {
            this._manager = new MiniNoticeManager(Settings.SERVER_DID_CONNECTION_STRING);
        }

        #region GET


        [GET("MN/MiniNoticeInfo")]
        public List<MiniNoticeInfoRaw> GetMiniNoticeInfo()
        {
            return this._manager.GetMiniNoticeInfo();
        }

        [GET("MN/MiniNoticeScheduleInfo?{CinemaCode}")]
        public List<MiniNoticeScheduleInfoProcedure> GetMiniNoticeScheduleInfo(string CinemaCode)
        {
            return this._manager.GetMiniNoticeScheduleInfo(CinemaCode);
        }

        #endregion

        #region PUT

        [HttpPut]
        [PUT("MN/InsertMiniNoticeInfo?{content}")]
        public void InsertMiniNoticeInfo(MiniNoticeInfoRaw content)
        {
            if (!this._manager.InsertMiniNoticeInfo(content))
            {
                ResponseException();
            }
        }

        [HttpPut]
        [PUT("MN/InsertMiniNoticeScheduleInfo?{content}")]
        public void InsertMiniNoticeScheduleInfo(MiniNoticeScheduleInfoRaw content)
        {
            if (!this._manager.InsertMiniNoticeScheduleInfo(content))
            {
                ResponseException();
            }
        }


        [HttpPut]
        [PUT("MN/DeleteMiniNoticeInfo?{ID}")]
        public void DeleteMiniNoticeInfo(string ID)
        {
            if (!this._manager.DeleteMiniNoticeInfo(ID))
            {
                ResponseException();
            }
        }

        [HttpPut]
        [PUT("MN/DeleteMiniNoticeScheduleInfo?{CinemaCode}&{DidItem}")]
        public void DeleteMiniNoticeScheduleInfo(string CinemaCode, string DidItem)
        {
            if (!this._manager.DeleteMiniNoticeScheduleInfo(CinemaCode, DidItem))
            {
                ResponseException();
            }
        }

        #endregion

        private void ResponseException()
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.NotModified);
            throw new HttpResponseException(response);
        }


    }
}
