using System.Web;
using System.Web.Mvc;
using AltSourceBank.Models;

namespace AltSourceBank.Controllers {
	public class UserController : Controller {
		[HttpGet]
        public ActionResult Index() {
            return View();
        }

		[HttpPost]
		public ActionResult Create(string name, string email, string password) {
			if (Models.User.Create(name, email, password)) {
				return new EmptyResult();
			}

			return new HttpStatusCodeResult(409);
		}

		[HttpPost]
		public ActionResult Login(string email, string password) {
			User u;

			u = Models.User.Login(email, password);

			if (u != null) {
				return Json(u);
			}

			return new HttpStatusCodeResult(401);
		}

		[HttpPost]
		public ActionResult Deposit(string key, double amount) {
			Models.User.Deposit(key, amount);

			return new EmptyResult();
		}

		[HttpPost]
		public ActionResult Withdrawal(string key, double amount) {
			Models.User.Withdrawal(key, amount);

			return new EmptyResult();
		}

		[HttpPost]
		public ActionResult Balance(string key) {
			return Json(Models.User.Balance(key));
		}

		[HttpPost]
		public ActionResult History(string key) {
			return Json(Models.User.History(key));
		}

		[HttpPost]
		public ActionResult Logout(string key) {
			Models.User.Logout(key);

			return new EmptyResult();
		}
	}
}