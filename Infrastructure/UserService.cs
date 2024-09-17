using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Domain.Models;
using WebApplication6.Data;

public class UserService : IUserSevice
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile> ValidateUserAsync(string email, string password)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && (u.PasswordHash != null) && u.PasswordHash == password);
    }

    Task<User> IUserSevice.ValidateUserAsync(string email, string password)
    {
        throw new NotImplementedException();
    }
}