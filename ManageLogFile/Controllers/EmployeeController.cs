using ManageLogFile.Model.Entities;
using ManageLogFile.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManageLogFile.Controllers
{
    [Authorize]
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger) : ControllerBase
    {
        private readonly IEmployeeService _employeeService = employeeService;
        private readonly ILogger<EmployeeController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                _logger.LogInformation("Request received to fetch all employee details at {Time}.", DateTime.Now);
                var employees = await _employeeService.GetEmployeeDetails();

                if (employees == null || !employees.Any())
                {
                    _logger.LogWarning("No employees found in the system. Request timestamp: {Time}.", DateTime.Now);
                    return NotFound("No employees found.");
                }

                _logger.LogInformation("{EmployeeCount} employees successfully fetched. Request timestamp: {Time}.", employees.Count(), DateTime.Now);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching employee details.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                _logger.LogInformation("Request received to fetch details for employee ID: {EmployeeId} at {Time}.", id, DateTime.Now);
                var employee = await _employeeService.GetEmployeeDetails(id);

                if (employee == null)
                {
                    _logger.LogWarning("Employee with ID {EmployeeId} not found. Request timestamp: {Time}.", id, DateTime.Now);
                    return NotFound($"Employee with ID {id} not found.");
                }

                _logger.LogInformation("Successfully fetched details for employee ID: {EmployeeId}. Request timestamp: {Time}.", id, DateTime.Now);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching details for employee ID: {EmployeeId} at {Time}.", id, DateTime.Now);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            try
            {
                _logger.LogInformation("Request received to add new employee with name: {EmployeeName} at {Time}.", employee.Name, DateTime.Now);
                var addedEmployee = await _employeeService.AddEmployee(employee);

                _logger.LogInformation("Successfully added new employee with ID: {EmployeeId}. Employee name: {EmployeeName}. Request timestamp: {Time}.", addedEmployee.Id, employee.Name, DateTime.Now);
                return CreatedAtAction(nameof(GetEmployeeById), new { id = addedEmployee.Id }, addedEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding new employee with name: {EmployeeName} at {Time}.", employee.Name, DateTime.Now);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            try
            {
                if (id != employee.Id)
                {
                    _logger.LogWarning("Employee ID mismatch: URL ID ({UrlId}) does not match request body ID ({ObjectId}). Request timestamp: {Time}.", id, employee.Id, DateTime.Now);
                    return BadRequest("Employee ID mismatch.");
                }

                _logger.LogInformation("Request received to update employee with ID: {EmployeeId} at {Time}.", id, DateTime.Now);
                var updatedEmployee = await _employeeService.UpdateEmployee(employee);

                if (updatedEmployee == null)
                {
                    _logger.LogWarning("Employee with ID {EmployeeId} not found for update. Request timestamp: {Time}.", id, DateTime.Now);
                    return NotFound($"Employee with ID {id} not found.");
                }

                _logger.LogInformation("Successfully updated employee with ID: {EmployeeId}. Request timestamp: {Time}.", id, DateTime.Now);
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating employee ID: {EmployeeId} at {Time}.", id, DateTime.Now);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                _logger.LogInformation("Request received to delete employee with ID: {EmployeeId} at {Time}.", id, DateTime.Now);
                var result = await _employeeService.DeleteEmployee(id);

                if (!result)
                {
                    _logger.LogWarning("Employee with ID {EmployeeId} not found for deletion. Request timestamp: {Time}.", id, DateTime.Now);
                    return NotFound($"Employee with ID {id} not found.");
                }

                _logger.LogInformation("Successfully deleted employee with ID: {EmployeeId}. Request timestamp: {Time}.", id, DateTime.Now);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting employee with ID: {EmployeeId} at {Time}.", id, DateTime.Now);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}