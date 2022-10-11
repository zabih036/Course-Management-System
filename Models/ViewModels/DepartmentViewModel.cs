using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.ViewModels
{
    public class DepartmentViewModel
    {

        public int DepartmentId { get; set; }

        [Required]
        [Display(Name = "Department Name :")]
        [DataType(DataType.Text)]
        [Remote("isDepartmentExist", "Class", AdditionalFields = "DepartmentId", ErrorMessage = "This Department is already exist, duplication is not allowed.")]
        public string Department { get; set; }

    }


}
