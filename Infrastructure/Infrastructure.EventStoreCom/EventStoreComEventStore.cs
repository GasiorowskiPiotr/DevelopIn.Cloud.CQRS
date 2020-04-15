using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevelopIn.Cloud.CQRS.Domain.Common;
using DevelopIn.Cloud.CQRS.Domain.Core;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace DevelopIn.Cloud.CQRS.Infrastructure.EventStoreCom
{
    public class EventStoreComEventStore : IEventStore
    {
        private readonly EventStoreComSettings _settings;

        private IEventStoreConnection _connection;

        public EventStoreComEventStore(EventStoreComSettings settings)
        {
            _settings = settings;
        }

        public async Task<IEnumerable<Event<TAggregateIdentity>>> GetEvents<TAggregateIdentity>(TAggregateIdentity aggregateIdentity) where TAggregateIdentity : IAggregateIdentity
        {
            await EnsureConnected().ConfigureAwait(false);

            var events = await _connection
                .ReadStreamEventsForwardAsync(aggregateIdentity.GetStringValue(), StreamPosition.Start,
                    StreamPosition.End, false).ConfigureAwait(false);

            return events.Events.Select(e =>
                    JsonConvert.DeserializeObject(Encoding.UTF8.GetString(e.Event.Data),
                        Type.GetType(e.Event.EventType)))
                .OfType<Event<TAggregateIdentity>>()
                .ToList();
        }

        public async Task StoreEvents<TAggregateIdentity>(IEnumerable<Event<TAggregateIdentity>> events) where TAggregateIdentity : IAggregateIdentity
        {
            await EnsureConnected().ConfigureAwait(false);

            foreach (var @event in events)
            {
                await _connection.AppendToStreamAsync(@event.AggregateIdentity.GetStringValue(), ExpectedVersion.Any,
                        new EventData(@event.Id, @event.GetType().FullName, true,
                            Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)), new byte[0]))
                    .ConfigureAwait(false);
            }
        }

        private Task EnsureConnected()
        {
            if (_connection != null) 
                return Task.CompletedTask;
            
            _connection = EventStoreConnection.Create(new Uri(_settings.ConnectionString));
            _connection.Disconnected += delegate { _connection.ConnectAsync().GetAwaiter().GetResult();  };
            return _connection.ConnectAsync();
        }
    }
}
