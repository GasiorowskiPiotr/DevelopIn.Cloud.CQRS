using System.Collections.Generic;
using System.Threading.Tasks;
using DevelopIn.Cloud.CQRS.Domain.Common;

namespace DevelopIn.Cloud.CQRS.Domain.Core
{
    public interface IEventStore
    {
        Task<IEnumerable<Event<TAggregateIdentity>>> GetEvents<TAggregateIdentity>(TAggregateIdentity aggregateIdentity)
            where TAggregateIdentity : IAggregateIdentity;

        Task StoreEvents<TAggregateIdentity>(IEnumerable<Event<TAggregateIdentity>> events) where TAggregateIdentity : IAggregateIdentity;
    }
}