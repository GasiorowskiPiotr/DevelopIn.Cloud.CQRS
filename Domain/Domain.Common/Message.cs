using System;

namespace DevelopIn.Cloud.CQRS.Domain.Common
{
    public abstract class Message
    {
        public DateTimeOffset CreatedOn { get; set; }

        public Guid Id { get; set; }

        public Guid ProcessId { get; set; }

        protected Message()
        {
            CreatedOn = DateTimeOffset.UtcNow;
            Id = Guid.NewGuid();
            ProcessId = Guid.Empty;
        }
    }
}