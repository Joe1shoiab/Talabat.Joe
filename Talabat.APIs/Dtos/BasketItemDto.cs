namespace Talabat.APIs.Dtos
{
    public class BasketItemDto
    {
        public int Id { get; set; }

        public string productName { get; set; }
        public string Quantity { get; set; } // to choose more items
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
    }
}