using System.Threading.Tasks;

namespace OrderMicroservice.API.Clients
{
    public interface IProductClient
    {
        Task<bool> ValidateProductAsync(int productId);
        Task<bool> CheckStockAsync(int productId, int requiredQuantity);
    }
}
