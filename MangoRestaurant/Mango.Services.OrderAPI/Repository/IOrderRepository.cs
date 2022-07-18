using Mango.Services.OrderAPI.Models;

namespace Mango.Services.OrderAPI.Repository
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader order);
        Task UpdateOrderPaymentStatus(int orderHearderId, bool paid);
    }
}
