using ManageLogFile.Dtos;
using ManageLogFile.Model.Entities;
using ManageLogFile.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ManageLogFile.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("signup")]
        public async Task<ActionResult<User>> Signup([FromBody] SignupRequestDto signupRequest)
        {
            try
            {
                _logger.LogInformation("Attempting to sign up a new user with username: {UserName}", signupRequest.Name);

                var addedUser = await _authService.AddUserAsync(signupRequest);
                _logger.LogInformation("User successfully signed up. User ID: {UserId}, Name: {UserName}", addedUser.Id, addedUser.Name);

                return CreatedAtAction(nameof(Signup), new { id = addedUser.Id, name = addedUser.Name }, addedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while signing up user: {UserName}", signupRequest.Name);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequestDto loginModel)
        {
            try
            {
                _logger.LogInformation("Initiating login attempt for user with email: {Email}", loginModel.Email);

                var result = await _authService.LoginAsync(loginModel);
                _logger.LogInformation("User successfully logged in with email: {Email}", loginModel.Email);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login attempt for user: {Email}", loginModel.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("add-user")]
        public async Task<ActionResult<User>> AddUser([FromBody] SignupRequestDto newUser)
        {
            try
            {
                _logger.LogInformation("Attempting to add a new user with username: {UserName}", newUser.Name);

                var addedUser = await _authService.AddUserAsync(newUser);
                _logger.LogInformation("New user successfully added. User ID: {UserId}, Name: {UserName}", addedUser.Id, addedUser.Name);

                return CreatedAtAction(nameof(AddUser), new { id = addedUser.Id }, addedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user: {UserName}", newUser.Name);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
