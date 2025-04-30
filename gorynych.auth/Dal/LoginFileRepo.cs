using Newtonsoft.Json;

namespace gorynych.auth.Dal;

public sealed class LoginFileRepo(string connectionString) : ILoginRepo
{
    public async Task<bool> Login(string username, byte[] passwordHash)
    {
        var s = await File.ReadAllTextAsync(connectionString);
        var users = JsonConvert.DeserializeObject<List<UserDto>>(s);
        var phs = BitConverter.ToString(passwordHash).Replace("-", string.Empty);
        return users?.Any(
            x => x.Username == username && string.Equals(x.Password, phs, StringComparison.OrdinalIgnoreCase)
        ) ?? false;
    }
}

public sealed record UserDto
{
    public string Username { get; init; }
    public string Password { get; init; }
}