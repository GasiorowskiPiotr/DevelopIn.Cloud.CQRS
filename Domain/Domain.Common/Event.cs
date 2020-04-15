namespace DevelopIn.Cloud.CQRS.Domain.Common
{
    public abstract class Event : Message
    {
    }

    public abstract class Event<TAggregateIdentity> : Event where TAggregateIdentity : IAggregateIdentity
    {
        public TAggregateIdentity AggregateIdentity { get; set; }

        protected Event()
        {
        }

        protected Event(TAggregateIdentity aggregateIdentity)
        {
            AggregateIdentity = aggregateIdentity;
        }
    }
}