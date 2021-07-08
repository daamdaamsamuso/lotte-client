using LotteCinemaService.Database.Manager;
using LotteCinemaService.WebAPI.Common.Models;
using System.Web.Http;

namespace LotteCinemaService.WebAPI.Common.Controllers
{
    public class WelcomeFacadeController : ApiController
    {
        private WelcomeFacadeManager _wfManager;

        public WelcomeFacadeController()
        {
            this._wfManager = new WelcomeFacadeManager(Settings.SERVER_DID_CONNECTION_STRING);
        }
    }
}