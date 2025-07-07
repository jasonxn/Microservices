namespace ProductMicroservice.API.DTOs
{
    public class ProductRequestDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
