using gorynych.mq;
using gorynych.mq.Publishers;
using MediatR;

namespace gorynych.api.Commands;

public record PublishMessageCommand(GorMsg Message) : IRequest<GorMsg>;

public class PublishMessageHandler (SimplePublisher simplePublisher) : IRequestHandler<PublishMessageCommand, GorMsg>
{
    public async Task<GorMsg> Handle(PublishMessageCommand request, CancellationToken ct)
    {
        await simplePublisher.Publish(request.Message, ct);
        return request.Message;
    }
}

public record AdvancedPublishMessageCommand(GorMsg Message) : IRequest<GorMsg>;

public class AdvancedPublishMessageHandler (AdvancedPublisher advancedPublisher) : IRequestHandler<AdvancedPublishMessageCommand, GorMsg>
{
    public async Task<GorMsg> Handle(AdvancedPublishMessageCommand request, CancellationToken ct)
    {
        await advancedPublisher.Publish(request.Message, ct);
        return request.Message;
    }
}