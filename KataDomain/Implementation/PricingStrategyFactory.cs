using System.Collections.Generic;
using Kata.Domain.DTO;
  
namespace Kata.Domain.Implementation {
    public static class PricingStrategyFactory
    {
        public static int GetQuantityForDiscountTotal(string order, List<QuantityForDiscount> rules, Dictionary<char, double> prices)
        {
            var eng = new PlainTextOrdersPricingEngine();
            eng.AddPricing(new QuantityForDiscountPricingStrategy(rules.ToArray(), order,
                new CharSkusOrderParser(new PriceOfSkuProvider<char>(prices))));
            return (int)eng.GetTotal();
        }
    }
}