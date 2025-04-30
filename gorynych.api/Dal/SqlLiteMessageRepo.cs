using System.Globalization;
using Dapper;
using gorynych.api.Contracts;
using gorynych.api.Services;
using gorynych.mq;
using Microsoft.Data.Sqlite;

namespace gorynych.api.Dal;

public class SqlLiteMessageRepo(string connectionString) : IMessageRepo
{
    public async Task Write(GorMsg message, CancellationToken ct = default)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(ct);
        await connection.ExecuteAsync(
            "INSERT INTO Messages (Message, Timestamp) VALUES (@Message, @Timestamp)",
            new {message.Message, Timestamp = message.Timestamp.ToUniversalTime()}
            );
    }

    public async Task<IList<GorMsg>> GetMessages(Paging request, CancellationToken ct = default)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(ct);
        var result = await connection.QueryAsync<MessageDto>(
            "SELECT Timestamp, Message FROM Messages LIMIT @Limit OFFSET @Offset",
            new
            {
                Limit = request.PageSize,
                Offset = request.PageSize * (request.Page - 1)
            });
            
            return result.Select(x => new GorMsg
                {
                    Message = x.Message,
                    Timestamp = DateTimeOffset.ParseExact(
                        x.Timestamp,
                        //2025-04-30 08:54:30.4418906+00:00
                        "yyyy-MM-dd HH:mm:ss.fffffffzzz",
                        CultureInfo.InvariantCulture
                    )
                })
            .ToList();
    }

    public async Task<int> Count(CancellationToken ct = default)
    {
        await using var connection = new SqliteConnection(connectionString);
        await connection.OpenAsync(ct);
        return await connection.ExecuteScalarAsync<int>("SELECT Count(1) FROM Messages");
    }

    private class MessageDto
    {
        public string Message { get; set; }
        public string Timestamp { get; set; }
    }
}