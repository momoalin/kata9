using System;
using System.Collections.Generic;
using System.Linq;
using Kata.Domain.DTO;
using Kata.Core.Abstractions;
using Kata.Domain.Abstractions;

namespace Kata.Domain.Implementation
{
    public class PlainTextOrdersPricingEngine : IPriceEngine
    {
        public PlainTextOrdersPricingEngine()
        {
            ListOfPricing = new List<IPricingStrategy>();
        }
        public List<QuantityForDiscount> Rules { get; }
        public IList<IPricingStrategy> ListOfPricing { get; set; }
        public void AddPricing(IPricingStrategy pricingStrategy)
        {
            ListOfPricing.Add(pricingStrategy);
        }
        public double GetTotal()
        {
            double total = 0;
            for (int i = 0; i < ListOfPricing.Count; i++)
            {
                total =+ ListOfPricing[i].ApplyDiscountPerRule();// needs testing with multiple strategies - 'total' concept incompatible with multiple strategies
            }
            return total;
        }

    }
}
