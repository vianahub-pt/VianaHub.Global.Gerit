using System.Security.Cryptography;

namespace VianaHub.Global.Gerit.Domain.Tools.Cryptography;

public static class CryptoAES
{
    private static readonly byte[] Key = ConvertHexStringToByteArray("2B7E151628AED2A6ABF7158809CF4F3C");

    public static string Encrypt(string input)
    {
        using Aes aesAlg = Aes.Create();
        aesAlg.Key = Key;

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using MemoryStream msEncrypt = new();
        using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            using StreamWriter swEncrypt = new(csEncrypt);
            swEncrypt.Write(input);
        }

        byte[] encryptedBytes = msEncrypt.ToArray();

        return Convert.ToBase64String(encryptedBytes);
    }

    public static string Decrypt(string encryptedInput)
    {
        byte[] encryptedBytes = Convert.FromBase64String(encryptedInput);

        using Aes aesAlg = Aes.Create();
        aesAlg.Key = Key;

        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using MemoryStream msDecrypt = new(encryptedBytes);
        using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
        using StreamReader srDecrypt = new(csDecrypt);
        string decryptedText = srDecrypt.ReadToEnd();

        return decryptedText;
    }

    private static byte[] ConvertHexStringToByteArray(string hexString)
    {
        int length = hexString.Length;
        byte[] byteArray = new byte[length / 2];
        for (int i = 0; i < length; i += 2)
        {
            byteArray[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
        }
        return byteArray;
    }
}
