public sealed class BcryptPasswordHasher : IPasswordHasher
{
    public string Hash(string plainTextPassoword)
    {
        return BCrypt.Net.BCrypt.HashPassword(plainTextPassoword);
    }

    public bool Verify(string plainTextPassoword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainTextPassoword, hashedPassword);
    }
}