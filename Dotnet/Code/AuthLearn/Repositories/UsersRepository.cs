using AuthLearn.Database;
using AuthLearn.Entities;
using AuthLearn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthLearn.Repositories;

public interface IUsersRepository
{
    public Task<IEnumerable<User>> GetUsersAsync();
    public Task<User?> GetUser(string id);

    public Task<User?> GetUserByName(string name);
    public Task<User> RegisterUserAsync(UserModel model);
    public bool VerifyPassword(User user, string password);
}

public class UsersRepository : IUsersRepository
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UsersRepository(AppDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetUserByName(string name)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
    }

    public async Task<User?> GetUser(string id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> RegisterUserAsync(UserModel model)
    {
        return await CreateUserAsync(model);
    }

    private async Task<User> CreateUserAsync(UserModel model)
    {
        //Map User from Model
        var newUser = new User()
        {
            Name = model.Name,
            Email = model.Email,
            DateOfBirth = model.DateOfBirth.ToString("yyyy/MM/dd")
        };

        //Hash Password
        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, model.Password);

        //Save to InMemory DB
        var userEntity = await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return userEntity.Entity;
    }

    public bool VerifyPassword(User user, string password)
    {
        return _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) ==
               PasswordVerificationResult.Success;
    }
}