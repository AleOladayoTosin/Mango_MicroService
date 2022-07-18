using Mango.Web.Models;
using Mango.Web.Services.IService;

namespace Mango.Web.Services
{
    public class ProductService :BaseService, IProductService
    {
        public ResponseDto response { get; set; }
        private IHttpClientFactory _clientFactory;
        public ProductService(IHttpClientFactory clientFactory):base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> CreateProductAsync<T>(ProductDto productDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = productDto,
                Url = $"{SD.ProductAPIBase}/api/products",
                AccessToken = token
            });
        }

        public async Task<T> DeleteProductAsync<T>(int productId, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = $"{SD.ProductAPIBase}/api/products/{productId}",
                AccessToken = token
            });
        }


        public async Task<T> GetAllProductAsync<T>(string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.ProductAPIBase}/api/products",
                AccessToken = token
            });
        }

        public async Task<T> GetProductByIdAsync<T>(int productId, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.ProductAPIBase}/api/products/{productId}",
                AccessToken = token
            });
        }
        

        public async Task<T> UpdateProductAsync<T>(ProductDto productDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = productDto,
                Url = $"{SD.ProductAPIBase}/api/products",
                AccessToken = token
            });
        }
    }
}
