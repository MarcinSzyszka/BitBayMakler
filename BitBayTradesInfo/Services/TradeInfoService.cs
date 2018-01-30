using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BitBayApiManager;
using BitBayCurrencies.Enums;
using BitBayPublicApi.Models;
using BitBayPublicApi.Models.Requests;
using BitBayPublicApi.Services;
using BitBayTradesInfo.Enums;

namespace BitBayTradesInfo.Services
{
    public class TradeInfoService : ITradeInfoService
    {
        private readonly IPublicApiClientService _publicApiClient;

        private readonly ITradeInfoDbService _tradeInfoDbService;

        private readonly IApiRequestsManagingService _apiRequestsManagingService;

        private readonly IDictionary<Currency, TradeInfoState> _currencySyncStateDict;

        private readonly IDictionary<Currency, List<TradeTransaction>> _transactionsDict;

        public TradeInfoService(IPublicApiClientService publicApiClient, ITradeInfoDbService tradeInfoDbService, IApiRequestsManagingService apiRequestsManagingService)
        {
            _publicApiClient = publicApiClient;
            _tradeInfoDbService = tradeInfoDbService;
            _apiRequestsManagingService = apiRequestsManagingService;
            _currencySyncStateDict = new Dictionary<Currency, TradeInfoState>();
            _transactionsDict = new Dictionary<Currency, List<TradeTransaction>>();
        }

        public List<TradeTransaction> GetTransactions(Currency currency)
        {
            if (_transactionsDict.ContainsKey(currency))
            {
                return _transactionsDict[currency];
            }

            return null;
        }

        public TradeInfoState GetSyncState(Currency currency)
        {
            if (_currencySyncStateDict.ContainsKey(currency))
            {
                return _currencySyncStateDict[currency];
            }

            return TradeInfoState.NotSynchronized;
        }

        public async Task<TradeInfoState> Sync(Currency currency)
        {
            InitializeDictionaries(currency);

            if (_currencySyncStateDict[currency] != TradeInfoState.NotSynchronized)
            {
                return _currencySyncStateDict[currency];
            }

            var storedTransactions = (await _tradeInfoDbService.GetStoredTransactions(currency))?.ToList();

            if (storedTransactions == null)
            {
                storedTransactions = new List<TradeTransaction>();
            }

            var getTradeRequest = new GetTradesRequest(currency)
            {
                SinceTid = storedTransactions.OrderByDescending(t => t.Tid).FirstOrDefault()?.Tid
            };


            var syncFinished = false;

            try
            {
                var i = 1M;
                Console.WriteLine("Starting downloading trades with TID greater than = " + getTradeRequest.SinceTid);

                while (true)
                {
                    var newTransactions = await _apiRequestsManagingService.MakeRequest(
                        o => _publicApiClient.GetData<IEnumerable<TradeTransaction>, GetTradesRequest>(o),
                        getTradeRequest);

                    if (newTransactions == null || newTransactions.Count() == 0)
                    {
                        break;
                    }

                    storedTransactions.AddRange(newTransactions);

                    getTradeRequest.SinceTid = storedTransactions.OrderByDescending(t => t.Tid).FirstOrDefault()?.Tid;

                    Console.WriteLine("Last downloaded TID = " + getTradeRequest.SinceTid);

                    if (i % 100 == 0)
                    {
                        Console.WriteLine("Auto safe - Storing trades in file");

                        await _tradeInfoDbService.SaveTransactions(storedTransactions, currency);
                    }

                    i++;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Error occured: " + exc);

                _currencySyncStateDict[currency] = TradeInfoState.NotSynchronized;

                return _currencySyncStateDict[currency];
            }
            finally
            {
                await _tradeInfoDbService.SaveTransactions(storedTransactions, currency);
            }

            _transactionsDict[currency] = storedTransactions;

            //TODO Run background task - getting and synchronizing new data

            _currencySyncStateDict[currency] = TradeInfoState.Synchronized;

            Console.WriteLine("Sync finished successfully!");

            return _currencySyncStateDict[currency];
        }

        private void InitializeDictionaries(Currency currency)
        {
            if (!_currencySyncStateDict.ContainsKey(currency))
            {
                _currencySyncStateDict.Add(currency, TradeInfoState.NotSynchronized);
            }

            if (!_transactionsDict.ContainsKey(currency))
            {
                _transactionsDict.Add(currency, null);
            }
        }
    }
}
