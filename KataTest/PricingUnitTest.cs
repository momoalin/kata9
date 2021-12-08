using System.Collections.Generic;
using Kata.Domain.DTO;
using Kata.Domain.Implementation;
using Xunit;

namespace KataTest
{
    public class KataTestClass
    {
        [Fact]
        public void TestTotals()
        {
            List<QuantityForDiscount> rules;
            Dictionary<char, double> prices;
            MockData(out rules, out prices);

            Assert.Equal(0, PricingStrategyFactory.GetQuantityForDiscountTotal("", rules, prices));
            Assert.Equal(50, PricingStrategyFactory.GetQuantityForDiscountTotal("A", rules, prices));
            Assert.Equal(80, PricingStrategyFactory.GetQuantityForDiscountTotal("AB", rules, prices));
            Assert.Equal(115, PricingStrategyFactory.GetQuantityForDiscountTotal("CDBA", rules, prices));

            Assert.Equal(100, PricingStrategyFactory.GetQuantityForDiscountTotal("AA", rules, prices));
            Assert.Equal(130, PricingStrategyFactory.GetQuantityForDiscountTotal("AAA", rules, prices));
            Assert.Equal(180, PricingStrategyFactory.GetQuantityForDiscountTotal("AAAA", rules, prices));
            Assert.Equal(230, PricingStrategyFactory.GetQuantityForDiscountTotal("AAAAA", rules, prices));
            Assert.Equal(260, PricingStrategyFactory.GetQuantityForDiscountTotal("AAAAAA", rules, prices));
            Assert.Equal(360, PricingStrategyFactory.GetQuantityForDiscountTotal("AAAAAAAA", rules, prices));
            Assert.Equal(390, PricingStrategyFactory.GetQuantityForDiscountTotal("AAAAAAAAA", rules, prices));

            Assert.Equal(160, PricingStrategyFactory.GetQuantityForDiscountTotal("AAAB", rules, prices));
            Assert.Equal(175, PricingStrategyFactory.GetQuantityForDiscountTotal("AAABB", rules, prices));
            Assert.Equal(190, PricingStrategyFactory.GetQuantityForDiscountTotal("AAABBD", rules, prices));
            Assert.Equal(190, PricingStrategyFactory.GetQuantityForDiscountTotal("DABABA", rules, prices));
            Assert.Equal(430, PricingStrategyFactory.GetQuantityForDiscountTotal("DABABADABABAA", rules, prices));

        }

        private static void MockData(out List<QuantityForDiscount> rules, out Dictionary<char, double> prices)
        {
            rules = new List<QuantityForDiscount>()
            {
                new QuantityForDiscount()
                {
                    SKU='A', Discount = 130, Quantity=3
                },
                new QuantityForDiscount()
                {
                    SKU='B', Discount = 45, Quantity=2
                }
            };
            prices = new Dictionary<char, double>();
            prices.Add('A', 50);
            prices.Add('B', 30);
            prices.Add('C', 20);
            prices.Add('D', 15);
        }
    }
}
