namespace SutiFiller.Web.Models
{
    public class Suti
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Price { get; set; }
        public string Description { get; set; } = null!;

        public Int32 CategoryId { get; set; }
        public Category Category { get; set; } = null!;


    }
}
