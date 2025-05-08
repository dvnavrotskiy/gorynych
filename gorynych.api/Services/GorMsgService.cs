using gorynych.api.Commands;
using gorynych.api.Contracts;
using gorynych.api.Queries;
using gorynych.mq;
using MediatR;

namespace gorynych.api.Services;

public class GorMsgService(IMediator mediator) : IGorMsgWriter
{
    public async Task Write(GorMsg message, CancellationToken ct = default)
    {
        await mediator.Send(new WriteMessageCommand(message), ct);
    }
    
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

