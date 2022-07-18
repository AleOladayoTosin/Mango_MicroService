using Mango.Service.CouponAPI.Models.Dtos;
using Mango.Service.CouponAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Service.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    public class CouponAPIController : Controller
    {
        protected ResponseDto _response;
        private readonly ICouponRepository _cartRepository;
        public CouponAPIController(ICouponRepository cartRepository)
        {
            _cartRepository = cartRepository;
            this._response = new ResponseDto();
        }
        [Authorize]
        [HttpGet("{code}")]
        public async Task<object> GetDiscountForCode(string code)
        {
            try
            {
                CouponDto couponDto = await _cartRepository.GetCouponByCode(code);
                _response.Result = couponDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
