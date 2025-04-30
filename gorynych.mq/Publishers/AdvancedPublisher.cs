using EasyNetQ;
using EasyNetQ.Topology;

namespace gorynych.mq.Publishers;

public class AdvancedPublisher(IBus bus)
{
    private readonly Exchange exchange = bus.Advanced.ExchangeDeclare("gorynych.api.advanced.x", ExchangeType.Topic);

    public async Task Publish(GorMsg msg, CancellationToken ct)
    {
        await bus.Advanced.PublishAsync(
            exchange: exchange,
            routingKey: "",
            mandatory: true,
            new Message<GorMsg>(msg),
            ct
        );
    }
}