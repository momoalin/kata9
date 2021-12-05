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
        public PlainTextOrdersPricingEngine(List<PricingRule<char>> rules, IOrderParser<char, string> orderParser)
        {
            Rules = rules;
            OrderParser = orderParser;
        }


        public List<PricingRule<char>> Rules { get; }
        public IOrderParser<char, string> OrderParser { get; }

        public int GetTotal(string order)
        {
            List<Order<char>> orders = OrderParser.ParseOrder(order);
            return GetTotal(orders);
        }

        private int GetTotal(List<Order<char>> orders)
        { 
            //apply discounts
            List<char> discountedSkus = orders
                .Where(a => Rules.Any(r => r.SKU == a.Sku) &&
                    orders.Count(r => r.Sku == a.Sku) >=
                    Rules.First(r => r.SKU == a.Sku).Quantity)
                .Select(x => x.Sku).ToList();
            PricingRule<char>[] rulesToApplyy = Rules.Where(a => discountedSkus.Contains(a.SKU)).ToArray();
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
