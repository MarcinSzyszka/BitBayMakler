using BitBayPublicApi.Enums;

namespace BitBayPublicApi.Models
{
    public class TradeTransaction
    {
        public long Date { get; set; }

        public decimal Price { get; set; }

        public TradeTransactionType Type { get; set; }

        public decimal Amount { get; set; }

        public int Tid { get; set; }
    }
}
