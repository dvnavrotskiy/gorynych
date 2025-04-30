using System.Security.Cryptography;
using System.Text;

namespace gorynych.auth.Services;

public static class Password
{
    public static void HashPassword(string password)
    {
        var h = GetSha256Hash(password);

        Console.WriteLine(BitConverter.ToString(h).Replace("-", string.Empty));
    }
    
    public static byte[] GetSha256Hash(string data)
    {
        return SHA256.HashData(Encoding.UTF8.GetBytes(data));
    }
}