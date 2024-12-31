using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SutiFiller.Data
{
    public class OrderDTO
    {
        public Int32 Id { get; set; }
        public Int32 PrePayment { get; set; }
        public Int32 TotalPrice { get; set; }

        public String Name { get; set; } = null!;
        public String BillingAddress { get; set; } = null!;
        public String PhoneNumber { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public String? Comment { get; set; }

        public StatusDTO? Status { get; set; }
        public Int32 StatusId { get; set; }
        public IList<SutiOrderDTO>? SutiOrders { get; set; } = new List<SutiOrderDTO>();
    }
}
