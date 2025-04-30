using EasyNetQ;

namespace gorynych.mq.Publishers;

public class SimplePublisher(IBus bus)
{
    public async Task Publish(GorMsg msg, CancellationToken ct)
    {
        await bus.PubSub.PublishAsync(msg, ct);
    }
}