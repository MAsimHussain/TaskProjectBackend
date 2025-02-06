using ApplicationLayer.CacheData;
using ApplicationLayer.Models;
using DomainLayer.EntityModels;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ServiceLayer.Services.Interface;

namespace OnionArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ICacheProvider   _cacheProvider;
        public EmployeeController(IEmployeeService employeeService, ICacheProvider cacheProvider)
        {
            _employeeService = employeeService;

            _cacheProvider = cacheProvider;
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
			if (!_cacheProvider.TryGetValue(CacheKeys.Employee, out IEnumerable< EmployeeReadDto> employeesDto))
            {
				employeesDto = await _employeeService.GetEmployeesAsync();

            if (employeesDto is null)
                return NotFound("Employee was not found");


				var cacheEntryOpions = new MemoryCacheEntryOptions()
				{
					AbsoluteExpiration = DateTime.Now.AddSeconds(30),
					SlidingExpiration = TimeSpan.FromSeconds(30),
					Size = 200
				};
              _cacheProvider.Set(CacheKeys.Employee, employeesDto, cacheEntryOpions);    
			}
			return Ok(employeesDto);

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
           
            if(!_cacheProvider.TryGetValue(CacheKeys.Employee, out EmployeeReadDto employeeDto))
            {
				employeeDto = await _employeeService.GetEmployeeByIdAsync(id);
				if (employeeDto is null)
				return NotFound($"Employee was not found with Id: {id}");
				
				var cacheEntryOpions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 200
                };

                _cacheProvider.Set(CacheKeys.Employee, employeeDto, cacheEntryOpions);  
            }

            return Ok(employeeDto);


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
