namespace SutiFiller.Server.Data
{
    public class Guest
    {
        public Guest()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public byte[] UserPassword { get; set; } = null!;
        public string UserChallenge { get; set; } = null!;

        public ICollection<Order> Orders { get; set; }
    }
}
