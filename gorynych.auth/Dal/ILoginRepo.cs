namespace gorynych.auth.Dal;

public interface ILoginRepo
{
    Task<bool> Login(string username, byte[] passwordHash);
}