using Mango.Web.Models;
using Mango.Web.Services.IService;

namespace Mango.Web.Services
{
    public class CouponService : BaseService, ICouponService
    {
        private IHttpClientFactory _clientFactory;
        public CouponService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> GetCouponByCodeAsync<T>(string code, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = $"{SD.CouponAPIBase}/api/coupon/{code}",
                AccessToken = token
            });
        }
    }
}
