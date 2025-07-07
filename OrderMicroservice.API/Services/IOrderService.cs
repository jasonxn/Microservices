using System.Collections.Generic;
using System.Threading.Tasks;
using OrderMicroservice.API.DTOs;
namespace OrderMicroservice.API.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDto>> GetAllAsync();
        Task<OrderResponseDto?> GetByReferenceAsync(string reference);
        Task<(bool Success, string? Error, OrderResponseDto? Order)> CreateAsync(CreateOrderRequestDto dto);
        Task<(bool Success, string? Error)> UpdateAsync(string reference, CreateOrderRequestDto dto);
        Task<bool> DeleteAsync(string reference);
    }
}
