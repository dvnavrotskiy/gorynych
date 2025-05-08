#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
using System.Collections.Concurrent;
using gorynych.api.Contracts;
using gorynych.mq;

namespace gorynych.api.Dal;

public class InMemoryMessageRepo : IMessageRepo
{
    private readonly ConcurrentBag<GorMsg> messages = [];

    public async Task Write(GorMsg message, CancellationToken ct = default)
    {
        messages.Add(message);
    }
    
    public async Task<IList<GorMsg>> GetMessages(Paging request, CancellationToken cancellationToken = default)
    {
        return messages
            .Skip(request.PageSize * (request.Page - 1))
            .Take(request.PageSize)
            .ToList();
    }

    public async Task<int> Count(CancellationToken ct = default)
    {
        return messages.Count;
    }
}