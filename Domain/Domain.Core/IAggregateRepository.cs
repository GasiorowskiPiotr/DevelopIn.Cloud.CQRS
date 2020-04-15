using System.Threading.Tasks;
using DevelopIn.Cloud.CQRS.Domain.Common;

namespace DevelopIn.Cloud.CQRS.Domain.Core
{
    public interface IAggregateRepository
    {
        Task<TAggregate> GetById<TAggregate, TAggregateIdentity>(TAggregateIdentity id)
            where TAggregate : AggregateRoot<TAggregateIdentity> , new()
            where TAggregateIdentity : IAggregateIdentity;

        Task Store<TAggregate, TAggregateIdentity>(TAggregate aggregateRoot)
            where TAggregate : AggregateRoot<TAggregateIdentity> 
            where TAggregateIdentity : IAggregateIdentity;
    }
}