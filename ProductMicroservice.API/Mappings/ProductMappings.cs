using ProductMicroservice.API.DTOs;
using ProductMicroservice.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductMicroservice.API.Mappings
{
    public static class ProductMappings
    {
        public static Product ToEntity(this ProductRequestDto dto)
        {
            return new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity
            };
        }

        public static ProductResponseDto ToResponseDto(this Product entity)
        {
            return new ProductResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                Quantity = entity.Quantity,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }

    }
}
