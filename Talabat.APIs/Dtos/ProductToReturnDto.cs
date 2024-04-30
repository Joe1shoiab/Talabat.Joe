namespace Talabat.APIs.Dtos
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string productBrand { get; set; }
        public string productType { get; set; }
        public int productTypeId { get; set; }
        public int productBrandId { get; set; }

    }
}
