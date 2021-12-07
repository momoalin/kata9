using System.Collections.Generic;
using Kata.Core.Abstractions;
using Kata.Domain.Abstractions;
using Kata.Domain.DTO;

namespace Kata.Domain.Implementation
{
    public class CharSkusOrderParser : IOrderParser<char, string>
    {
        public string Order { get; set; }
        public IPriceOfSkuProvider<char> PriceOfSkuProvider { get; set; }
        public CharSkusOrderParser(IPriceOfSkuProvider<char> priceOfSkuProvider)
        {
            PriceOfSkuProvider = priceOfSkuProvider;
        }
        public List<Order<char>> ParseOrder(string order)
        {
            char[] skus = order.ToCharArray();
            List<Order<char>> orders = new List<Order<char>>();
            for (int i = 0; i < skus.Length; i++)
            {
                var price = PriceOfSkuProvider.GetPrice(skus[i]);//what if not found?
                //give up business rules for discount, processed list
                //set order price according to discount applied
                //whichever rule(by x get n free, by x for n price, buy x per metric) w/conversion
                orders.Add(new Order<char>()
                {
                    Sku = skus[i],
                    Price = price
                });
            }
            return orders;
        }
    }
}
