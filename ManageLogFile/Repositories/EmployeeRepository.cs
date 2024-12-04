using ManageLogFile.Data.Context;
using ManageLogFile.Model.Entities;
using ManageLogFile.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ManageLogFile.Repositories
{
    public class EmployeeRepository(LogFileContext context) : IEmployeeRepository
    {
        private readonly LogFileContext _context = context;

        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _context.Employees.SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            var result = await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var result = _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.SingleOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                return false;
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
