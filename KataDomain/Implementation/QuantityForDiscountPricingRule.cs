using System;
using System.Collections.Generic;
using System.Linq;
using Kata.Core.Abstractions;
using Kata.Domain.Abstractions;
using Kata.Domain.DTO;

namespace Kata.Domain.Implementation
{
    public class QuantityForDiscountPricingStrategy : IPricingStrategy
    {
        public QuantityForDiscount[] Rules { get; set; }
        public List<Order<char>> Orders { get; set; }
        public IOrderParser<char, string> OrderParser { get; }

        public QuantityForDiscountPricingStrategy()
        {

        }
        public QuantityForDiscountPricingStrategy(QuantityForDiscount[] rules, string orders,
            IOrderParser<char, string> orderParser)
        {
            Rules = rules;
            OrderParser = orderParser;
            Orders = OrderParser.ParseOrder(orders);
        }
        public double ApplyDiscountPerRule(List<Order<char>> orders)
        {
            List<char> discountedSkus = GetDiscounted(orders);
            QuantityForDiscount[] rulesToApplyy = Rules.Where(a => discountedSkus.Contains(a.SKU)).ToArray();
            //calculate prices for discounted orders and remove those orders
            return ApplyDiscountPerRule(orders, rulesToApplyy);
        }

        public double ApplyDiscountPerRule()
        {
            return ApplyDiscountPerRule(Orders);
        }


        private double ApplyDiscountPerRule(List<Order<char>> orders, QuantityForDiscount[] rulesToApply)
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
            return (int)Math.Round(orders.Sum(o => o.Price));

        }

        private List<char> GetDiscounted(List<Order<char>> orders)
        {
            return orders.Where(a => Rules.Any(r => r.SKU == a.Sku) &&
                    orders.Count(r => r.Sku == a.Sku) >=
                    Rules.First(r => r.SKU == a.Sku).Quantity)
                .Select(x => x.Sku).ToList();
        }
    }

}
