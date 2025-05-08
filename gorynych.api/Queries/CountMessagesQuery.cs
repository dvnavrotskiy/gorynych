using gorynych.api.Dal;
using MediatR;

namespace gorynych.api.Queries;

public record CountMessagesQuery : IRequest<int>;

public class CountMessagesQueryHandler(IMessageRepo repo) : IRequestHandler<CountMessagesQuery, int>
{
    public async Task<int> Handle(CountMessagesQuery request, CancellationToken ct)
    {
        return await repo.Count(ct);
    }
}
