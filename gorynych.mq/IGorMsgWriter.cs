namespace gorynych.mq;

public interface IGorMsgWriter
{
    Task Write(GorMsg message, CancellationToken cancellationToken = default);
}