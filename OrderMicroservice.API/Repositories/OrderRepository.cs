using Microsoft.EntityFrameworkCore;
using OrderMicroservice.API.Data;
using OrderMicroservice.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace OrderMicroservice.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;

        public OrderRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _db.Orders
                            .Include(o => o.Items)
                            .AsNoTracking()
                            .ToListAsync()
                            .ConfigureAwait(false);
        }

        public async Task<Order?> GetByReferenceAsync(string reference)
        {
            return await _db.Orders
                            .Include(o => o.Items)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(o => o.Reference == reference)
                            .ConfigureAwait(false);
        }

        public async Task<Order> AddAsync(Order order)
        {
            var entity = (await _db.Orders.AddAsync(order).ConfigureAwait(false)).Entity;
            await _db.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task UpdateAsync(Order order)
        {
            _db.Orders.Update(order);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Order order)
        {
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

