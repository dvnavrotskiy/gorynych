using gorynych.api.Commands;
using gorynych.api.Contracts;
using gorynych.api.Services;
using gorynych.mq;
using MediatR;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1573 // For CancellationToken
// Parameter has no matching param tag in the XML comment (but other parameters do)

namespace gorynych.api.Controllers;

[ApiController, Route("api/pubsub")]
public class PubSubController(
    ILogger<PubSubController> logger,
    IMediator mediator,
    IGorMsgReader msgReader
    )
    : ControllerBase
{
    /// <summary>
    /// Опубликовать сообщение
    /// </summary>
    /// <param name="message">Текст</param>
    /// <returns>200</returns>
    [HttpPost("publish")]
    public async Task<IActionResult> Publish(string message, CancellationToken ct)
    {
        logger.LogInformation($"Publish message: {message}");

        await mediator.Send(
            new PublishMessageCommand(
                new GorMsg { Message = message, Timestamp = DateTimeOffset.UtcNow }
            ),
            ct
        );

        return Ok();
    }
    
    /// <summary>
    /// Опубликовать сообщение в расширенном режиме
    /// </summary>
    /// <param name="message">Текст</param>
    /// <returns>200</returns>
    [HttpPost("advanced_publish")]
    public async Task<IActionResult> AdvancedPublish(string message, CancellationToken ct)
    {
        logger.LogInformation($"Advanced publish message: {message}");

        await mediator.Send(
            new AdvancedPublishMessageCommand(
                new GorMsg { Message = message, Timestamp = DateTimeOffset.UtcNow }
            ),
            ct
        );
        return Ok();
    }

    /// <summary>
    /// Прочитать полученное из очереди
    /// </summary>
    /// <returns>Коллекция сообщений</returns>
    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery]Paging request, CancellationToken ct)
    {
        var result = await msgReader.GetMessages(request, ct);
        return Ok(result);
    }
}