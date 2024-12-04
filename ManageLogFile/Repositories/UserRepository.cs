using ManageLogFile.Data.Context;
using ManageLogFile.Model.Entities;
using ManageLogFile.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ManageLogFile.Repositories
{
    public class UserRepository(LogFileContext context) : IUserRepository
    {
        private readonly LogFileContext _context = context;
        public async Task<User> AddUserAsync(User user)
        {
            var addedUser = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return addedUser.Entity;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}
