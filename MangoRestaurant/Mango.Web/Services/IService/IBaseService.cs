using Mango.Web.Models;

namespace Mango.Web.Services.IService
{
    public interface IBaseService : IDisposable
    {
        ResponseDto response { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
