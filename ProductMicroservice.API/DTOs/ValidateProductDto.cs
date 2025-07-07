using System;
using ProductMicroservice.API.DTOs;
using ProductMicroservice.API.Models;
using ProductMicroservice.API.Repositories;
using ProductMicroservice.API.Mappings;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ProductMicroservice.API.Services;

namespace ProductMicroservice.API.DTOs
{
    public static class ProductRequestValidator
    {
        public static void Validate(ProductRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Product name cannot be empty.");

            if (dto.Price < 0)
                throw new ArgumentException("Price cannot be negative.");

            if (dto.Quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");
        }
    }
}
