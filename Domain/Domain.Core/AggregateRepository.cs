using System.Threading.Tasks;
using DevelopIn.Cloud.CQRS.Domain.Common;

namespace DevelopIn.Cloud.CQRS.Domain.Core
{
    public class AggregateRepository : IAggregateRepository
    {
        private readonly IEventStore _eventStore;

        public AggregateRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<TAggregate> GetById<TAggregate, TAggregateIdentity>(TAggregateIdentity id) where TAggregate : AggregateRoot<TAggregateIdentity>, new() where TAggregateIdentity : IAggregateIdentity
        {
            var events = await _eventStore.GetEvents(id).ConfigureAwait(false);

            var aggregate = new TAggregate();

            foreach (var @event in events)
            {
                aggregate.PlayEvent(@event);
            }

            return aggregate;
        }

        public async Task Store<TAggregate, TAggregateIdentity>(TAggregate aggregateRoot) where TAggregate : AggregateRoot<TAggregateIdentity> where TAggregateIdentity : IAggregateIdentity
        {
            var events = aggregateRoot.UncommitedEvents;

            await _eventStore.StoreEvents(events).ConfigureAwait(false);
        }
    }
}