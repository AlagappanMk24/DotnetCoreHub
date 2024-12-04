using ManageLogFile.Service.Interface;
using ManageLogFile.Model.Entities;
using ManageLogFile.Repositories.Interface;

namespace ManageLogFile.Service
{
    public class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;

        public async Task<List<Employee>> GetEmployeeDetails()
        {
            return await _employeeRepository.GetAllEmployees();
        }

        public async Task<Employee> GetEmployeeDetails(int id)
        {
            return await _employeeRepository.GetEmployeeById(id);
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            return await _employeeRepository.AddEmployee(employee);
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            return await _employeeRepository.UpdateEmployee(employee);
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            return await _employeeRepository.DeleteEmployee(id);
        }
    }

}
