namespace pathly_backend.IAM.Application.Internal.DTOs

{
    public class AuthResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}