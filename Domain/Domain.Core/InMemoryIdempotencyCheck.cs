using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevelopIn.Cloud.CQRS.Domain.Core
{
    public class InMemoryIdempotencyCheck : IIdempotencyCheck
    {
        private readonly HashSet<Guid> _processedMessages = new HashSet<Guid>();

        public Task<bool> IsProcessed(Guid guid)
        {
            return Task.FromResult(_processedMessages.Contains(guid));
        }

        public Task MarkProcessed(Guid guid)
        {
            _processedMessages.Add(guid);

            return Task.CompletedTask;
        }
    }
}