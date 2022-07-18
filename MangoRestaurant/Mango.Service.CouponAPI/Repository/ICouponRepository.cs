using Mango.Service.CouponAPI.Models.Dtos;

namespace Mango.Service.CouponAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponByCode(string couponCode);
    }
}
