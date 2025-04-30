using gorynych.api.Contracts;
using gorynych.api.Dal;
using gorynych.mq;

namespace gorynych.api.Services;

public class GorMsgService(IMessageRepo repo) : IGorMsgWriter
{

    public async Task Write(GorMsg message, CancellationToken ct = default)
    {
        await repo.Write(message, ct);
    }
    
    public async Task<MessagesResponse> GetMessages(Paging request, CancellationToken ct = default)
    {
        var count = await repo.Count(ct);

        request.Normalize(count);
        
        var items = await repo.GetMessages(request, ct);
        return new MessagesResponse
        {
            Messages = items,
            TotalCount = count,
            Paging = request
        };
    }
}

