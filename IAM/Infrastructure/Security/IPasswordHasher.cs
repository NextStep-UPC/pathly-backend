namespace pathly_backend.IAM.Infrastructure.Security

{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string inputPassword);
    }
}