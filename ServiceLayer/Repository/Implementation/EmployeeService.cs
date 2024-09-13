
using ServiceLayer.Services.Interface;
using DomainLayer.EntityModels;
using RepositoryLayer.Data;
using Microsoft.EntityFrameworkCore;
using ApplicationLayer.Models;
using Microsoft.AspNetCore.Http;

namespace ServiceLayer.Service.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicatonDbContext _DbContext;
        public EmployeeService(ApplicatonDbContext dbContext)
        {

            _DbContext = dbContext;
        }
        public async Task<EmployeeDto> AddEmployeeAsync(EmployeeDto employeeDto)
        {
           
            string? imageFileName = await UploadProfileImageAsync(employeeDto.ProfileImageFile);

            var employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                ProfileImage = imageFileName
            };

            _DbContext.tblEmployees.Add(employee);

            await _DbContext.SaveChangesAsync();

            return employeeDto;

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

        public async Task<EmployeeReadDto?> GetEmployeeByIdAsync(int id)
        {

            var employee =  await _DbContext.tblEmployees.Where(e => e.Id == id).Select(e => new EmployeeReadDto
            {
                Id = e.Id,
                Email = e.Email,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Phone = e.Phone,
                ProfileImage = e.ProfileImage,


            }).FirstOrDefaultAsync();

            return employee;
        }

        public async Task<List<EmployeeReadDto>> GetEmployeesAsync()
        {
            var employees = await _DbContext.tblEmployees.Select(e => new EmployeeReadDto()
            {
                Id = e.Id,  
                Email = e.Email,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Phone = e.Phone,
                ProfileImage = e.ProfileImage,
            }).ToListAsync();

             
            return employees;    
        }

        public async Task<EmployeeDto> UpdateEmployeeAsync(int id, EmployeeDto employeeDto)
        {

            var empExist =  _DbContext.tblEmployees.FirstOrDefault(e => e.Id ==id);

           
            string? imageFileName = empExist.ProfileImage;

            if (employeeDto.ProfileImageFile != null)
            {
                imageFileName = await UploadProfileImageAsync(employeeDto.ProfileImageFile);
            }
            if (empExist != null)
            {
                empExist.FirstName = employeeDto.FirstName;
                empExist.LastName = employeeDto.LastName;
                empExist.Email = employeeDto.Email;
                empExist.Phone = employeeDto.Phone;
                empExist.ProfileImage = imageFileName;
            }

            await _DbContext.SaveChangesAsync();

            return employeeDto;

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
