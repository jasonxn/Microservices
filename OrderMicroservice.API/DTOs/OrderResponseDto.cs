using System;
using System.Collections.Generic;
namespace OrderMicroservice.API.DTOs
{
    public class OrderResponseDto
    {
        public string Reference { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }
}
