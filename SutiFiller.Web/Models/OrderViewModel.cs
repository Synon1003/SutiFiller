using System.ComponentModel.DataAnnotations;

namespace SutiFiller.Web.Models
{
    public class OrderViewModel : GuestViewModel
    {
        public Order? Order { get; set; }

    }
}
