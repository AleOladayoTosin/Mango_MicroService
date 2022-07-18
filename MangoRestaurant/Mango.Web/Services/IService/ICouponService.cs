namespace Mango.Web.Services.IService
{
    public interface ICouponService
    {
        Task<T> GetCouponByCodeAsync<T>(string code, string token = null);
    }
}
