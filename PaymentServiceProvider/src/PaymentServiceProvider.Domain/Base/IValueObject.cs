namespace PaymentServiceProvider.Domain.Base
{
    public interface IValueObject<T>
    {
        bool SameValueAs(T other);
    }
}
