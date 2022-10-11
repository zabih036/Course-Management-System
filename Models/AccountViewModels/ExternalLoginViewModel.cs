using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
