using ManageLogFile.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ManageLogFile.Data.Context
{
    public class LogFileContext(DbContextOptions<LogFileContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
