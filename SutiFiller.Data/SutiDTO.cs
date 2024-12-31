namespace SutiFiller.Data
{
    public class SutiDTO
    {
        public Int32 Id { get; set; }
        public String Name { get; set; } = null!;
        public String Description { get; set; } = null!;
        public Int32 Price { get; set; }
        public Int32 CategoryId { get; set; }
        public CategoryDTO? Category { get; set; }
        public IList<ImageDTO>? Images { get; set; }
        public override Boolean Equals(Object? obj)
        {
            return (obj is SutiDTO dto) && Name == dto.Name;
        }
    }
}
