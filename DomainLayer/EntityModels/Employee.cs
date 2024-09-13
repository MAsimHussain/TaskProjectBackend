using System.ComponentModel.DataAnnotations;

namespace DomainLayer.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email format.")]
        public string Email { get; set; } = null!;

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? Phone { get; set; }

        public string? ProfileImage { get; set; }
    }

}
