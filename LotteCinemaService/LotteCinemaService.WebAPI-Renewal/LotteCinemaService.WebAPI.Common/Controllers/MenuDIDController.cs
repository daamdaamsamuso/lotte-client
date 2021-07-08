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
    public class MenuDIDController : ApiController
    {
        private MenuDIDManager _manager;
        public MenuDIDController()
        {
            this._manager = new MenuDIDManager(Settings.SERVER_DID_CONNECTION_STRING);
        }

        #region GET

        [GET("MB/ContentsInfo?{CinemaCode}")]
        public List<MenuDIDInfoRaw> GetConteneList(int CinemaCode, string MenuDIDItem)
        {
            return this._manager.GetContentList(CinemaCode,MenuDIDItem);
        }

        [GET("MB/GetItemByID?{ID}")]
        public List<MenuDIDInfoRaw> GetItemByID(string ID)
        {
            return this._manager.GetItemByID(ID);
        }

        [GET("MB/NewMenuInfo")]
        public List<NewMenuInfoRaw> GetNewMenuInfo()
        {
            return this._manager.GetNewMenuInfo();
        }

        [GET("MB/NewMenuScheduleInfo?{CinemaCode}")]
        public List<NewMenuScheduleInfoProcedure> GetNewMenuScheduleInfo(string CinemaCode)
        {
            return this._manager.GetNewMenuScheduleInfo(CinemaCode);
        }

        #endregion

        #region PUT

        [HttpPut]
        [PUT("MB/SetContentsInfo?{content}")]
        public void SetContentInfo(MenuDIDInfoRaw content)
        {
            if (!this._manager.InsertContentsInfo(content))
            {
                ResponseException();
            }
        }

        [HttpPut]
        [PUT("MB/UpdateContent?{content}")]
        public void UpdateContent(MenuDIDInfoRaw content)
        {
            if (!this._manager.UpdateContent(content))
            {
                ResponseException();
            }
        }

        [HttpPut]
        [PUT("MB/DeleteContentInfo?{ID}")]
        public void DeleteMediaInfo(string ID)
        {
            if (!this._manager.DeleteContentsInfo(ID))
            {
                ResponseException();
            }
        }

        [HttpPut]
        [PUT("MB/InsertNewMenuInfo?{content}")]
        public void InsertNewMenuInfo(NewMenuInfoRaw content)
        {
            if (!this._manager.InsertNewMenuInfo(content))
            {
                ResponseException();
            }
        }

        [HttpPut]
        [PUT("MB/UpdateNewMenuInfo?{content}")]
        public void UpdateNewMenuInfo(NewMenuInfoRaw content)
        {
            if (!this._manager.UpdateNewMenuInfo(content))
            {
                ResponseException();
            }
        }

        [HttpPut]
        [PUT("MB/InsertNewMenuScheduleInfo?{content}")]
        public void InsertNewMenuScheduleInfo(NewMenuScheduleInfoRaw content)
        {
            if (!this._manager.InsertNewMenuScheduleInfo(content))
            {
                ResponseException();
            }
        }

        [HttpPut]
        [PUT("MB/DeleteNewMenuInfo?{ID}")]
        public void DeleteNewMenuInfo(string ID)
        {
            if (!this._manager.DeleteNewMenuInfo(ID))
            {
                ResponseException();
            }
        }

        [HttpPut]
        [PUT("MB/DeleteNewMenuScheduleInfo?{CinemaCode}&{DidItem}")]
        public void DeleteNewMenuScheduleInfo(string CinemaCode,string DidItem)
        {
            if (!this._manager.DeleteNewMenuScheduleInfo(CinemaCode,DidItem))
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