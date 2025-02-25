using ApplicationLayer.CacheData;
using ApplicationLayer.Models;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ServiceLayer.Services.Interface;
using TaskProject.UI.DIServices;

namespace OnionArchitecture.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly IEmployeeService _employeeService;
		private readonly ICacheProvider _cacheProvider;
		public EmployeeController(IEmployeeService employeeService, ICacheProvider cacheProvider)
		{
			_employeeService = employeeService;

			_cacheProvider = cacheProvider;
		}


		[HttpPost]

		public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employeeDto)
		{
			if (!string.IsNullOrEmpty(employeeDto.Email))
			{
				var employee = await _employeeService.AddEmployeeAsync(employeeDto);
				if (employee != null)
				{
					if (_cacheProvider.TryGetValue(CacheKeys.Employee, out IEnumerable<EmployeeReadDto> employeesDto))
					{
						var updateCache = await _employeeService.GetEmployeesAsync();

						var cacheEntryOptions = new MemoryCacheEntryOptions()
						{
							AbsoluteExpiration = DateTime.Now.AddSeconds(30),
							SlidingExpiration = TimeSpan.FromSeconds(30),
							Size = 800
						};

						_cacheProvider.Set(CacheKeys.Employee, updateCache, cacheEntryOptions);

					}


					return Ok(new ErrorResponse()
					{
						StatusCode = 200,
						Message = "Success",
						Timestamp = DateTime.Now,
						StatusText = "Ok",
						Detail = "Record Created Successfully!"
					});
				}

			}


			return NotFound(new ErrorResponse()
			{
				StatusCode = 500,
				Message = "Failed",
				Timestamp = DateTime.Now,
				StatusText = "Internal Server Error",
				Detail = "Internal server error. Please, try again."
			});
		}


		[HttpGet]
		[Route("Employees")]
		public async Task<IActionResult> GetEmployees()
		{
			if (!_cacheProvider.TryGetValue(CacheKeys.Employee, out IEnumerable<EmployeeReadDto> employeesDto))
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
				// update cache 
				if (_cacheProvider.TryGetValue(CacheKeys.Employee, out IEnumerable<EmployeeReadDto> employeesDto))
				{
					var updateCache = employeesDto.Where(e => e.Id != id).ToList();

					var cacheEntryOptions = new MemoryCacheEntryOptions()
					{
						AbsoluteExpiration = DateTime.Now.AddSeconds(30),
						SlidingExpiration = TimeSpan.FromSeconds(30),
						Size = 800
					};

					_cacheProvider.Set(CacheKeys.Employee, updateCache, cacheEntryOptions);

				}

				return Ok(new ErrorResponse()
				{
					StatusCode = 200,
					Message = "Success",
					Timestamp = DateTime.Now,
					StatusText = "Ok",
					Detail = "Record Delete Successfully!"
				});
			}
			return NotFound(new ErrorResponse()
			{
				StatusCode = 404,
				Message = "Failed",
				Timestamp = DateTime.Now,
				StatusText = "NotFound",
				Detail = "Record is not found with user id!"
			});
		}


		[HttpGet("{id}")]
		public async Task<IActionResult> GetEmployeeById([FromRoute] int id)
		{

			if (!_cacheProvider.TryGetValue(CacheKeys.Employee, out EmployeeReadDto employeeDto))
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
		public async Task<IActionResult> UpdateEmployee([FromRoute] int id, [FromBody] EmployeeDto employee)
		{

			var updateEmployee = await _employeeService.UpdateEmployeeAsync(id, employee);

			if (updateEmployee == null)
			{
				return NotFound(new ErrorResponse()
				{
					StatusCode = 200,
					Message = "Failed",
					Timestamp = DateTime.Now,
					StatusText = "NotFound",
					Detail = "Failed to update employee.!"
				});
			}

			if (_cacheProvider.TryGetValue(CacheKeys.Employee, out IEnumerable<EmployeeReadDto> employeesDto))
			{
				var updateemployeeDto = await _employeeService.GetEmployeesAsync();
				if (updateemployeeDto is null)
					return NotFound($"Employee was not found with Id: {id}");

				var cacheEntryOpions = new MemoryCacheEntryOptions()
				{
					AbsoluteExpiration = DateTime.Now.AddSeconds(30),
					SlidingExpiration = TimeSpan.FromSeconds(30),
					Size = 200
				};

				_cacheProvider.Set(CacheKeys.Employee, updateemployeeDto, cacheEntryOpions);
			}

			return Ok(new ErrorResponse()
			{
				StatusCode = 200,
				Message = "Success",
				Timestamp = DateTime.Now,
				StatusText = "Ok",
				Detail = "Employee Update Successfully!"
			});

		}
	}
}
