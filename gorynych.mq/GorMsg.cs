namespace gorynych.mq;

public class GorMsg
{
    public Guid RequestId { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Message { get; set; }
}