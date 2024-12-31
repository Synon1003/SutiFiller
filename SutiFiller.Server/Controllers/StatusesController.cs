using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SutiFiller.Data;
using SutiFiller.Server.Data;

namespace SutiFiller.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        private SutisContext _context;


        public StatusesController(SutisContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetStatuses()
        {
            try
            {
                return Ok(_context.Statuses.Where(s => s.Name != "Deleted").ToList().Select(status => new StatusDTO
                {
                    Id = status.Id,
                    Name = status.Name
                }));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
