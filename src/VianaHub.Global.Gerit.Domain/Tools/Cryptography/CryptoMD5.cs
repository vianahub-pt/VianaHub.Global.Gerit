using System.Security.Cryptography;
using System.Text;

namespace VianaHub.Global.Gerit.Domain.Tools.Cryptography;

public static class CryptoMD5
{
    public static string Encrypt(string input)
    {
        using MD5 md5Hash = MD5.Create();
        return GenerateHash(md5Hash, input);
    }

    private static string GenerateHash(MD5 md5Hash, string input)
    {
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        StringBuilder sBuilder = new();

        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        return sBuilder.ToString();
    }

    public static bool Verify(string input, string hash)
    {
        string hashOfInput = Encrypt(input);

        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        return comparer.Compare(hashOfInput, hash) == 0;
    }
}

