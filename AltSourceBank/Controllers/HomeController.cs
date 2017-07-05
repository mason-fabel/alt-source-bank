using System.Web.Mvc;

namespace AltSourceBank.Controllers {
    public class HomeController : Controller {
		[HttpGet]
        public ActionResult Index() {
            return View();
        }
    }
}