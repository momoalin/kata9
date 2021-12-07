using System.Collections;
using System.Collections.Generic;

namespace Kata.Core.Abstractions
{
    public interface IPriceEngine<T>
    {
        IList<IPricing> ListOfPricing { get; set; }

        public int GetTotal(T order);
    }
}