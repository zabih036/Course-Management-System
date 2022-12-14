using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password must be six digit long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name ="New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
