
namespace Domain.Models
{
    public interface IUserSevice
    {
        Task<User> ValidateUserAsync(string email, string password);
    }
}
