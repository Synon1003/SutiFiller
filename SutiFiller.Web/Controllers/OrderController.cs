using Microsoft.AspNetCore.Mvc;
using SutiFiller.Web.Models;

namespace SutiFiller.Web.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(IAccountService accountService, ISutiService sutiService)
            : base(accountService, sutiService)
        { }

        [HttpGet]
        public IActionResult Index()
        {
            var guest = _sutiService.GetGuest(HttpContext.Session.GetString("user"));

            if (guest == null)
                return RedirectToAction("Index", "Home");

            var order = _sutiService.GetOrderByGuestId(guest.Id);

            if (order == null)
                return RedirectToAction("Index", "Home");

            //order.SutiOrders = _sutiService.GetSutiOrdersByOrderId(order.Id);
            OrderViewModel? orderViewModel = _sutiService.NewOrder(order.Id);

            return View(orderViewModel);
        }

        [HttpGet]
        public IActionResult Show(Int32 orderId)
        {
            OrderViewModel? orderViewModel = _sutiService.NewOrder(orderId);

            if (orderViewModel == null || orderViewModel.Order.SutiOrders.Count() == 0)
                return RedirectToAction("Index", "Home");

            String? userName = HttpContext.Session.GetString("user");
            if (userName != null)
            {
                Guest? guest = _accountService.GetGuest(userName);

                if (guest != null)
                {
                    orderViewModel.GuestAddress = guest.Address;
                    orderViewModel.GuestName = guest.Name;
                    orderViewModel.GuestPhoneNumber = guest.PhoneNumber;
                }
            }

            return View("Show", orderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Int32? orderId, OrderViewModel orderViewModel)
        {
            if (orderId == null || orderViewModel == null)
                return RedirectToAction("Index", "Home");

            orderViewModel.Order = _sutiService.GetOrderById(orderId);

            if (orderViewModel.Order == null)
                return RedirectToAction("Index", "Home");


            if (!ModelState.IsValid)
                return View("Index", orderViewModel);

            String? userName = HttpContext.Session.GetString("user");
            if (userName == null)
            {
                if (!_accountService.Create(orderViewModel, out userName))
                {
                    ModelState.AddModelError("", "A rendelés rögzítése sikertelen, kérlek próbáld újra!");
                    return View("Index", orderViewModel);
                }
            }

            if (!_sutiService.SaveOrder(userName, orderViewModel))
            {
                ModelState.AddModelError("", "A rendelés rögzítése sikertelen, kérlek próbáld újra!");
                return View("Index", orderViewModel);
            }

            orderViewModel.Order.TotalPrice = _sutiService.GetPrice(orderViewModel);

            ViewBag.Message = "A rendelésed sikeresen rögzítésre került!";
            return View("Result", orderViewModel);
        }

        public FileResult BasketImage()
        {
            return File("~/images/PinkBasket.png", "image/png");
        }

        public ActionResult Add(Int32 sutiId)
        {
            var guest = _sutiService.GetGuest(HttpContext.Session.GetString("user"));
            if (guest == null)
                return RedirectToAction("Index", "Home");

            var order = _sutiService.GetOrderByGuestId(guest.Id);
            if (order == null)
                return RedirectToAction("Index", "Home");

            _sutiService.AddSutiToSutiOrders(order, sutiId);

            return RedirectToAction("Index");
        }
    }
}
