using BitBayCurrencies.Enums;

namespace BitBayPublicApi.Models.Requests
{
    public class GetTradesRequest : RequestBase
    {
        public GetTradesRequest(Currency currency) : base(currency, "trades.json")
        {
        }

        public bool SortDesc { get; set; }

        public int? SinceTid { get; set; }

        public override string GetEndPointUrl()
        {
            string parameters = "?" + (SortDesc ? "desc" : "asc");

            if (SinceTid.HasValue)
            {
                parameters += $"&since={SinceTid.Value}";
            }

            return EndpointName + parameters;
        }
    }
}
