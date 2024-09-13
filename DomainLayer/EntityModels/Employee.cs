using System.ComponentModel.DataAnnotations;

namespace DomainLayer.EntityModels
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required]
        public string Email { get; set; }
        public string? Phone { get; set; }

        // Image property (storing the image as a byte array)
        public string? ProfileImage { get; set; }
    }
}
