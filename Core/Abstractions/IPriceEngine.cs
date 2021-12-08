using System.Collections;
using System.Collections.Generic;

namespace Kata.Core.Abstractions
{
    public interface IPriceEngine
    {
        IList<IPricingStrategy> ListOfPricing { get; set; }

        public double GetTotal();
    }
}