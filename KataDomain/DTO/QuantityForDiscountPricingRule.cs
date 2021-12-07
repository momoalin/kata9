using System.Collections.Generic;
using KataDomain.Abstractions;

namespace Kata.Domain.DTO
{
    public class QuantityForDiscountPricingRule<T> : IPricing
    {
        public T SKU { get; set; }
        public int Current { get; set; }
        public int TotalQuantity { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public double GetDiscountPricing()
        {
            double discountUnit = (double)rulesToApplyy[i].Discount() / rulesToApplyy[i].Discount()//rulesToApplyy[i].Quantity;

        }

        private static void ApplyDiscountPerRule(List<Order<char>> orders, QuantityForDiscountPricingRule<char>[] rulesToApplyy, int i)
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
    }

}
