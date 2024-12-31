using Microsoft.AspNetCore.Mvc;
using SutiFiller.Web.Models;

namespace SutiFiller.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IAccountService accountService, ISutiService sutiService)
            : base(accountService, sutiService)
        {}

		public IActionResult Index()
        {
            return View("Index", _sutiService.Sutis.ToList());
        }

        public IActionResult List(Int32 categoryId)
        {
            List<Suti> sutis = _sutiService.GetSutis(categoryId).ToList();

            if (sutis.Count == 0)
                return RedirectToAction(nameof(Index));

            return View("Index", sutis);
        }

        public IActionResult Details(Int32 sutiId)
        {
            Suti? suti = _sutiService.GetSuti(sutiId);

            if (suti == null)
                return RedirectToAction(nameof(Index));

            ViewBag.Title = "Süti részletek: " + suti.Name + " (" + suti.Category.Name + ")";
            ViewBag.Images = _sutiService.GetSutiImageIds(suti.Id).ToList();

            return View("Details", suti);
        }

        public FileResult ImageForSuti(Int32 sutiId)
        {
            Byte[]? imageContent = _sutiService.GetSutiMainImage(sutiId);

            if (imageContent == null)
                return File("~/images/NoImage.png", "image/png");

            return File(imageContent, "image/jpg");
        }

        public FileResult Image(Int32 imageId, Boolean large = false)
        {
            Byte[]? imageContent = _sutiService.GetSutiImage(imageId, large);

            if (imageContent == null)
                return File("~/images/NoImage.png", "image/png");

            return File(imageContent, "image/jpg");
        }

        public IActionResult SutiSearch(string searchString)
        {
            IEnumerable<Suti> sutis = from s in _sutiService.Sutis select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                sutis = sutis.Where(s => s.Name.Contains(searchString));
            }

            return View("Index", sutis.ToList());
        }
    }
}
