using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.ViewModels
{
    public class classRegistrationViewModel
    {

        public int classNameId { get; set; }

        [Required]
        [Display(Name = "Class Name :")]
        [DataType(DataType.Text)]
        [Remote("isClassExist", "Class", AdditionalFields = "classNameId", ErrorMessage = "This class is already exist, duplication is not allowed.")]
        public string className { get; set; }


        [Display(Name = "Department :")]
        [ForeignKey("DepartmentId")]
        [Required]
        public int department { get; set; }

    }


}
