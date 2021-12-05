namespace Kata.Core.Abstractions
{
    public interface IPriceEngine<T>//generic probably overkill
    {
        public int GetTotal(T order);
    }
}