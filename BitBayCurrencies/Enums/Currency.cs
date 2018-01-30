using BitBayCurrencies.Attributes;

namespace BitBayCurrencies.Enums
{
    public enum Currency
    {
        [ApiParameterName("GAMEPLN")]
        Game = 1,
        [ApiParameterName("ETHPLN")]
        Ethereum = 2
    }
}
