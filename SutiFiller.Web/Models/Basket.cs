namespace SutiFiller.Web.Models
{
    public class Basket
    {
        public Int32 Id { get; set; }
        public Int32 GuestId { get; set; }
        public String Name { get; set; }
        public String BillingAddress { get; set; }
        public String PhoneNumber { get; set; }
        public List<SutiOrder>? SutiOrders { get; set; }
        public Boolean Ordered { get; set; }
        public Int32 TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
