namespace ProductMicroservice.API.DTOs
{
    public class StockResponseDto
    {
        public bool IsSufficient { get; set; }
        public string Message { get; set; } = null!;
    }
}
