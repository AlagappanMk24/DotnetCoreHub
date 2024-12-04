using ManageLogFile.Model.Entities;

namespace ManageLogFile.Service.Interface
{
    public interface IEmployeeService
    {
        Task<Employee> AddEmployee(Employee employee);
        Task<bool> DeleteEmployee(int id);
        Task<List<Employee>> GetEmployeeDetails();
        Task<Employee> GetEmployeeDetails(int id);
        Task<Employee> UpdateEmployee(Employee employee);
    }
}