using ManageLogFile.Dtos;
using ManageLogFile.Model.Entities;

namespace ManageLogFile.Service.Interface
{
    public interface IAuthService
    {
        Task<User> AddUserAsync(SignupRequestDto signupRequest);
        Task<string> LoginAsync(LoginRequestDto loginRequestDto);
    }
}

