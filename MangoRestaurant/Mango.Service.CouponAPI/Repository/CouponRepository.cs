using AutoMapper;
using Mango.Service.CouponAPI.DbContexts;
using Mango.Service.CouponAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        public CouponRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
       
        public async Task<CouponDto> GetCouponByCode(string couponCode)
        {
            var couponModel = await _db.coupons
               .Where(c => c.CouponCode == couponCode).FirstOrDefaultAsync();
            return _mapper.Map<CouponDto>(couponModel);
        }
    }
}
