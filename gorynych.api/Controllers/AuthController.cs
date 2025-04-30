using gorynych.auth;
using gorynych.auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace gorynych.api.Controllers;

/// <summary>
/// Контроллер для логина
/// </summary>
/// <param name="loginService"></param>
[Route("api/auth"), ApiController]
public class AuthController(LoginService loginService) : ControllerBase
{
    /// <summary>
    /// Логин
    /// </summary>
    /// <param name="loginRequest">Логин и пароль</param>
    /// <returns>Код</returns>
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequest loginRequest)
    {
        var loginResponse = await loginService.Login(loginRequest);
        return loginResponse ? Ok() : Unauthorized();
    }
}