using AttributeRouting.Web.Http;
using LotteCinemaService.Database.Manager;
using LotteCinemaService.Model.Common.Raw;
using LotteCinemaService.WebAPI.Common.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace LotteCinemaService.WebAPI.Common.Controllers
{
    public class NowShowingController : ApiController
    {
        private NowShowingManager _nsManager;

        public NowShowingController() 
        {
            this._nsManager = new NowShowingManager(Settings.SERVER_DID_CONNECTION_STRING);
        }

        // 영화 순서와 정보
        [GET("NS/MovieShowingInfoList?{theater}&{itemID}")]
        public List<MovieShowingInfoRaw> GetMovieShowingInfoList(string theater, string itemID)
        {
            var movieShowingInfoList = this._nsManager.GetMovieShowingInfoList(theater, itemID);

            return movieShowingInfoList;
        }

        // 영화 순서와 정보
        [GET("NS/MovieShowingInfoAutoList?{theater}&{itemID}&{playDate}")]
        public List<MovieShowingInfoRaw> GetMovieShowingInfoList(string theater, string itemID, string playDate)
        {
            var movieShowingInfoList = this._nsManager.GetMovieShowingInfoAutoList(theater, itemID, playDate);

            return movieShowingInfoList;
        }

        // 영화 배우 목록
        [GET("NS/MovistInfoList?{contentsCode}")]
        public List<MovistInfoRaw> GetMovistInfoList(string contentsCode)
        {
            var movistInfoList = this._nsManager.GetMovistInfoList(contentsCode);

            return movistInfoList;
        }
    }
}