using gorynych.auth.Dal;

namespace gorynych.auth.Services;

public class LoginService(ILoginRepo repo)
{
    public async Task<bool> Login(LoginRequest request)
    {
        return await repo.Login(request.Username, Password.GetSha256Hash(request.Password));
    }   
}