using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SutiFiller.Server.Data;
using SutiFiller.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SutiFiller.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SutisController : ControllerBase
    {
        private SutisContext _context;

        public SutisController(SutisContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetSutis()
        {
            try
            {
                return Ok(_context.Sutis.Include(s => s.Category).ToList().Select(suti => new SutiDTO
                {
                    Id = suti.Id,
                    Name = suti.Name,
                    Category = new CategoryDTO { Id = suti.Category.Id, Name = suti.Category.Name },
                    CategoryId = suti.CategoryId,
                    Price = suti.Price,
                    Description = suti.Description,
                }));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetSutiByName(String name)
        {
            try
            {
                return Ok(_context.Sutis.Where(s => s.Name == name).FirstOrDefault());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult CreateSuti([FromBody] SutiDTO sutiDTO)
        {
            try
            {
                var addedSuti = _context.Sutis.Add(new Suti
                {
                    Name = sutiDTO.Name,
                    CategoryId = sutiDTO.CategoryId,
                    Price = sutiDTO.Price,
                    Description = sutiDTO.Description
                });

                _context.SaveChanges();

                return Created($"/api/orders/{sutiDTO.Name}", new { id = addedSuti.Entity.Id });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult UpdateSuti([FromBody] SutiDTO sutiDTO)
        {
            try
            {
                Suti suti = _context.Sutis.FirstOrDefault(s => s.Id == sutiDTO.Id);

                if (suti == null)
                    return NotFound();

                suti.Name = sutiDTO.Name;
                suti.CategoryId = sutiDTO.CategoryId;
                suti.Price = sutiDTO.Price;
                suti.Description = sutiDTO.Description;

                _context.SaveChanges();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSuti(Int32 id)
        {
            try
            {
                Suti suti = _context.Sutis.FirstOrDefault(s => s.Id == id);

                if (suti == null)
                    return NotFound();

                _context.Sutis.Remove(suti);

                _context.SaveChanges();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
