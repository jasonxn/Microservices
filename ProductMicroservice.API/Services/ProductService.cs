using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductMicroservice.API.DTOs;
using ProductMicroservice.API.Mappings;
using ProductMicroservice.API.Models;
using ProductMicroservice.API.Repositories;

namespace ProductMicroservice.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync().ConfigureAwait(false);
            return products.Select(p => p.ToResponseDto());
        }

        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id).ConfigureAwait(false);
            return product?.ToResponseDto();
        }

        public async Task<ProductResponseDto> CreateAsync(ProductRequestDto dto)
        {
            var entity = dto.ToEntity();
            var created = await _repo.AddAsync(entity).ConfigureAwait(false);
            return created.ToResponseDto();
        }

        public async Task<bool> UpdateAsync(int id, ProductRequestDto dto)
        {
            if (!await _repo.ExistsAsync(id).ConfigureAwait(false))
                return false;

            var entity = dto.ToEntity();
            entity.Id = id;
            await _repo.UpdateAsync(entity).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await _repo.ExistsAsync(id).ConfigureAwait(false))
                return false;

            await _repo.DeleteAsync(id).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> ValidateProductAsync(int id)
        {
            return await _repo.ExistsAsync(id).ConfigureAwait(false);
        }

        public async Task<bool> CheckStockAsync(int id, int requiredQuantity)
        {
            var product = await _repo.GetByIdAsync(id).ConfigureAwait(false);
            return product is not null && product.Quantity >= requiredQuantity;
        }
    }
}
