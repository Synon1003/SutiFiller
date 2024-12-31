using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SutiFiller.Data;
using SutiFiller.Server.Data;

namespace SutiFiller.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private SutisContext _context;


        public CategoriesController(SutisContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                return Ok(_context.Categories.ToList().Select(category => new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                }));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetSutisForCategory(Int32 id)
        {
            try
            {
                return Ok(_context.Sutis.Where(s => s.CategoryId == id).ToList().Select(suti => new SutiDTO
                {
                    Id = suti.Id,
                    Name = suti.Name,
                    //Category = new CategoryDTO { Id = suti.Category.Id, Name = suti.Category.Name },
                    CategoryId = suti.CategoryId,
                    Price = suti.Price,
                    Description = suti.Description
                }));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
