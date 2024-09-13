namespace TaskProject.UI.Model
{
    public class EmployeeModel
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Handle the image as a byte array
        public IFormFile? ProfileImageFile { get; set; }
    }

}
