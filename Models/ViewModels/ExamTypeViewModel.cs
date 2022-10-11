using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.ViewModels
{
    public class ExamTypeViewModel
    {
        public int ExamTypeId { get; set; }

        [Required]
        [Display(Name = " Exam Type :")]
        [DataType(DataType.Text)]
        [Remote("IsExamTypeExist", "Student", AdditionalFields = "ExamTypeId", ErrorMessage = "This type is already added.")]

        public string ExamType { get; set; }
    }
}
