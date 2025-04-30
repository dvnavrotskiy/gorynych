using EasyNetQ;

namespace gorynych.mq.Subscribers;

public class SimpleSubscriber(IBus bus, IGorMsgWriter writer)
{
    public void Subscribe()
    {
        bus.PubSub.Subscribe<GorMsg>("gorynych.simple.sub", Handle);
    }

    private void Handle(GorMsg msg)
    {
        writer.Write(msg, CancellationToken.None).GetAwaiter().GetResult();
    }


}