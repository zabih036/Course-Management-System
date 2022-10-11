using Microsoft.AspNetCore.Identity;

namespace JahanInstitute.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string EmployeeName { get; set; }
        public string ImagePath { get; set; }
        public string UserType { get; set; }
       
    }
}
