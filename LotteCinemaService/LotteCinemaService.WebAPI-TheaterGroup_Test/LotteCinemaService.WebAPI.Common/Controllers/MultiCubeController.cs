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
    public class MultiCubeController  : ApiController
    {
        private MultiCubeManager _manager;

        public MultiCubeController()
        {
            this._manager = new MultiCubeManager(Settings.SERVER_DID_CONNECTION_STRING);
        }

        [GET("MC/ContractList?{itemCode}&{AdvertiserName}&{ContractID}&{ContractName}")]
        public List<ContractInfoProcedure> GetContractInfoList(string itemCode, string AdvertiserName, string ContractID, string ContractName)
        {
            return this._manager.GetContractList(itemCode, AdvertiserName, ContractID, ContractName);
        }

        [HttpPut]
        [PUT("MC/SetContentsInfo?{contents}")]
        public void SetContentInfo(ContentsInfoRaw content)
        {
            if (!this._manager.InsertContentsInfo(content))
            {
                ResponseException();
            }
        }

        [HttpPut]
        [PUT("MC/SetAdverInfo?{ad}")]
        public void SetAdverInfo(AdInfoRaw ad)
        {
            if (!this._manager.InsertADInfo(ad))
            {
                ResponseException();
            }
        }

        [HttpPut]
        [PUT("MC/SetScheduleInfo?{schedule}")]
        public void SetScheduleInfo(ScheduleInfoRaw schedule)
        {
            if (!this._manager.SetScheduleInfo(schedule))
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