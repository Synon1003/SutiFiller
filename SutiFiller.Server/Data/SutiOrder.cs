namespace SutiFiller.Server.Data
{
    public class SutiOrder
    {
        public Int32 Id { get; set; }
        public Int32 Quantity { get; set; }
        public Int32 AllInPrice { get; set; }
        public String? Message { get; set; }

        public Int32 SutiId { get; set; }
        public Suti? Suti { get; set; }
        public Int32 OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
