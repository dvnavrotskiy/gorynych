using EasyNetQ;
using EasyNetQ.Topology;
using gorynych.common;
using Microsoft.Extensions.Logging;

namespace gorynych.mq.Subscribers;

public class AdvancedSubscriber(ILogger<AdvancedSubscriber> logger, IBus bus, IGorMsgWriter writer)
{
    public void Subscribe()
    {
        var exchange = bus.Advanced.ExchangeDeclare("gorynych.api.advanced.x", ExchangeType.Topic);
        var queue = bus.Advanced.QueueDeclare("gorynych.api.advanced.q");
        bus.Advanced.Bind(exchange, queue, "");
        bus.Advanced.Consume<GorMsg>(queue, (m, _) => Handle(m.Body));
    }
    
    private void Handle(GorMsg msg)
    {
        using var scope = logger.BeginScope(
            new LogDictionary<string, object> { [StringConstants.XRequestId] = msg.RequestId }
        );
        logger.LogInformation($"Advanced Handle message: {msg}");
        writer.Write(msg, CancellationToken.None).GetAwaiter().GetResult();
    }
}