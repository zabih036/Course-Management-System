using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]       
        [ForeignKey("EmployeeId")]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
       // [Remote("IsEmployeeAccountExist", "Account", AdditionalFields = "register_EmployeeId", ErrorMessage = " This user has an account.")]

        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password ")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


        [Required]
        [Display(Name = "Department:")]
        [ForeignKey("id")]
        public string id { get; set; }

        [Required]
        [Display(Name = "Department:")]
        [ForeignKey("id2")]
        public string studentOrEmployee { get; set; } 
        public string id2 { get; set; } = "jan";
        public string email { get; set; }
        public string email2 { get; set; }
        public string role { get; set; }
        public string trueOrfalse { get; set; }


    }
}
