using Mango.Web.Models;
using Mango.Web.Services.IService;

namespace Mango.Web.Services
{
    public class CartService : BaseService, ICartService
    {
        public ResponseDto response { get; set; }
        private IHttpClientFactory _clientFactory;
        public CartService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> AddToCardAsync<T>(CartDto cardDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cardDto,
                Url = $"{SD.ShoppingCartAPIBase}/api/cart/AddCart",
                AccessToken = token
            });
        }

        public async Task<T> GetCartByUserIdAsync<T>(string userId, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.ShoppingCartAPIBase}/api/cart/GetCart/{userId}",
                AccessToken = token
            });
        }

        public async Task<T> RemoveFromCardAsync<T>(int cardId, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cardId,
                Url = $"{SD.ShoppingCartAPIBase}/api/cart/RemoveCart",
                AccessToken = token
            });
        }

        public async Task<T> UpdateToCardAsync<T>(CartDto cardDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = cardDto,
                Url = $"{SD.ShoppingCartAPIBase}/api/cart/UpdateCart",
                AccessToken = token
            });
        }

        public async Task<T> ApplyCoupon<T>(CartDto cardDto, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cardDto,
                Url = $"{SD.ShoppingCartAPIBase}/api/cart/ApplyCoupon",
                AccessToken = token
            });
        }

        public async Task<T> RemoveCoupon<T>(string UserId, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = UserId,
                Url = $"{SD.ShoppingCartAPIBase}/api/cart/RemoveCoupon",
                AccessToken = token
            });
        }

        public async Task<T> Checkout<T>(CartHeaderDto cartHeader, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartHeader,
                Url = $"{SD.ShoppingCartAPIBase}/api/cart/checkout",
                AccessToken = token
            });
        }
    }
}
