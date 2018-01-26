using System.Collections.Generic;

namespace BitBayPublicApi.Models.Responses
{
    public class TradesResponse
    {
        public IEnumerable<TradeTransaction> Transactions { get; set; }
    }
}
