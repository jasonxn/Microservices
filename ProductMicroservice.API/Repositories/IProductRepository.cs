using System.Collections.Generic;
using System.Threading.Tasks;
using ProductMicroservice.API.Models;

namespace ProductMicroservice.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
