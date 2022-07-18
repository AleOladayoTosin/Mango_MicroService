using Mango.Service.ShoppingCartAPI.Models.Dtos;

namespace Mango.Service.ShoppingCartAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCoupon(string couponName);
    }
}
