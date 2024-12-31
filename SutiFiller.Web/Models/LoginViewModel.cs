using System.ComponentModel.DataAnnotations;

namespace SutiFiller.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "A felhasználónév megadása kötelező.")]
        public String UserName { get; set; } = null!;

        [Required(ErrorMessage = "A jelszó megadása kötelező.")]
        [DataType(DataType.Password)]
        public String UserPassword { get; set; } = null!;

        public Boolean RememberLogin { get; set; }
    }
}
