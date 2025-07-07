namespace OrderMicroservice.API.DTOs
{
    public class CreateOrderRequestDto
    {
        public string? Reference { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}
