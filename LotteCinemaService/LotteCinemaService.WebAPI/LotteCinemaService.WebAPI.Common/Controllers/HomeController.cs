using System.Web.Mvc;

namespace LotteCinemaService.WebAPI.Common.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}