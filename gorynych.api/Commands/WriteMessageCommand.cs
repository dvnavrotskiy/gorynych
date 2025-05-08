using gorynych.api.Dal;
using gorynych.mq;
using MediatR;

namespace gorynych.api.Commands;

public record WriteMessageCommand(GorMsg Message) : IRequest<GorMsg>;

public class WriteMessageCommandHandler(IMessageRepo repo) : IRequestHandler<WriteMessageCommand, GorMsg>
{
    public async Task<GorMsg> Handle(WriteMessageCommand request, CancellationToken ct)
    {
        await repo.Write(request.Message, ct);
        return request.Message;
    }
}