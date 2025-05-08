using gorynych.api.Contracts;
using gorynych.mq;

namespace gorynych.api.Dal;

public interface IMessageRepo
{
    Task Write(GorMsg message, CancellationToken ct = default);
    Task<IList<GorMsg>> GetMessages(Paging request, CancellationToken ct = default);
    Task<int> Count(CancellationToken ct = default);
}