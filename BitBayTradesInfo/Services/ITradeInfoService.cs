using System.Threading.Tasks;
using BitBayTradesInfo.Enums;

namespace BitBayTradesInfo.Services
{
    public interface ITradeInfoService
    {
        TradeInfoState State { get; }
        Task<TradeInfoState> Sync();
    }
}