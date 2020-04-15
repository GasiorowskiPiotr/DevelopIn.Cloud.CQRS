namespace DevelopIn.Cloud.CQRS.Domain.Common
{
    public abstract class Command<TAggregateIdentity> : Command where TAggregateIdentity : IAggregateIdentity
    {
        public TAggregateIdentity AggregateIdentity { get; set; }

        protected Command()
        {
        }

        protected Command(TAggregateIdentity aggregateIdentity)
        {
            AggregateIdentity = aggregateIdentity;
        }
    }

    public abstract class Command : Message
    {
    }
}