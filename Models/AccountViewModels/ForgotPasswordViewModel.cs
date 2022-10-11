using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your email address to send you reset code for new password.")]
        [EmailAddress]
        [Display(Name = "Email:")]
        public string Email { get; set; }
    }
}
