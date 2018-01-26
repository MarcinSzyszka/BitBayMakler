using System;

namespace BitBayCurrencies.Attributes
{
    public class ApiParameterNameAttribute : Attribute
    {
        public ApiParameterNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
