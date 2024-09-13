using DomainLayer.EntityModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services.Interface;
using TaskProject.UI.Model;

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

        public async Task<IActionResult> AddEmployee([FromForm] EmployeeModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }

            string? imageFileName = await UploadProfileImageAsync(model.ProfileImageFile);

            var employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                ProfileImage = imageFileName
            };

            await _employeeService.AddEmployeeAsync(employee);

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

            return Ok("Emplyee Delete SuccessFully!");


        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById([FromRoute] int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee is null)
            {

                return NotFound();


            }

            return Ok(employee);


        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] int id, [FromForm] EmployeeModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }

            var existingEmployee = await _employeeService.GetEmployeeByIdAsync(id);

            if (existingEmployee == null)
            {
                return NotFound("Employee not found.");
            }

            string? imageFileName = existingEmployee.ProfileImage;

            if (model.ProfileImageFile != null)
            {
                imageFileName = await UploadProfileImageAsync(model.ProfileImageFile);
            }

            var employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                ProfileImage = imageFileName
            };

            var updateEmployee = await _employeeService.UpdateEmployeeAsync(id, employee);

            if (updateEmployee == null)
            {
                return NotFound("Failed to update employee.");
            }

            return Ok(updateEmployee);


        }




        private async Task<string?> UploadProfileImageAsync(IFormFile profileImageFile)
        {
            if (profileImageFile != null && profileImageFile.Length > 0)
            {
                string uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImageFile.FileName);

                string logoFilePath = Path.Combine(uploadFolderPath, uniqueFileName);

                using (var fileStream = new FileStream(logoFilePath, FileMode.Create))
                {
                    await profileImageFile.CopyToAsync(fileStream);
                }

                return uniqueFileName;
            }

            return null;
        }

    }
}
