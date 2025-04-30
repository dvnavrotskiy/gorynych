using gorynych.mq;

namespace gorynych.api.Contracts;

public sealed record MessagesResponse
{
    public IList<GorMsg> Messages { get; init; }
    public int TotalCount { get; init; }
    
    public Paging Paging { get; init; }
}