using System.Linq;
using System.Reflection;
using BitBayCurrencies.Attributes;
using BitBayCurrencies.Enums;

namespace BitBayCurrencies.Extensions
{
    public static class CurrencyEnumExtension
    {
        public static string GetApiParameterName(this Currency currency)
        {
            return (currency.GetType().GetMember(currency.ToString()).FirstOrDefault()?.GetCustomAttribute(typeof(ApiParameterNameAttribute)) as ApiParameterNameAttribute)?.Name;
        }
    }
}
