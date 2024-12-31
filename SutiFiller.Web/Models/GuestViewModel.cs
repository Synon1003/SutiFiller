using System.ComponentModel.DataAnnotations;

namespace SutiFiller.Web.Models
{
    public class GuestViewModel
    {
        [Required(ErrorMessage = "A név megadása kötelező.")]
        [StringLength(60, ErrorMessage = "A rendelő neve maximum 60 karakter lehet.")]
        public String GuestName { get; set; } = null!;


        [Required(ErrorMessage = "A cím megadása kötelező.")]
        public String GuestAddress { get; set; } = null!;

        [Required(ErrorMessage = "A telefonszám megadása kötelező.")]
        [Phone(ErrorMessage = "A telefonszám formátuma nem megfelelő.")]
        [DataType(DataType.PhoneNumber)]
        public String GuestPhoneNumber { get; set; } = null!;
    }
}
