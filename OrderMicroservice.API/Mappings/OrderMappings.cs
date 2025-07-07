using System;
using System.Linq;
using OrderMicroservice.API.DTOs;
using OrderMicroservice.API.Models;
namespace OrderMicroservice.API.Mappings
{
    public static class OrderMappings
    {
        private static string GenerateReference() => $"ORD-{DateTime.UtcNow:yyyyMMdd-HHmmss}";

        public static Order ToEntity(this CreateOrderRequestDto dto)
        {
            var reference = !string.IsNullOrWhiteSpace(dto.Reference)
                ? dto.Reference
                : GenerateReference();

            var order = new Order
            {
                Reference = reference,
                CreatedAt = DateTime.UtcNow,
                Items = (dto.Items ?? Enumerable.Empty<CreateOrderItemDto>())
                    .Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity
                    })
                    .ToList()
            };

            return order;
        }

        public static OrderResponseDto ToResponseDto(this Order order)
        {
            return new OrderResponseDto
            {
                Reference = order.Reference,
                CreatedAt = order.CreatedAt,
                Items = (order.Items ?? Enumerable.Empty<OrderItem>())
                    .Select(i => new OrderItemResponseDto
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity
                    })
                    .ToList()
            };
        }
    }
}
