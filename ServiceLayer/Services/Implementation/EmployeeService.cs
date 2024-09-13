
using ServiceLayer.Services.Interface;
using DomainLayer.EntityModels;
using RepositoryLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.Service.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicatonDbContext _DbContext;
        public EmployeeService(ApplicatonDbContext dbContext)
        {

            _DbContext = dbContext;
        }
        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {


            var emp = new Employee()
            {
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Phone = employee.Phone,
                ProfileImage = employee.ProfileImage,
            };


            _DbContext.tblEmployees.Add(emp);

            await _DbContext.SaveChangesAsync();

            return emp;

        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var user = _DbContext.tblEmployees.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {

                _DbContext.tblEmployees.Remove(user);
                await _DbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {

            return _DbContext.tblEmployees.FirstOrDefault(x => x.Id == id);

        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {

            var users = await _DbContext.tblEmployees.ToListAsync();

            return users;
        }

        public async Task<Employee> UpdateEmployeeAsync(int id, Employee employee)
        {
            var empExist = await _DbContext.tblEmployees.FindAsync(id);

            if (empExist != null)
            {
                empExist.FirstName = employee.FirstName;
                empExist.LastName = employee.LastName;
                empExist.Email = employee.Email;
                empExist.Phone = employee.Phone;
                empExist.ProfileImage = employee.ProfileImage;
            }

            await _DbContext.SaveChangesAsync();

            return employee;

        }
    }
}
