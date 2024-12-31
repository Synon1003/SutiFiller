namespace SutiFiller.Web.Models
{
    public class Category
    {
        public Category()
        {
            Sutis = new List<Suti>();
        }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Suti> Sutis { get; set; }
    }
}
