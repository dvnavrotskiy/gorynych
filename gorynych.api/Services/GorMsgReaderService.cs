using gorynych.api.Contracts;
using gorynych.api.Queries;
using MediatR;

namespace gorynych.api.Services;

public class GorMsgReaderService(IMediator mediator) : IGorMsgReader
{
    public async Task<MessagesResponse> GetMessages(Paging request, CancellationToken ct = default)
    {
        var count = await mediator.Send(new CountMessagesQuery(), ct);

        request.Normalize(count);
        
        var items = await mediator.Send(new ListMessagesQuery(request), ct);
        return new MessagesResponse
        {
            Messages = items,
            TotalCount = count,
            Paging = request
        };
    }
}