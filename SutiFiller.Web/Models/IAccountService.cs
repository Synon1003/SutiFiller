namespace SutiFiller.Web.Models
{
    public interface IAccountService
    {
        Int32 UserCount { get; }
        String? CurrentUserName { get; }

        Guest? GetGuest(String userName);
        Boolean Login(LoginViewModel user);
        Boolean Logout();
        Boolean Register(RegistrationViewModel guest);
        Boolean Create(GuestViewModel guest, out String userName);
    }
}
