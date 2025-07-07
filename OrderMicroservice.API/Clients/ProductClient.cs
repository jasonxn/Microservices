using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace OrderMicroservice.API.Clients
{
    public class ProductClientOptions
    {
        public string BaseUrl { get; set; } = null!;
    }

    public class ProductClient : IProductClient
    {
        private readonly HttpClient _http;
        private readonly ILogger<ProductClient> _logger;

        public ProductClient(HttpClient http, IOptions<ProductClientOptions> opts, ILogger<ProductClient> logger)
        {
            _http = http;
            _logger = logger;
            _http.BaseAddress = new Uri(opts.Value.BaseUrl);
        }

        public async Task<bool> ValidateProductAsync(int productId)
        {
            var response = await _http.GetAsync($"/api/products/{productId}/validate")
                                      .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<bool> CheckStockAsync(int productId, int requiredQuantity)
        {
            var response = await _http.GetAsync($"/api/products/{productId}/stock/{requiredQuantity}")
                                      .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.BadRequest)
                return false;

            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
