namespace SutiFiller.Web.Models
{
    public interface ISutiService
    {
        IEnumerable<Suti> Sutis { get; }
        IEnumerable<Category> Categories { get; }

        Suti? GetSuti(Int32 sutiId);
        IEnumerable<Suti> GetSutis(Int32 categoryId);
        IEnumerable<Int32> GetSutiImageIds(Int32 sutiId);
        Byte[]? GetSutiMainImage(Int32 sutiId);
        Byte[]? GetSutiImage(Int32 imageId, Boolean large);
        Guest GetGuest(String? userName);
        Order GetOrderByGuestId(Int32? guestId);
        Order GetOrderById(Int32? orderId);
        List<SutiOrder> GetSutiOrdersByOrderId(Int32 orderId);
        OrderViewModel? NewOrder(Int32 orderId);

        Boolean SaveOrder(String userName, OrderViewModel orderViewModel);
        Int32 GetPrice(OrderViewModel orderViewModel);
        void AddSutiToSutiOrders(Order order, Int32 sutiId);
    }
}
