using AttributeRouting.Web.Http;
using LotteCinemaService.Database.Manager;
using LotteCinemaService.WebAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LotteCinemaService.WebAPI.Common.Controllers
{
    public class DigitalGalleryController : ApiController
    {
        private DigitalGalleryManager _dgManager;

        public DigitalGalleryController()
        {
            this._dgManager = new DigitalGalleryManager(Settings.SERVER_DID_CONNECTION_STRING);
        }

        #region DeleteMediaInfo
        [HttpDelete]
        [DELETE("DG/MediaInfo?{id}")]
        public void DeleteMediaInfo(string id)
        {
            if (!this._dgManager.DeleteMediaInfo(id))
            {
                ResponseException();
            }
        }
        #endregion

        #region ResponseException
        [NonAction]
        private void ResponseException()
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.NotModified);
            throw new HttpResponseException(response);
        }
        #endregion 
    }
}
