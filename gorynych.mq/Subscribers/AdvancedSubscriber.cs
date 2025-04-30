using EasyNetQ;
using EasyNetQ.Topology;

namespace gorynych.mq.Subscribers;

public class AdvancedSubscriber(IBus bus, IGorMsgWriter writer)
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
        writer.Write(msg, CancellationToken.None).GetAwaiter().GetResult();
    }
}