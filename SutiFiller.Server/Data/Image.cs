namespace SutiFiller.Server.Data
{
    public class Image
    {
        public int Id { get; set; }
        public int SutiId { get; set; }
        public byte[]? ImageSmall { get; set; }
        public byte[]? ImageLarge { get; set; }
        public Suti? Suti { get; set; }
    }
}
