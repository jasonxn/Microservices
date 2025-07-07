using Microsoft.EntityFrameworkCore;
using ProductMicroservice.API.Data;
using ProductMicroservice.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductMicroservice.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products
                            .AsNoTracking()
                            .ToListAsync()
                            .ConfigureAwait(false);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _db.Products
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p => p.Id == id)
                            .ConfigureAwait(false);
        }

        public async Task<Product> AddAsync(Product product)
        {
            var entity = (await _db.Products.AddAsync(product).ConfigureAwait(false)).Entity;
            await _db.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task UpdateAsync(Product product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _db.Products.FindAsync(id).ConfigureAwait(false);
            if (existing != null)
            {
                _db.Products.Remove(existing);
                await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Products.AnyAsync(p => p.Id == id).ConfigureAwait(false);
        }
    }
}
