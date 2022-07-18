using Mango.Web.Models;

namespace Mango.Web.Services.IService
{
    public interface ICartService
    {
        Task<T> GetCartByUserIdAsync<T>(string userId, string token = null);
        Task<T> AddToCardAsync<T>(CartDto cardDto, string token = null);
        Task<T> UpdateToCardAsync<T>(CartDto cardDto, string token = null);
        Task<T> RemoveFromCardAsync<T>(int cardId, string token = null);
        Task<T> ApplyCoupon<T>(CartDto cardDto, string token = null);
        Task<T> RemoveCoupon<T>(string UserId, string token = null);
        Task<T> Checkout<T>(CartHeaderDto cartHeader, string token = null);

    }
}
