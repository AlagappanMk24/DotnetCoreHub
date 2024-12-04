using ManageLogFile.Repositories.Interface;
using ManageLogFile.Repositories;
using ManageLogFile.Service.Interface;
using ManageLogFile.Service;

namespace ManageLogFile
{
    public static class ServiceDependencies
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        }
    }
}
