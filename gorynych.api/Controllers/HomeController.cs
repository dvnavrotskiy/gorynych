using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace gorynych.api.Controllers;

/// <summary>
/// Информационный контроллер
/// </summary>
[ApiController, Route("/")]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> logger;

    private readonly string serviceInfo;

    public HomeController(
        IConfiguration          configuration,
        ILogger<HomeController> logger
    )
    {
        this.logger              = logger;

        var configName       = configuration["Name"];
        var serviceStartTime = Process.GetCurrentProcess().StartTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

        serviceInfo = $"gorynych.api ({configName}) on {Environment.MachineName} / {serviceStartTime}";
    }

    /// <summary>
    /// Базовая информационная строка сервиса
    /// </summary>
    /// <returns>Строка окружения, машины и времени запуска</returns>
    [HttpGet]
    public ActionResult Get()
    {
        return Ok(serviceInfo);
    }
    
    /// <summary>
    /// Базовая информационная строка сервиса с логированием
    /// </summary>
    /// <returns>Строка окружения, машины и времени запуска</returns>
    [HttpGet("/log")]
    public ActionResult Log()
    {
        logger.LogInformation(serviceInfo);

        return Ok(serviceInfo);
    }
    
    /// <summary>
    /// Тестовый метод
    /// </summary>
    /// <param name="callback">Строка в ответ</param>
    /// <returns>Развернутая строка из запроса</returns>
    [HttpGet("/test")]
    [SwaggerOperation(Summary = "Test method with parameter", Description = "Test method with string parameter")]
    [SwaggerResponse(200, "I guess everything worked")]
    [SwaggerResponse(400, "BAD REQUEST")]
    public ActionResult Test(string callback)
    {
        var charArray = callback.ToCharArray();
        Array.Reverse(charArray);
        callback = new string(charArray);
        
        return Ok(callback);
    }

    [HttpGet("/fail")]
    public ActionResult Fail()
    {
        throw new Exception("Method failed");
    }
}