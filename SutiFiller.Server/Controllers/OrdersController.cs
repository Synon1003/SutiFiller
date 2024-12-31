using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SutiFiller.Data;
using SutiFiller.Server.Data;

namespace SutiFiller.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private SutisContext _context;


        public OrdersController(SutisContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            try
            {
                return Ok(_context.Orders.Where(o => o.StatusId != 5).Include(s => s.Status).ToList().Select(order => new OrderDTO
                {
                    Id = order.Id,
                    PrePayment = order.PrePayment,
                    TotalPrice = order.TotalPrice,
                    Name = order.Name,
                    BillingAddress = order.BillingAddress,
                    PhoneNumber = order.PhoneNumber,
                    Status = new StatusDTO { Id = order.Status.Id, Name = order.Status.Name },
                    StatusId = order.StatusId,
                    DueDate = order.DueDate,
                    Comment = order.Comment,
                }));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(Int32 id)
        {
            try
            {
                var order = _context.Orders.FirstOrDefault(s => s.Id == id);
                if (order == null) return NotFound();

                return Ok(new OrderDTO 
                {
                    Id = order.Id,
                    PrePayment = order.PrePayment,
                    TotalPrice = order.TotalPrice,
                    Name = order.Name,
                    BillingAddress = order.BillingAddress,
                    PhoneNumber = order.PhoneNumber,
                    Status = new StatusDTO { Id = order.Status.Id, Name = order.Status.Name },
                    StatusId = order.StatusId,
                    DueDate = order.DueDate,
                    Comment = order.Comment,
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}/sutiorders")]
        public IActionResult GetSutiOrders(Int32 id)
        {
            Order order = _context.Orders.Where(o => o.Id == id).ToList().FirstOrDefault();

            var sutiorders = _context.SutiOrders.Include(so => so.Suti)
            .Where(sutiorder => sutiorder.OrderId == id).ToList();

            List<SutiOrderDTO> sutiOrders = new List<SutiOrderDTO>();
            foreach (SutiOrder so in sutiorders)
            {
                Suti suti = _context.Sutis.Include(s => s.Category).Where(s => s.Id == so.SutiId).FirstOrDefault();
                sutiOrders.Add(new SutiOrderDTO
                {
                    Id = so.Id,
                    Message = so.Message,
                    Quantity = so.Quantity,
                    AllInPrice = so.AllInPrice,
                    Suti = new SutiDTO { Id = suti.Id, Name = suti.Name, Price = suti.Price, 
                        Category = new CategoryDTO { Id = suti.CategoryId, Name = suti.Category.Name } },
                    SutiId = so.SutiId,
                    OrderId = so.OrderId,
                });
            }

            try
            {
                return Ok(sutiOrders);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderDTO orderDTO)
        {
            try
            {
                var addedOrder = _context.Orders.Add(new Order
                {
                    PrePayment = orderDTO.PrePayment,
                    TotalPrice = orderDTO.TotalPrice,
                    Name = orderDTO.Name,
                    BillingAddress = orderDTO.BillingAddress,
                    PhoneNumber = orderDTO.PhoneNumber,
                    StatusId = orderDTO.StatusId,
                    DueDate = orderDTO.DueDate,
                    Comment = orderDTO.Comment?? "" ,
                });


                _context.SaveChanges();

                orderDTO.Id = addedOrder.Entity.Id;

                foreach (var so in orderDTO.SutiOrders)
                {
                    _context.SutiOrders.Add(new SutiOrder
                    {
                        OrderId = orderDTO.Id,
                        SutiId = so.SutiId,
                        Quantity = so.Quantity,
                        AllInPrice = so.AllInPrice,
                        Message = so.Message?? "",
                    });
                }

                _context.SaveChanges();

                return Created($"/api/orders/{orderDTO.Id}", new { id = addedOrder.Entity.Id });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult UpdateOrder([FromBody] OrderDTO orderDTO)
        {
            try
            {
                Order order = _context.Orders.FirstOrDefault(o => o.Id == orderDTO.Id && o.Name == orderDTO.Name);

                if (order == null)
                    return NotFound();

                order.PrePayment = orderDTO.PrePayment;
                order.TotalPrice = orderDTO.TotalPrice;
                order.Name = orderDTO.Name;
                order.BillingAddress = orderDTO.BillingAddress;
                order.PhoneNumber = orderDTO.PhoneNumber;
                order.StatusId = orderDTO.StatusId;
                order.DueDate = orderDTO.DueDate;
                order.Comment = orderDTO.Comment;

                var sutiOrdersToRemove = _context.SutiOrders.Where(so => so.OrderId == orderDTO.Id);
                _context.SutiOrders.RemoveRange(sutiOrdersToRemove);

                foreach (SutiOrderDTO sutiOrder in orderDTO.SutiOrders)
                {
                    _context.SutiOrders.Add(new SutiOrder
                    {
                        OrderId = sutiOrder.OrderId,
                        SutiId = sutiOrder.SutiId,
                        Quantity = sutiOrder.Quantity,
                        AllInPrice = sutiOrder.AllInPrice,
                        Message = sutiOrder.Message,
                    });
                }

                _context.SaveChanges();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(Int32 id)
        {
            try
            {
                Order order = _context.Orders.FirstOrDefault(o => o.Id == id);

                if (order == null)
                    return NotFound();

                var sutiOrdersToRemove = _context.SutiOrders.Where(so => so.OrderId == id).ToList();
                _context.SutiOrders.RemoveRange(sutiOrdersToRemove);

                _context.Orders.Remove(order);

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
