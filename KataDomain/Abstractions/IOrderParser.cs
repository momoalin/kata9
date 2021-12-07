using System.Collections.Generic;
using Kata.Domain.DTO;

namespace Kata.Domain.Abstractions
{
    public interface IOrderParser<SKU,T>//generic probably overkill
    {
        public List<Order<SKU>> ParseOrder(T sku);
    }
}