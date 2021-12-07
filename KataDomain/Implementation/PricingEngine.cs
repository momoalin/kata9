using System;
using System.Collections.Generic;
using System.Linq;
using Kata.Domain.DTO;
using Kata.Core.Abstractions;
using Kata.Domain.Abstractions;

namespace Kata.Domain.Implementation
{
    public class PlainTextOrdersPricingEngine : IPriceEngine<string>//char skus  snot csv
    {
        public PlainTextOrdersPricingEngine(List<QuantityForDiscountPricingRule<char>> rules, IOrderParser<char, string> orderParser)
        {
            Rules = rules;
            OrderParser = orderParser;
        }
        public List<QuantityForDiscountPricingRule<char>> Rules { get; }
        public IOrderParser<char, string> OrderParser { get; }

        public int GetTotal(string order)
        {
            List<Order<char>> orders = OrderParser.ParseOrder(order);
            //apply discounts
            List<char> discountedSkus = GetDiscounted(orders);
            QuantityForDiscountPricingRule<char>[] rulesToApplyy = Rules.Where(a => discountedSkus.Contains(a.SKU)).ToArray();
            //calculate prices for discounted orders and remove those orders
            ApplyDiscountPerRule(orders, rulesToApplyy);
            //sum discounted and non discounted orders
            return (int)Math.Round(orders.Sum(o => o.Price));
        }

        private void ApplyDiscountPerRule(List<Order<char>> orders, QuantityForDiscountPricingRule<char>[] rulesToApply)
        {
            for (int i = 0; i < rulesToApply.Length; i++)
            {
                double discountUnit = (double)rulesToApply[i].Discount / rulesToApply[i].Quantity;
                var rounds = Math.Floor((decimal)orders.Count(a => a.Sku == rulesToApply[i].SKU) / rulesToApply[i].Quantity);
                for (int j = 0; j < rounds; j++)
                {
                    for (int k = 0; k < rulesToApply[i].Quantity; k++)
                    {
                        orders.Where(a => a.Price != discountUnit && a.Sku == rulesToApply[i].SKU)
                                .FirstOrDefault().Price = discountUnit;
                    }
                }
            }
        }

        private List<char> GetDiscounted(List<Order<char>> orders)///bool dd = orders.Count(r => r.Sku == a.Sku) >= Rules.First(r => r.SKU == a.Sku).Quantity;
        {
            return orders
                .Where(a => Rules.Any(r => r.SKU == a.Sku) &&
                    orders.Count(r => r.Sku == a.Sku) >=
                    Rules.First(r => r.SKU == a.Sku).Quantity)
                .Select(x => x.Sku).ToList();
        }

    }
}
