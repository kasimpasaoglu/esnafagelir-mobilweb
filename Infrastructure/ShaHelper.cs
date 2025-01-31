public static class ShaHelper
{
    public static string HashPassword(string password, out string salt)
    {
        using var sha512 = System.Security.Cryptography.SHA512.Create();

        // Rastgele salt oluştur
        var saltBytes = new byte[16];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);

        salt = Convert.ToBase64String(saltBytes);

        // Şifre + Salt'ı hashle
        var hashBytes = sha512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password + salt));
        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyPassword(string password, string salt, string hashedPassword)
    {
        using var sha512 = System.Security.Cryptography.SHA512.Create();
        var hashBytes = sha512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password + salt));
        return Convert.ToBase64String(hashBytes) == hashedPassword;
    }
}
