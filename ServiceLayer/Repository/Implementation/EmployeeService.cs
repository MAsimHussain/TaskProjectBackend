using ApplicationLayer.Models;
using DomainLayer.EntityModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using ServiceLayer.Services.Interface;

namespace ServiceLayer.Service.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicatonDbContext _DbContext;
        private readonly IFileService _FileService;  
        public EmployeeService(ApplicatonDbContext dbContext, IFileService fileService)
        {
            _FileService = fileService; 
            _DbContext = dbContext;
        }
        public async Task<EmployeeDto> AddEmployeeAsync(EmployeeDto employeeDto)
        {

            string? imageFileName = await _FileService.UploadFileAsync(employeeDto.ProfileImageFile);

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
            var employee = _DbContext.tblEmployees.FirstOrDefault(x => x.Id == id);

            if (employee != null)
            {
                await _FileService.DeleteFileAsync(employee.ProfileImage);
                _DbContext.tblEmployees.Remove(employee);
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

            if (employeeDto.ProfileImageFile != null) { 

                await _FileService.DeleteFileAsync(imageFileName);

               var Image  = await _FileService.UploadFileAsync(employeeDto.ProfileImageFile);
                imageFileName = Image;
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
