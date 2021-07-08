using AttributeRouting.Web.Http;
using LotteCinemaLibraries.Config;
using LotteCinemaService.Database.Manager;
using LotteCinemaService.Model.Common;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.Model.Enum;
using LotteCinemaService.WebAPI.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace LotteCinemaService.WebAPI.Common.Controllers
{
    public class ISController : ApiController
    {
        private ISManager _isManager;

        public ISController()
        {
            this._isManager = new ISManager(Settings.SERVER_DID_CONNECTION_STRING);
        }

        [GET("IS/SpecialImageInfoList?{theater}&{ItemID}")]
        public List<ISSpecialImageInfo> GetSpecialImageInfoList(string theater, string itemID)
        {
            List<ISSpecialImageInfo> list = new List<ISSpecialImageInfo>();

            var result = this._isManager.GetISSpecialImageInfoList(theater, itemID);

            var groups = (from g in result
                          group g by new { g.ISID, g.BeginDate, g.EndDate }).ToList();

            foreach (var group in groups) 
            {
                ISSpecialImageInfo info = new ISSpecialImageInfo
                {
                    ISID = group.Key.ISID,
                    BeginDate = group.Key.BeginDate,
                    EndDate = group.Key.EndDate,
                    ContentsList = new List<ContentsInfo>()
                };

                foreach (var item in group)
                {
                    ContentsInfo contentsInfo = new ContentsInfo
                    {
                        ContentsID = item.ContentsID,
                        FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                        FileType = item.FileType,
                        ContentsType = item.ContentsType
                    };

                    var configFile = ConfigHelper.GetFilePath(group.Key.ISID, contentsInfo.FileName, ItemID.ISAll, ContentsType.Image);

                    contentsInfo.FtpFilePath = configFile.FtpFilePath;
                    contentsInfo.LocalFilePath = configFile.LocalFilePath;

                    info.ContentsList.Add(contentsInfo);
                }

                list.Add(info);
            }

            return list;
        }

        [GET("IS/SpecialImageInfo?{theater}&{ItemID}&{begin}&{end}")]
        public List<ISSpecialImageInfoRaw> GetSpecialImageInfo(string theater, string itemID,string begin,string end)
        {
           return this._isManager.GetISSpecialImageInfo(theater, itemID,begin,end);
        }

        #region DeleteNoticeInfo
        [HttpDelete]
        [DELETE("IS/SpecialImageInfo?{isID}")]
        public void DeleteSpecialImageInfo(string isID)
        {
            if (!this._isManager.DeleteSpecialImageInfo(isID))
            {
                ResponseException();
            }
        }
        #endregion

        [GET("IS/ISMenuInfo?{theater}&{ItemID}")]
        public ISMenuInfo GetISMenuInfo(string theater, string itemID)
        {
            ISMenuInfo info = new ISMenuInfo();

            var result = this._isManager.GetISMenuInfoList(theater, itemID);

            if (result.Count > 0)
            {
                var item = result[0];
                info.ISID = item.ISID;
                info.HomeMainType = item.HomeMainType;
                info.FloorPageVisible = item.FloorPageVisible;
                info.NotciePageVisible = item.NoticePageVisible;
                info.IdleInterval = item.IdleInterval;
                info.NoticeExposureTime = item.NoticeExposureTime;
            }

            return info;
        }

        [GET("IS/ISMenuInfoList?{theater}")]
        public List<ISMenuInfoRaw> GetISMenuInfoList(string theater)
        {
            ISMenuInfoRaw info = new ISMenuInfoRaw();

            var result = this._isManager.GetISMenuInfoList(theater);
            return result;
        }

        [HttpPut]
        [PUT("IS/ISSpecialImage?{item}")]
        public void SetISSpecialImage(ISSpecialImageInfoRaw item)
        {
            if (!this._isManager.InsertISSpecialImage(item))
            {
                ResponseException();
            }
        }

        [HttpPut]
        [PUT("IS/ISMenuInfoList?{items}")]
        public void SetISMenuInfoList(List<ISMenuInfoRaw> items)
        {
            if (!this._isManager.InsertMenuInfoList(items))
            {
                ResponseException();
            }
        }

        

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