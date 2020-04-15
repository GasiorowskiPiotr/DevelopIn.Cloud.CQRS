using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevelopIn.Cloud.CQRS.Domain.Common;

namespace DevelopIn.Cloud.CQRS.Domain.Core
{
    public abstract class AggregateRootCommandHandler<TAggregateRoot, TAggregateIdentity>
        where TAggregateRoot : AggregateRoot<TAggregateIdentity>, new()
        where TAggregateIdentity : IAggregateIdentity   
    {
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IIdempotencyCheck _idempotencyCheck;

        protected AggregateRootCommandHandler(IAggregateRepository aggregateRepository, IIdempotencyCheck idempotencyCheck)
        {
            _aggregateRepository = aggregateRepository;
            _idempotencyCheck = idempotencyCheck;
        }

        protected async Task HandleCommand<TCommand>(TCommand command, Func<TAggregateRoot, TCommand, IEnumerable<Event<TAggregateIdentity>>> handler, Func<IEnumerable<Event<TAggregateIdentity>>, Task> dispatchEvents) where TCommand : Command<TAggregateIdentity>
        {
            var isAlreadyProcessed = await _idempotencyCheck.IsProcessed(command.Id).ConfigureAwait(false);
            if (isAlreadyProcessed)
            {
                return;
            }

            var aggregateRoot = await _aggregateRepository
                .GetById<TAggregateRoot, TAggregateIdentity>(command.AggregateIdentity).ConfigureAwait(false);

            var events = handler(aggregateRoot, command);

            foreach (var @event in events)
            {
                aggregateRoot.PlayEvent(@event, true);
            }

            await _aggregateRepository.Store<TAggregateRoot, TAggregateIdentity>(aggregateRoot).ConfigureAwait(false);
            await dispatchEvents(events).ConfigureAwait(false);

            await _idempotencyCheck.MarkProcessed(command.Id).ConfigureAwait(false);
        }

        protected async Task HandleCommand<TCommand>(TCommand command,
            Func<TAggregateRoot, TCommand, Task<IEnumerable<Event<TAggregateIdentity>>>> handler,
            Func<IEnumerable<Event<TAggregateIdentity>>, Task> dispatchEvents)
            where TCommand : Command<TAggregateIdentity>
        {
            var isAlreadyProcessed = await _idempotencyCheck.IsProcessed(command.Id).ConfigureAwait(false);
            if (isAlreadyProcessed)
            {
                return;
            }

            var aggregateRoot = await _aggregateRepository
                .GetById<TAggregateRoot, TAggregateIdentity>(command.AggregateIdentity).ConfigureAwait(false);

            var events = await handler(aggregateRoot, command).ConfigureAwait(false);

            foreach (var @event in events)
            {
                aggregateRoot.PlayEvent(@event);
            }

            await _aggregateRepository.Store<TAggregateRoot, TAggregateIdentity>(aggregateRoot).ConfigureAwait(false);
            await dispatchEvents(events).ConfigureAwait(false);
        }

    }
}