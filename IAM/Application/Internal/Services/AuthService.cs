using Microsoft.EntityFrameworkCore;

using pathly_backend.IAM.Application.Internal.DTOs;
using pathly_backend.IAM.Application.Internal.Interfaces;
using pathly_backend.IAM.Domain.Model.Entities;
using pathly_backend.IAM.Infrastructure.Persistence.Context;
using pathly_backend.IAM.Infrastructure.Security;

namespace pathly_backend.IAM.Application.Internal.Services

{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(
            ApplicationDbContext context,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
                throw new Exception("Passwords do not match.");

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                throw new Exception("User already exists.");

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Birthdate = request.Birthdate,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);

            return new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email,
                Token = token
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
                throw new Exception("Invalid email or password.");

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);

            return new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email,
                Token = token
            };
        }
    }
}