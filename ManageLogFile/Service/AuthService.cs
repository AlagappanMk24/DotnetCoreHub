using AutoMapper;
using ManageLogFile.Dtos;
using ManageLogFile.Model.Entities;
using ManageLogFile.Repositories.Interface;
using ManageLogFile.Service.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ManageLogFile.Service
{
    public class AuthService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;
        public async Task<User> AddUserAsync(SignupRequestDto signupRequest)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(signupRequest.Email);
            if (existingUser != null)
            {
                throw new Exception("A user with this email already exists.");
            }

            var user = _mapper.Map<User>(signupRequest);
            return await _userRepository.AddUserAsync(user);
        }

        public async Task<string> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginRequestDto.Email);
            if (user == null || user.Password != loginRequestDto.Password)
            {
                throw new Exception("Invalid credentials");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["JwtSettings:Subject"]),
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("Email", user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
