using EasyNetQ;

namespace gorynych.mq;

public static class BusFactory
{
    public static IBus Create(string connectionString)
    {
        return RabbitHutch.CreateBus(connectionString);
    }
}