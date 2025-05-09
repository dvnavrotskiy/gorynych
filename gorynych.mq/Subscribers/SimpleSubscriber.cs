using EasyNetQ;
using gorynych.common;
using Microsoft.Extensions.Logging;

namespace gorynych.mq.Subscribers;

public class SimpleSubscriber(ILogger<SimpleSubscriber> logger, IBus bus, IGorMsgWriter writer)
{
    public void Subscribe()
    {
        bus.PubSub.Subscribe<GorMsg>("gorynych.simple.sub", Handle);
    }

    private void Handle(GorMsg msg)
    {
        using var scope = logger.BeginScope(
            new LogDictionary<string, object> { [StringConstants.XRequestId] = msg.RequestId }
        );
        logger.LogInformation($"Handle message: {msg}");
        writer.Write(msg, CancellationToken.None).GetAwaiter().GetResult();
    }
}