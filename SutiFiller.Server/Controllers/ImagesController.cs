using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SutiFiller.Data;
using SutiFiller.Server.Data;
using Microsoft.AspNetCore.Http.Extensions;

namespace SutiFiller.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private SutisContext _context;

        public ImagesController(SutisContext context)
        {
            _context = context;
        }

        [HttpGet("{sutiId}")]
        public IActionResult GetImages(Int32 sutiId)
        {
            return Ok(_context.Images.Where(image => image.SutiId == sutiId).Select(image => new ImageDTO { Id = image.Id, ImageSmall = image.ImageSmall }));
        }

        [HttpPost]
        public IActionResult PostImage([FromBody] ImageDTO image)
        {
            if (image == null || !_context.Sutis.Any(suti => image.SutiId == suti.Id))
                return NotFound();

            Image sutiImage = new Image
            {
                SutiId = image.SutiId,
                ImageSmall = image.ImageSmall,
                ImageLarge = image.ImageLarge
            };

            _context.Images.Add(sutiImage);

            try
            {
                _context.SaveChanges();
                var location = Url.Action("GetImage", new { id = image.Id });
                return Created(location, image.Id);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteImage(Int32 id)
        {
            Image image = _context.Images.FirstOrDefault(im => im.Id == id);

            if (image == null)
                return NotFound();

            try
            {
                _context.Images.Remove(image);
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
