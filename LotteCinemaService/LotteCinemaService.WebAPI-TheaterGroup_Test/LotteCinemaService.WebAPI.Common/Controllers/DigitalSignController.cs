using AttributeRouting.Web.Http;
using LotteCinemaLibraries.Config;
using LotteCinemaService.Database.Manager;
using LotteCinemaService.Model.Common;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.Model.Enum;
using LotteCinemaService.WebAPI.Common.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace LotteCinemaService.WebAPI.Common.Controllers
{
    public class DigitalSignController : ApiController
    {
        private DigitalSignManager _dsManager;

        public DigitalSignController()
        {
            this._dsManager = new DigitalSignManager(Settings.SERVER_DID_CONNECTION_STRING);
        }

        [GET("DS/SkinInfoList?{theater}")]
        public List<DigitalSignSkinInfo> GetSkinInfoList(string theater)
        {
            List<DigitalSignSkinInfo> list = new List<DigitalSignSkinInfo>();

            var result = this._dsManager.GetSkinInfoList(theater);

            foreach (var item in result)
            {
                DigitalSignSkinInfo skinInfo = new DigitalSignSkinInfo
                {
                    SkinID = item.SkinID,
                    FileName = string.Format("{0}.{1}", item.FileName, item.FileType),
                    ContentsType = item.ContentsType
                };

                var configFile = ConfigHelper.GetFilePath(skinInfo.SkinID, skinInfo.FileName, ItemID.DigitalSign, ContentsType.Skin);

                skinInfo.FtpFilePath = configFile.FtpFilePath;
                skinInfo.LocalFilePath = configFile.LocalFilePath;

                list.Add(skinInfo);
            }

            return list;
        }

        [GET("DS/NoticeInfoList?{theater}")]
        public List<DigitalSignNoticeInfo> GetNoticeInfoList(string theater)
        {
            var result = this._dsManager.GetNoticeInfoList(theater);

            foreach (var item in result)
            {
                if (!string.IsNullOrWhiteSpace(item.IconFileName))
                {
                    var configFile = ConfigHelper.GetFilePath("", item.IconFileName, ItemID.DigitalSign, ContentsType.Notice);

                    item.FtpIconFilePath = configFile.FtpFilePath;
                    item.LocalIconFilePath = configFile.LocalFilePath;
                }
            }

            return result;
        }

        [GET("DS/NoticeInfoItem?{noticeID}")]
        public DigitalSignInfoRaw GetNoticeInfoItem(string noticeID)
        {
            var result = this._dsManager.GetNoticeInfoItem(noticeID);
            return result;
        }

        #region DeleteNoticeInfo
        [HttpDelete]
        [DELETE("DS/NoticeInfo?{notice}")]
        public void DeleteNoticeInfo(string noticeID)
        {
            if (!this._dsManager.DeleteNoticeInfo(noticeID))
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