using System.Collections.Generic;
using System.Threading.Tasks;
using BitBayCurrencies.Enums;
using BitBayPublicApi.Models;

namespace BitBayTradesInfo.Services
{
    public interface ITradeInfoDbService
    {
        Task<IEnumerable<TradeTransaction>> GetStoredTransactions(Currency currency);
        Task SaveTransactions(IEnumerable<TradeTransaction> transactions, Currency currency);
    }
}
