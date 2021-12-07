using System;
using System.Collections.Generic;
using Kata.Domain;
using Kata.Domain.DTO;
using Kata.Domain.Implementation;
using KataDomain.Implementation;
using Xunit;

namespace KataTest
{
    public class KataTestClass
    {
        [Fact]
        public void TestTotals()
        {
            List<QuantityForDiscountPricingRule<char>> rules;
            Dictionary<char, double> prices;
            MockData(out rules, out prices);

            var priceEngine = new PlainTextOrdersPricingEngine(rules,
                new CharSkusOrderParser(new PriceOfSkuProvider<char>(prices)));
            Assert.Equal(0, priceEngine.GetTotal(""));
            Assert.Equal(50, priceEngine.GetTotal("A"));
            Assert.Equal(80, priceEngine.GetTotal("AB"));
            Assert.Equal(115, priceEngine.GetTotal("CDBA"));

            Assert.Equal(100, priceEngine.GetTotal("AA"));
            Assert.Equal(130, priceEngine.GetTotal("AAA"));
            Assert.Equal(180, priceEngine.GetTotal("AAAA"));
            Assert.Equal(230, priceEngine.GetTotal("AAAAA"));
            Assert.Equal(260, priceEngine.GetTotal("AAAAAA"));
            Assert.Equal(360, priceEngine.GetTotal("AAAAAAAA"));
            Assert.Equal(390, priceEngine.GetTotal("AAAAAAAAA"));

            Assert.Equal(160, priceEngine.GetTotal("AAAB"));
            Assert.Equal(175, priceEngine.GetTotal("AAABB"));
            Assert.Equal(190, priceEngine.GetTotal("AAABBD"));
            Assert.Equal(190, priceEngine.GetTotal("DABABA"));
            Assert.Equal(430, priceEngine.GetTotal("DABABADABABAA"));

        }

        private static void MockData(out List<QuantityForDiscountPricingRule<char>> rules, out Dictionary<char, double> prices)
        {
            rules = new List<QuantityForDiscountPricingRule<char>>()
            {
                new QuantityForDiscountPricingRule<char>()
                {
                    SKU='A', Discount = 130, Quantity=3
                },
                new QuantityForDiscountPricingRule<char>()
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
