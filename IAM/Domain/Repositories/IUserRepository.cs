﻿using IAM.Domain.Entities;

namespace IAM.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task<bool> EmailExistsAsync(string email);
}