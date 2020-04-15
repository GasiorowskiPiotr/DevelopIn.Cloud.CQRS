using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevelopIn.Cloud.CQRS.Domain.Common;

namespace DevelopIn.Cloud.CQRS.Domain.Core
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly IDictionary<string, IList<Event>> _events = new Dictionary<string, IList<Event>>();

        public Task<IEnumerable<Event<TAggregateIdentity>>> GetEvents<TAggregateIdentity>(TAggregateIdentity aggregateIdentity) where TAggregateIdentity : IAggregateIdentity
        {
            if (_events.ContainsKey(aggregateIdentity.GetStringValue()))
            {
                return Task.FromResult(_events[aggregateIdentity.GetStringValue()].OfType<Event<TAggregateIdentity>>()
                    .AsEnumerable());
            }

            return Task.FromResult(Enumerable.Empty<Event<TAggregateIdentity>>());
        }

        public Task StoreEvents<TAggregateIdentity>(IEnumerable<Event<TAggregateIdentity>> events) where TAggregateIdentity : IAggregateIdentity
        {
            foreach (var @event in events)
            {
                if (!_events.ContainsKey(@event.AggregateIdentity.GetStringValue()))
                {
                    _events.Add(@event.AggregateIdentity.GetStringValue(), new List<Event>());
                }

                _events[@event.AggregateIdentity.GetStringValue()].Add(@event);
            }

            return Task.CompletedTask;
        }
    }
}