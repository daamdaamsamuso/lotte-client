using System.Web.Mvc;

namespace LotteCinemaService.WebAPI.TBA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
