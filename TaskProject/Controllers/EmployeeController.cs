using ApplicationLayer.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.Interface;

namespace OnionArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;

        }


        [HttpPost]

        public async Task<IActionResult> AddEmployee([FromForm] EmployeeDto employeeDto)
        {

            var employee = await _employeeService.AddEmployeeAsync(employeeDto);
            if (employee == null) {

                return NotFound();
            }

            return Ok("Employee created successfully.");
        }


        [HttpGet]
        [Route("Employees")]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeService.GetEmployeesAsync();

            if (employees is null)
            {
                return NotFound();
            }
            return Ok(employees);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            var emplyee = await _employeeService.DeleteEmployeeAsync(id);

            if (emplyee)
            {

                return Ok("Employee Delete Successfully!");


            }
            return NotFound();




        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute] int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee is null)
            {

                return NotFound("Employee Not Found");


            }

            return Ok(employee);


        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int id, [FromForm] EmployeeDto employee)
        {
            
            var updateEmployee = await _employeeService.UpdateEmployeeAsync(id, employee);

            if (updateEmployee == null)
            {
                return NotFound("Failed to update employee.");
            }

            return Ok("Employee Update Successfully!");


        }





    }
}
