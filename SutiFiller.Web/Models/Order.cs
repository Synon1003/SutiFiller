using System.ComponentModel.DataAnnotations.Schema;

namespace SutiFiller.Web.Models
{
    public class Order
    {
        public Int32 Id { get; set; }
        public Int32 CustomerId { get; set; }
        public Int32 PrePayment { get; set; }
        public Int32 TotalPrice { get; set; }

        public String Name { get; set; } = null!;
        public String PhoneNumber { get; set; } = null!;
        public String BillingAddress { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public String? Comment { get; set; }

        public List<SutiOrder>? SutiOrders { get; set; }
        public Status? Status { get; set; }
        public Int32 StatusId { get; set; }
    }
}
