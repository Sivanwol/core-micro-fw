using System.Security.Cryptography;
namespace Application.Utils; 

public static class StringExtensions {
    public static string RandomString(int length) {
        var alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+-*!=";
        var outOfRange = byte.MaxValue + 1 - (byte.MaxValue + 1) % alphabet.Length;
        return string.Concat(
            Enumerable
                .Repeat(0, int.MaxValue)
                .Select(e => RandomByte())
                .Where(randomByte => randomByte < outOfRange)
                .Take(length)
                .Select(randomByte => alphabet[randomByte % alphabet.Length])
        );
    }
    
    private static byte RandomByte()
    {
        using (var randomizationProvider = new RNGCryptoServiceProvider())
        {
            var randomBytes = new byte[1];
            randomizationProvider.GetBytes(randomBytes);
            return randomBytes.Single();
        }   
    }
}