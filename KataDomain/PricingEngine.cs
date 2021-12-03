using System;
using System.Collections.Generic;
using System.Linq;

namespace KataDomain
{
    public class PricingEngine
    {
        public PricingEngine(List<PricingRule> rules, Dictionary<char, int> prices)
        {
            Rules = rules;
            Prices = prices;
        }

        private string _order;

        public List<PricingRule> Rules { get; }
        public Dictionary<char, int> Prices { get; }

        private List<Order> ParseOrder()//TODO: refactor out as interface to inject in usin DI, remove implementation from here to make extensible
        {
            char[] skus = _order.ToCharArray();
            List<Order> orders = new List<Order>();
            for (int i = 0; i < skus.Length; i++)
            {
                var price = Prices[skus[i]];//what if not found?

                orders.Add(new Order()
                {
                    Sku = skus[i],
                    Price = price
                });
            }
            return orders;
        } 
        public int GetTotal(string order)
        {
            _order = order;
            List<Order> orders = ParseOrder();
            return GetTotal(orders);
        }

        private int GetTotal(List<Order> orders)
        { 
            //apply discounts
            List<char> discountedSkus = orders
                .Where(a => Rules.Any(r => r.SKU == a.Sku) &&
                    orders.Count(r => r.Sku == a.Sku) >=
                    Rules.First(r => r.SKU == a.Sku).Quantity)
                .Select(x => x.Sku).ToList();
            var rulesToApplyy = Rules.Where(a => discountedSkus.Contains(a.SKU)).ToArray();
            //calculate prices for discounted orders and remove those orders

            for (int i = 0; i < rulesToApplyy.Length; i++)
            {
                double discountUnit = (double)rulesToApplyy[i].Discount / rulesToApplyy[i].Quantity;

                var rounds = Math.Floor((decimal)orders.Count(a => a.Sku == rulesToApplyy[i].SKU) / rulesToApplyy[i].Quantity);
                for (int j = 0; j < rounds; j++)
                {
                    for (int k = 0; k < rulesToApplyy[i].Quantity; k++)
                    {
                        orders.Where(a => a.Price != discountUnit && a.Sku == rulesToApplyy[i].SKU).FirstOrDefault().Price = discountUnit;
                    }
                }
            }

            //sum discounted and non discounted orders
            return (int)Math.Round(orders.Sum(o => o.Price));

        }
    }
}
