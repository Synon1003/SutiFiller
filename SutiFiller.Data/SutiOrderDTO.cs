namespace SutiFiller.Data
{
    public class SutiOrderDTO
    {
        public Int32 Id { get; set; }
        public Int32 Quantity { get; set; }
        public Int32 AllInPrice { get; set; }
        public String? Message { get; set; }

        public Int32 OrderId { get; set; }
        public OrderDTO? Order { get; set; }
        public Int32 SutiId { get; set; }
        public SutiDTO? Suti { get; set; }
    }
}
