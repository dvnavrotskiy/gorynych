using gorynych.api.Contracts;
using gorynych.api.Dal;
using gorynych.mq;
using MediatR;

namespace gorynych.api.Queries;

public record ListMessagesQuery(Paging Paging) : IRequest<IList<GorMsg>>;

public class ListMessagesQueryHandler(IMessageRepo repo) : IRequestHandler<ListMessagesQuery, IList<GorMsg>>
{
    public async Task<IList<GorMsg>> Handle(ListMessagesQuery request, CancellationToken ct)
    {
        return await repo.GetMessages(request.Paging, ct);
    }
}