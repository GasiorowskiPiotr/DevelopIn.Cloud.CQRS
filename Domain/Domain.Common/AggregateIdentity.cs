namespace DevelopIn.Cloud.CQRS.Domain.Common
{
    public abstract class AggregateIdentity<TKeyType> : IAggregateIdentity
    {
        public TKeyType Value { get; set; }

        protected AggregateIdentity() { }

        protected AggregateIdentity(TKeyType value)
        {
            Value = value;
        }

        public string GetStringValue() => $"{GetType().FullName}--{Value}";
    }
}