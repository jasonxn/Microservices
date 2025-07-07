using System.Collections.Generic;
using System.Threading.Tasks;
using OrderMicroservice.API.Models;
namespace OrderMicroservice.API.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByReferenceAsync(string reference);
        Task<Order> AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
    }
}
