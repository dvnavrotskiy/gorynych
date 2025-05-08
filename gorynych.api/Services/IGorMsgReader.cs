using gorynych.api.Contracts;

namespace gorynych.api.Services;

public interface IGorMsgReader
{
    Task<MessagesResponse> GetMessages(Paging request, CancellationToken ct = default);
}