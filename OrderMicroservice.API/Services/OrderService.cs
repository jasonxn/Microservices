using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderMicroservice.API.Clients;
using OrderMicroservice.API.DTOs;
using OrderMicroservice.API.Mappings;
using OrderMicroservice.API.Models;
using OrderMicroservice.API.Repositories;
namespace OrderMicroservice.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductClient _productClient;

        public OrderService(
            IOrderRepository orderRepo,
            IProductClient productClient)
        {
            _orderRepo = orderRepo;
            _productClient = productClient;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
        {
            var orders = await _orderRepo.GetAllAsync().ConfigureAwait(false);
            return orders.Select(o => o.ToResponseDto());
        }

        public async Task<OrderResponseDto?> GetByReferenceAsync(string reference)
        {
            var order = await _orderRepo.GetByReferenceAsync(reference).ConfigureAwait(false);
            return order?.ToResponseDto();
        }

        public async Task<(bool Success, string? Error, OrderResponseDto? Order)> CreateAsync(CreateOrderRequestDto dto)
        {
            foreach (var item in dto.Items)
            {
                var exists = await _productClient.ValidateProductAsync(item.ProductId).ConfigureAwait(false);
                if (!exists)
                    return (false, $"Product ID {item.ProductId} not found.", null);

                var inStock = await _productClient.CheckStockAsync(item.ProductId, item.Quantity)
                                              .ConfigureAwait(false);
                if (!inStock)
                    return (false, $"Insufficient stock for product ID {item.ProductId}.", null);
            }

            var orderEntity = dto.ToEntity();
            var created = await _orderRepo.AddAsync(orderEntity).ConfigureAwait(false);
            
            return (true, null, created.ToResponseDto());
        }

        public async Task<(bool Success, string? Error)> UpdateAsync(string reference, CreateOrderRequestDto dto)
        {
            var existing = await _orderRepo.GetByReferenceAsync(reference).ConfigureAwait(false);
            if (existing == null)
                return (false, $"Order with reference {reference} not found.");

            foreach (var item in dto.Items)
            {
                var exists = await _productClient.ValidateProductAsync(item.ProductId).ConfigureAwait(false);
                if (!exists)
                    return (false, $"Product ID {item.ProductId} not found.");

                var inStock = await _productClient.CheckStockAsync(item.ProductId, item.Quantity)
                                              .ConfigureAwait(false);
                if (!inStock)
                    return (false, $"Insufficient stock for product ID {item.ProductId}.");
            }

            existing.Items = dto.Items
                                .Select(i => new OrderItem
                                {
                                    ProductId = i.ProductId,
                                    Quantity = i.Quantity,
                                    OrderId = existing.Id
                                })
                                .ToList();

            if (!string.IsNullOrWhiteSpace(dto.Reference))
                existing.Reference = dto.Reference;

            await _orderRepo.UpdateAsync(existing).ConfigureAwait(false);
            return (true, null);
        }

        public async Task<bool> DeleteAsync(string reference)
        {
            var existing = await _orderRepo.GetByReferenceAsync(reference).ConfigureAwait(false);
            if (existing == null)
                return false;

            await _orderRepo.DeleteAsync(existing).ConfigureAwait(false);
            return true;
        }
    }
}
