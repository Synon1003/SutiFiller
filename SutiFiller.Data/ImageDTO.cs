namespace SutiFiller.Data
{
    public class ImageDTO
    {
        public Int32 Id { get; set; }
        public Int32 SutiId { get; set; }
        public Byte[]? ImageSmall { get; set; }
        public Byte[]? ImageLarge { get; set; }
    }
}
