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

        private List<Order> ParseOrder()
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
            List<(Order Order, bool Discounted)> ordersProcessing = orders
                .Select(a => (a, false)).ToList();
            //apply discounts
            List<char> discountedSkus = orders
                .Where(a => Rules.Any(r => r.SKU == a.Sku) &&
                    orders.Count(r => r.Sku == a.Sku) >=
                    Rules.First(r => r.SKU == a.Sku).Quantity)
                .Select(x => x.Sku).ToList();
            //calculate prices for discounted orders and remove those orders
            foreach (var sku in discountedSkus)
            {
                var noOfSkuOrders = orders.Count(a => a.Sku == sku);
                var discountRule = Rules.Where(r => r.SKU == sku)
                    .FirstOrDefault();

                var discount = discountRule.Discount;
                double discountUnit = (double)discount / discountRule.Quantity;
                var timesToApply = noOfSkuOrders / discountRule.Quantity;
                var currentTime = 0;
                int i = 0;
                ordersProcessing
                    .ForEach(op => {
                        if (i < discountRule.Quantity &&
                        ordersProcessing.Count(oplist => !oplist.Discounted &&
                        oplist.Order.Sku == sku) <= (discountRule.Quantity * 2)
                        && op.Order.Sku == sku && !op.Discounted)
                        {
                            op.Discounted = true;
                            op.Order.Price = discountUnit;
                            i++;
                        }
                        else
                        {
                            i = 0;
                            currentTime++;
                        }
                        
                    });

            }
            //sum discounted and non discounted orders
            return (int)Math.Ceiling(ordersProcessing.Sum(o => o.Order.Price));

        }
    }
}
