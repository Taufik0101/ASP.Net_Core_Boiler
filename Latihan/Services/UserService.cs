using System.Net.Mail;
using Latihan.Data;
using Latihan.DTO;
using Latihan.Models;
using Latihan.Utils;
using Microsoft.EntityFrameworkCore;

namespace Latihan.Services;

public interface IUserService
{
    Task<List<User>> GetUser();
    Task<User> GetUserById(Guid id);
    Task<User> CreateUser(Register addUserRequest);
    Task<User> UpdateUser(Guid id, AddUserRequest updateUserRequest);
    Task<bool> DeleteUser(Guid id);
    Task<User?> GetUserByEmail(string email);
    bool ValidateEmail(string email);
}

public class UserService : IUserService
{
    private readonly DataContext _context;

    public UserService(
        DataContext context
        )
    {
        _context = context;
    }

    public async Task<List<User>> GetUser()
    {
        return await _context.User.Where(
            u => u.DeletedAt == null
        ).ToListAsync();
    }

    public async Task<User> GetUserById(Guid id)
    {
        var user = await _context.User.FindAsync(id);
        return user ?? throw new KeyNotFoundException("User Not Found");
    }

    public async Task<User> CreateUser(Register addUserRequest)
    {
        var validEmail = ValidateEmail(addUserRequest.Email);
        if (!validEmail)
        {
            throw new ApplicationException("Email Not Valid");
        }
        
        var userEmail = await GetUserByEmail(addUserRequest.Email);

        if (userEmail != null)
        {
            throw new BadHttpRequestException("Email Already Used");
        }

        var user = new User()
        {
            Id = Guid.NewGuid(),
            Email = addUserRequest.Email,
            Name = addUserRequest.Name,
            Password = addUserRequest.Password,
            Role = addUserRequest.Role,
            CreatedAt = DateTime.Now,
            UpdatedAt = null,
            DeletedAt = null,
        };
            
        await _context.User.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateUser(Guid id, AddUserRequest updateUserRequest)
    {
        var userEmail = await GetUserByEmail(updateUserRequest.Email);

        if (userEmail != null && userEmail.Id != id)
        {
            throw new BadHttpRequestException("Email Already Used");
        }

        var userFind = await GetUserById(id);
        userFind.Email = updateUserRequest.Email;
        userFind.Name = updateUserRequest.Name;
        userFind.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return userFind;

    }

    public async Task<bool> DeleteUser(Guid id)
    {
        var userFind = await GetUserById(id);

        userFind.DeletedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await _context.User.Where(
            u => u.Email == email 
                 && u.DeletedAt == null
        ).FirstOrDefaultAsync();
        return user;
    }

    public bool ValidateEmail(string email)
    {
        var valid = true;
            
        try { 
            var emailAddress = new MailAddress(email);
        }
        catch {
            valid = false;
        }

        return valid;
    }
}