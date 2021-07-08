using AttributeRouting.Web.Http;
using LotteCinemaService.Database.Manager;
using LotteCinemaService.Model.Common.Procedure;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.WebAPI.Common.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace LotteCinemaService.WebAPI.Common.Controllers
{
    public class DigitalWindowController : ApiController
    {
        private DigitalWindowManager _dwManager;

        public DigitalWindowController()
        {
            _dwManager = new DigitalWindowManager(Settings.SERVER_DID_CONNECTION_STRING);
        }

        [GET("DW/ContractInfoList?{itemCode}&{AdvertiserName}&{ContractID}&{ContractName}")]
        public List<ContractInfoProcedure> GetContractInfoList(string itemCode, string AdvertiserName, string ContractID, string ContractName)
        {
            return this._dwManager.GetContractList(itemCode,AdvertiserName,ContractID,ContractName);
        }

        [HttpPut]
        [PUT("DW/SetContentsInfo?{contents}")]
        public void SetContentInfo(ContentsInfoRaw content)
        {
            if (!this._dwManager.InsertContentsInfo(content))
            {
                ResponseException();
            }
        }

        [HttpPut]
        [PUT("DW/SetAdverInfo?{ad}")]
        public void SetAdverInfo(AdInfoRaw ad)
        {
            if (!this._dwManager.InsertADInfo(ad))
            {
                ResponseException();
            }
        }

        [HttpPut]
        [PUT("DW/SetScheduleInfo?{schedule}")]
        public void SetScheduleInfo(ScheduleInfoRaw schedule)
        {
            if (!this._dwManager.SetScheduleInfo(schedule))
            {
                ResponseException();
            }
        }

    
        private void ResponseException()
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.NotModified);
            throw new HttpResponseException(response);
        }
    }
}