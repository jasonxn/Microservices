using System;
using System.Collections.Generic;
namespace OrderMicroservice.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Reference { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }
}
