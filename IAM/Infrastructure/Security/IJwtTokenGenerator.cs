namespace pathly_backend.IAM.Infrastructure.Security

{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId, string email);
    }
}