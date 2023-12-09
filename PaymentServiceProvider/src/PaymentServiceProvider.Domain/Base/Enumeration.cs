namespace PaymentServiceProvider.Domain.Base
{
    public abstract class Enumeration<T>
    {
        public T Id { get; protected set; }
        public string Description { get; protected set; }

        protected Enumeration(T id, string description)
        {
            this.Id = id;
            this.Description = description;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Enumeration<T> enumeration)
                return false;

            bool isSameType = GetType().Equals(obj.GetType());
            bool hasSameId = Id!.Equals(enumeration.Id);
            return isSameType && hasSameId;
        }

        public override string ToString()
        {
            return Description;
        }

        public override int GetHashCode()
        {
            return Id!.GetHashCode();
        }
    }
}
