using ManageLogFile.Model.Entities;

namespace ManageLogFile.Repositories.Interface
{
    public interface IEmployeeRepository
    {
        Task<Employee> AddEmployee(Employee employee);
        Task<bool> DeleteEmployee(int id);
        Task<List<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeeById(int id);
        Task<Employee> UpdateEmployee(Employee employee);
    }
}