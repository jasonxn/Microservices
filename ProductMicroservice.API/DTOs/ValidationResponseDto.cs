namespace ProductMicroservice.API.DTOs
{
    public class ValidationResponseDto
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = null!;
    }
}
