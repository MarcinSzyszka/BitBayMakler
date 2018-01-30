using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BitBayApiManager
{
    public class ApiRequestsManagingService : IApiRequestsManagingService
    {
        private readonly ConcurrentDictionary<long, object> _waitingRequests;

        public ApiRequestsManagingService()
        {
            _waitingRequests = new ConcurrentDictionary<long, object>();
        }

        public async Task<TOut> MakeRequest<TOut, TIn>(Func<TIn, Task<TOut>> requestAction, TIn parameter)
        {
            _waitingRequests.TryAdd(DateTime.Now.Ticks, requestAction);

            var result = default(TOut);

            var executed = false;

            int elapsedMiliseconds = 0;
            var stoper = new Stopwatch();

            while (!executed)
            {
                stoper.Reset();

                var delayMiliseconds = elapsedMiliseconds < 1000 ? 1000 - elapsedMiliseconds : 0;

                await Task.Delay(delayMiliseconds);

                var firstRequestInQeue = _waitingRequests.OrderByDescending(r => r.Key).First();

                if ((firstRequestInQeue.Value as Func<TIn, Task<TOut>>) == requestAction)
                {
                    stoper.Start();

                    result = await requestAction(parameter);

                    stoper.Stop();
                    elapsedMiliseconds = (int)stoper.ElapsedMilliseconds;

                    var deleted = false;

                    while (!deleted)
                    {
                        if (DeleteReuqest(firstRequestInQeue))
                        {
                            deleted = true;
                        }
                    }

                    executed = true;
                }
            }

            return result;
        }

        private bool DeleteReuqest(KeyValuePair<long, object> firstRequestInQeue)
        {
            return _waitingRequests.TryRemove(firstRequestInQeue.Key, out var requestToRemove);
        }
    }
}
