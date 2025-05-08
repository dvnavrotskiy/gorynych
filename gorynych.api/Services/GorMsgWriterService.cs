using gorynych.api.Commands;
using gorynych.mq;
using MediatR;

namespace gorynych.api.Services;

public class GorMsgWriterService(IMediator mediator) : IGorMsgWriter
{
    public async Task Write(GorMsg message, CancellationToken ct = default)
    {
        await mediator.Send(new WriteMessageCommand(message), ct);
    }
}