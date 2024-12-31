using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SutiFiller.Web.Models;

namespace SutiFiller.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IAccountService _accountService;
        protected readonly ISutiService _sutiService;

        public BaseController(IAccountService accountService, ISutiService sutiService)
        {
            _accountService = accountService;
            _sutiService = sutiService;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            ViewBag.Categories = _sutiService.Categories.ToArray();
            ViewBag.UserCount = _accountService.UserCount;

            if (_accountService.CurrentUserName != null)
                ViewBag.CurrentGuestName = _accountService.GetGuest(_accountService.CurrentUserName)?.Name;
        }
    }
}
