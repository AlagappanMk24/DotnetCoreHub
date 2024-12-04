using ManageLogFile.Model.Entities;

namespace ManageLogFile.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
    }
}
