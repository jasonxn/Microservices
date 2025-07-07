using System.Collections.Generic;
using System.Threading.Tasks;
using ProductMicroservice.API.DTOs;

namespace ProductMicroservice.API.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto?> GetByIdAsync(int id);
        Task<ProductResponseDto> CreateAsync(ProductRequestDto dto);
        Task<bool> UpdateAsync(int id, ProductRequestDto dto);
        Task<bool> DeleteAsync(int id);
        Task<ValidationResponseDto> ValidateProductAsync(int id);
        Task<StockResponseDto> CheckStockAsync(int id, int requiredQuantity);
    }
}
