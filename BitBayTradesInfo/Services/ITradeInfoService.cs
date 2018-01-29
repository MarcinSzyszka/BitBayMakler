using System.Collections.Generic;
using System.Threading.Tasks;
using BitBayCurrencies.Enums;
using BitBayPublicApi.Models;
using BitBayTradesInfo.Enums;

namespace BitBayTradesInfo.Services
{
    public interface ITradeInfoService
    {
        TradeInfoState GetSyncState(Currency currency);
        List<TradeTransaction> GetTransactions(Currency currency);
        Task<TradeInfoState> Sync(Currency currency);
    }
}