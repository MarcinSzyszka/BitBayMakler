using System.Collections.Generic;
using System.Threading.Tasks;
using BitBayPublicApi.Models;

namespace BitBayTradesInfo.Services
{
    public interface ITradeInfoDbService
    {
        Task<IEnumerable<TradeTransaction>> GetStoredTransactions();
        Task SaveTransactions(IEnumerable<TradeTransaction> transactions);
    }
}
