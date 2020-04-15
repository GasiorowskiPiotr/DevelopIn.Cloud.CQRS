using System;
using System.Threading.Tasks;

namespace DevelopIn.Cloud.CQRS.Domain.Core
{
    public interface IIdempotencyCheck
    {
        Task<bool> IsProcessed(Guid guid);
        Task MarkProcessed(Guid guid);
    }
}
