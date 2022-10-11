using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.ViewModels
{
    public class ForgotPasswordCodeViewModel
    {
        [Required(ErrorMessage = "کوډ ولیکی.")]
        [EmailAddress]
        [Display(Name = "کوډ")]
        public int Code { get; set; }
    }
}
