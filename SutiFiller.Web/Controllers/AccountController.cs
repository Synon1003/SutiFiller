using Microsoft.AspNetCore.Mvc;
using SutiFiller.Web.Models;

namespace SutiFiller.Web.Controllers
{
    public class AccountController : BaseController
    {
		public AccountController(IAccountService accountService, ISutiService sutiService)
            : base(accountService, sutiService)
        { }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
                return View("Login", user);

            if (!_accountService.Login(user))
            {
                ModelState.AddModelError("", "Hibás felhasználónév, vagy jelszó.");
                return View("Login", user);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegistrationViewModel guest)
        {
            if (!ModelState.IsValid)
                return View("Register", guest);

            if (!_accountService.Register(guest))
            {
                ModelState.AddModelError("UserName", "A megadott felhasználónév már létezik.");
                return View("Register", guest);
            }

            ViewBag.Information = "A regisztráció sikeres volt. Kérjük, jelentkezzen be.";

            if (HttpContext.Session.GetString("user") != null)
                HttpContext.Session.Remove("user");

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            _accountService.Logout();

            return RedirectToAction("Index", "Home");
        }
    }
}
