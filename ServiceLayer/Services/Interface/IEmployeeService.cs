
using DomainLayer.EntityModels;

namespace ServiceLayer.Services.Interface
{
    public interface IEmployeeService
    {


        Task<Employee> GetEmployeeByIdAsync(int id);

        Task<Employee> AddEmployeeAsync(Employee employee);

        Task<List<Employee>> GetEmployeesAsync();

        Task<bool> DeleteEmployeeAsync(int id);

        Task<Employee> UpdateEmployeeAsync(int id, Employee employee);



    }
}
