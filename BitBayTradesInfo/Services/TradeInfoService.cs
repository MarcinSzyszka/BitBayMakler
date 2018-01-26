using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitBayCurrencies.Enums;
using BitBayPublicApi.Models;
using BitBayPublicApi.Models.Requests;
using BitBayPublicApi.Services;
using BitBayTradesInfo.Enums;

namespace BitBayTradesInfo.Services
{
    public class TradeInfoService : ITradeInfoService
    {
        private readonly IPublicApiClient _publicApiClient;

        private readonly ITradeInfoDbService _tradeInfoDbService;

        private TradeInfoState _state;

        public TradeInfoService(IPublicApiClient publicApiClient, ITradeInfoDbService tradeInfoDbService)
        {
            _publicApiClient = publicApiClient;
            _tradeInfoDbService = tradeInfoDbService;
        }

        public TradeInfoState State => _state;

        public async Task<TradeInfoState> Sync()
        {
            if (_state != TradeInfoState.NotSynchronized)
            {
                return _state;
            }

            _state = TradeInfoState.SynchronizationStarted;

            var storedTransactions = (await _tradeInfoDbService.GetStoredTransactions()).ToList();

            var getTradeRequest = new GetTradesRequest(Currency.Game)
            {
                SinceTid = storedTransactions.OrderByDescending(t => t.Tid).FirstOrDefault()?.Tid
            };


            var syncFinished = false;

            while (!syncFinished)
            {
                var newTransactions = await _publicApiClient.GetData<IEnumerable<TradeTransaction>, GetTradesRequest>(getTradeRequest);

                if (newTransactions == null || newTransactions.Count() == 0)
                {
                    syncFinished = true;
                }

                storedTransactions.AddRange(newTransactions);

                getTradeRequest.SinceTid = storedTransactions.OrderByDescending(t => t.Tid).FirstOrDefault()?.Tid;
            }

            _state = TradeInfoState.Synchronized;

            return _state;
        }
    }
}
