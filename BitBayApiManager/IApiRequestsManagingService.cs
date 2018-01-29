using System;
using System.Threading.Tasks;

namespace BitBayApiManager
{
    public interface IApiRequestsManagingService
    {
        Task<TOut> MakeRequest<TOut, TIn>(Func<TIn, Task<TOut>> requestAction, TIn parameter);
    }
}
