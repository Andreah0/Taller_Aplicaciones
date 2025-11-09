using Company.Shared.DTOs;
using Company.Shared.Entities;
using Microsoft.AspNetCore.Identity;

namespace Company.Backend.Respositories.Interfaces;

public interface IUsersRepository
{
    Task<SignInResult> LoginAsync(LoginDTO model);

    Task LogoutAsync();

    Task<User> GetUserAsync(string email);

    Task<IdentityResult> AddUserAsync(User user, string password);

    Task CheckRoleAsync(string roleName);

    Task AddUserToRoleAsync(User user, string roleName);

    Task<bool> IsUserInRoleAsync(User user, string roleName);
}