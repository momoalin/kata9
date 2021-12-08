using System;
namespace Kata.Core.Abstractions
{
    public interface IPricingStrategy
    {
        public double ApplyDiscountPerRule();
    }
}
