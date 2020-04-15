using System.Collections.Generic;
using System.Reflection;
using DevelopIn.Cloud.CQRS.Domain.Common;

namespace DevelopIn.Cloud.CQRS.Domain.Core
{
    public abstract class AggregateRoot<TAggregateIdentity> where TAggregateIdentity : IAggregateIdentity
    {
        public TAggregateIdentity Id { get; set; }
        public IEnumerable<Event<TAggregateIdentity>> UncommitedEvents => _uncommitedEvents;
        
        private readonly IList<Event<TAggregateIdentity>> _uncommitedEvents = new List<Event<TAggregateIdentity>>();

        protected AggregateRoot()
        {
        }

        public void PlayEvent(Event<TAggregateIdentity> @event, bool isUncommited = false)
        {
            GetType().InvokeMember(
                $"Apply{@event.GetType().Name}", 
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, 
                null,
                this, 
                new object[] { @event }
            );
            Id = @event.AggregateIdentity;

            if (isUncommited)
            {
                _uncommitedEvents.Add(@event);
            }
        }
    }
}