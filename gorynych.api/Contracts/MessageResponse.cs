using gorynych.mq;

namespace gorynych.api.Contracts;

public sealed record MessagesResponse
{
    public required IList<GorMsg> Messages { get; init; }
    public int TotalCount { get; init; }
    
    public required Paging Paging { get; init; }
}