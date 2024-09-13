
using ApplicationLayer.Models;
using DomainLayer.EntityModels;

namespace ServiceLayer.Services.Interface
{
    public interface IEmployeeService
    {


        Task<EmployeeReadDto> GetEmployeeByIdAsync(int id);

        Task<EmployeeDto> AddEmployeeAsync(EmployeeDto employee);

        Task<List<EmployeeReadDto>> GetEmployeesAsync();

        Task<bool> DeleteEmployeeAsync(int id);

        Task<EmployeeDto> UpdateEmployeeAsync(int id, EmployeeDto employee);



    }
}
