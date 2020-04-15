using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevelopIn.Cloud.CQRS.Domain.Common;

namespace DevelopIn.Cloud.CQRS.Domain.Core
{
    public abstract class ProcessManagerEventHandler
    {
        protected IAggregateRepository Repository { get; }

        protected ProcessManagerEventHandler(IAggregateRepository aggregateRepository)
        {
            Repository = aggregateRepository;
        }

        protected async Task HandleEvent<TEvent>(TEvent @event, Func<TEvent, IEnumerable<Command>> handler, Func<IEnumerable<Command>, Task> dispatchCommands) where TEvent : Event
        {
            var commands = handler(@event);
            await dispatchCommands(commands);
        }

        protected async Task HandleEvent<TEvent>(TEvent @event, Func<TEvent, Task<IEnumerable<Command>>> handler,
            Func<IEnumerable<Command>, Task> dispatchCommands) where TEvent : Event
        {
            var commands = await handler(@event).ConfigureAwait(false);
            await dispatchCommands(commands);
        }
    }
}