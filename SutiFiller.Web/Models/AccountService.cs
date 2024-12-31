using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace SutiFiller.Web.Models
{
    public class AccountService : IAccountService
    {
        private readonly SutisContext _context;
        private readonly HttpContext _httpContext;
        private readonly ApplicationState _applicationState;

        public AccountService(SutisContext context,
            IHttpContextAccessor httpContextAccessor, ApplicationState applicationState)
        {
            _context = context;
            _httpContext = httpContextAccessor.HttpContext!;
            _applicationState = applicationState;

            // ha a felhasználónak van sütije, de még nincs bejelentkezve, bejelentkeztetjük
            if (_httpContext.Request.Cookies.ContainsKey("user_challenge") &&
                !_httpContext.Session.Keys.Contains("user"))
            {
                Guest? guest = _context.Guests.FirstOrDefault(
                    g => g.UserChallenge == _httpContext.Request.Cookies["user_challenge"]);
                
                if (guest != null)
                {
                    _httpContext.Session.SetString("user", guest.UserName);

                    UserCount++;
                }
            }
        }

        public Int32 UserCount
        {
            get => (Int32)_applicationState.UserCount;
            set => _applicationState.UserCount = value;
        }

        public String? CurrentUserName => _httpContext.Session.GetString("user");


        public Guest? GetGuest(String userName)
        {
            return _context.Guests.FirstOrDefault(c => c.UserName == userName);
        }

        public Boolean Login(LoginViewModel user)
        {
            if (!Validator.TryValidateObject(user, new ValidationContext(user, null, null), null))
                return false;

            Guest? guest =
                _context.Guests.FirstOrDefault(c => c.UserName == user.UserName);

            if (guest == null)
                return false;

            Byte[] passwordBytes;
            using (var alg = SHA512.Create())
            {
                passwordBytes = alg.ComputeHash(Encoding.UTF8.GetBytes(user.UserPassword));
            }

            if (!passwordBytes.SequenceEqual(guest.UserPassword))
                return false;

            _httpContext.Session.SetString("user", user.UserName);

            if (user.RememberLogin)
            {
                _httpContext.Response.Cookies.Append("user_challenge",
                    guest.UserChallenge,
                    new CookieOptions
                    {
                        Expires = DateTime.Today.AddDays(365),
                        HttpOnly = true,
                        //Secure = true,
                    });
            }

            UserCount++;

            return true;
        }

        public Boolean Logout()
        {
            if (!_httpContext.Session.Keys.Contains("user"))
                return false;

            _httpContext.Session.Remove("user");
            _httpContext.Response.Cookies.Delete("user_challenge");

            UserCount--;

            return true;
        }

        public Boolean Register(RegistrationViewModel guest)
        {
            if (!Validator.TryValidateObject(guest, new ValidationContext(guest, null, null), null))
                return false;

            if (_context.Guests.Count(c => c.UserName == guest.UserName) != 0)
                return false;

            Byte[] passwordBytes;
            using (var alg = SHA512.Create())
            {
                passwordBytes = alg.ComputeHash(Encoding.UTF8.GetBytes(guest.UserPassword));
            }

            _context.Guests.Add(new Guest
            {
                Name = guest.GuestName,
                Address = guest.GuestAddress,
                PhoneNumber = guest.GuestPhoneNumber,
                UserName = guest.UserName,
                UserPassword = passwordBytes,
                UserChallenge = Guid.NewGuid().ToString()
            });

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public Boolean Create(GuestViewModel guest, out String userName)
        {
            userName = "user" + Guid.NewGuid();

            if (!Validator.TryValidateObject(guest, new ValidationContext(guest, null, null), null))
                return false;

            _context.Guests.Add(new Guest
            {
                Name = guest.GuestName,
                Address = guest.GuestAddress,
                PhoneNumber = guest.GuestPhoneNumber,
                UserName = userName
            });

            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
