using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SutiFiller.Web.Models
{
    public class SutiService : ISutiService
    {
        private readonly SutisContext _context;

        public SutiService(SutisContext context)
        {
            _context = context;
        }

        public IEnumerable<Suti> Sutis => _context.Sutis.Include(s => s.Category);
        public IEnumerable<Category> Categories => _context.Categories;

        public Suti? GetSuti(Int32 sutiId)
        {
            return _context.Sutis
                .Include(s => s.Category)
                .FirstOrDefault(suti => suti.Id == sutiId);
        }

        public IEnumerable<Suti> GetSutis(Int32 categoryId)
        {
            return _context.Sutis.Where(suti => suti.CategoryId == categoryId);
        }

        public IEnumerable<Int32> GetSutiImageIds(Int32 sutiId)
        {
            return _context.Images
                .Where(image => image.SutiId == sutiId)
                .Select(image => image.Id);
        }

        public Byte[]? GetSutiMainImage(Int32 sutiId)
        {
            return _context.Images
                .Where(image => image.SutiId == sutiId)
                .Select(image => image.ImageSmall)
                .FirstOrDefault();
        }

        public Byte[]? GetSutiImage(Int32 imageId, Boolean large)
        {
            Byte[]? imageContent = _context.Images
                .Where(image => image.Id == imageId)
                .Select(image => large ? image.ImageLarge : image.ImageSmall)
                .FirstOrDefault();

            return imageContent;
        }

        public Guest GetGuest(String? userName)
        {
            if (userName == null)
                return null;

            return _context.Guests.FirstOrDefault(g => g.UserName == userName);
        }

        public Order GetOrderByGuestId(Int32? guestId)
        {
            if (guestId == null)
                return null;

            return _context.Orders
                //.Where(b => b.CustomerId == guestId && b.Ordered == false)
                .Where(o => o.CustomerId == guestId)
                .FirstOrDefault();
        }

        public Order GetOrderById(Int32? orderId)
        {
            if (orderId == null)
                return null;

            return _context.Orders
                //.Where(o => o.Id == orderId && o.Ordered == false)
                .Where(o => o.Id == orderId)
                .FirstOrDefault();
        }

        public List<SutiOrder> GetSutiOrdersByOrderId(Int32 orderId)
        {
            var sutiorders = _context.SutiOrders
                .Include(so => so.Suti).Where(so => so.OrderId == orderId).ToList();

            return sutiorders;
        }

        public OrderViewModel? NewOrder(Int32 orderId)
        {
            OrderViewModel orderViewModel = new OrderViewModel {};
            orderViewModel.Order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (orderViewModel.Order != null)
            { 
                orderViewModel.Order.SutiOrders = GetSutiOrdersByOrderId(orderId);
            }

            return orderViewModel;
        }

        public Boolean SaveOrder(String userName, OrderViewModel orderViewModel)
        {
            if (!Validator.TryValidateObject(orderViewModel, new ValidationContext(orderViewModel, null, null), null))
                return false;

            Guest? guest = _context.Guests.FirstOrDefault(g => g.UserName == userName);

            if (guest == null)
                return false;

            _context.Orders.Add(new Order
            {
                CustomerId = guest.Id,
            });

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public Int32 GetPrice(OrderViewModel orderViewModel)
        {
            Int32 price = 0;
            foreach (var so in orderViewModel.Order.SutiOrders) 
            {
                price += so.AllInPrice * so.Quantity;
            }
            return price;
        }

        public void AddSutiToSutiOrders(Order order, Int32 sutiId)
        {
            var suti = GetSuti(sutiId);
            var sutiorder = _context.SutiOrders.FirstOrDefault(
                s => s.OrderId == order.Id && s.SutiId == sutiId);

            if (suti == null) return;


            if (sutiorder != null)
            {
                sutiorder.Quantity += 1;
                sutiorder.AllInPrice += suti.Price;
                _context.SutiOrders.Update(sutiorder);
            }
            else
            {
                SutiOrder newSutiOrder = new SutiOrder()
                {
                    SutiId = sutiId,
                    OrderId = order.Id,
                    AllInPrice = suti.Price,
                    Quantity = 1,
                };
                _context.SutiOrders.Add(newSutiOrder);
            }

            order.TotalPrice += suti.Price;
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
    }
}
