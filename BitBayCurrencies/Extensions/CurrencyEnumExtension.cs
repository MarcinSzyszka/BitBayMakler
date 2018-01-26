using System;
using System.Reflection;
using BitBayCurrencies.Attributes;
using BitBayCurrencies.Enums;

namespace BitBayCurrencies.Extensions
{
    public static class CurrencyEnumExtension
    {
        public static string GetApiParameterName(this Currency currency)
        {

            return (currency.GetType().GetCustomAttribute(typeof(ApiParameterNameAttribute)) as ApiParameterNameAttribute)?.Name;

            var currencyValues = Enum.GetValues(currency.GetType());

            foreach (Currency curr in currencyValues)
            {
                if (curr == currency)
                {

                }
            }
        }
    }
}
