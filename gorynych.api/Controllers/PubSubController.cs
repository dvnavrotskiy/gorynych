using System.Diagnostics;
using gorynych.api.Contracts;
using gorynych.api.Services;
using gorynych.mq;
using gorynych.mq.Publishers;
using Microsoft.AspNetCore.Mvc;

namespace gorynych.api.Controllers;

[ApiController, Route("api/pubsub")]
public class PubSubController(
    ILogger<PubSubController> logger,
    SimplePublisher simplePublisher,
    AdvancedPublisher advancedPublisher,
    GorMsgService service
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
        
        await simplePublisher.Publish(new GorMsg {Message = message, Timestamp = DateTimeOffset.UtcNow}, ct);

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
        
        await advancedPublisher.Publish(new GorMsg {Message = message, Timestamp = DateTimeOffset.UtcNow}, ct);

        return Ok();
    }

    /// <summary>
    /// Прочитать полученное из очереди
    /// </summary>
    /// <returns>Коллекция сообщений</returns>
    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery]Paging request, CancellationToken ct)
    {
        var result = await service.GetMessages(request, ct);
        return Ok(result);
    }
}