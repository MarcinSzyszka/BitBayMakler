using BitBayCurrencies.Enums;
using BitBayCurrencies.Extensions;

namespace BitBayPublicApi.Models.Requests
{
    public abstract class RequestBase
    {
        protected RequestBase(Currency currency, string categoryName)
        {
            Currency = currency;
            EndpointName = $"{currency.GetApiParameterName()}/{categoryName}";
        }

        protected string EndpointName { get; }

        private Currency Currency { get; }

        public abstract string GetEndPointUrl();
    }
}
