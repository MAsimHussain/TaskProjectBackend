using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ApplicationLayer.Models
{
    public class EmployeeDto
    {


        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }


        public IFormFile? ProfileImageFile { get; set; }
    }
      public class EmployeeReadDto
    {


        public int Id { get; set; } 
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string? ProfileImage { get; set; }

    }


}
