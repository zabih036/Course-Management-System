using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.ViewModels
{
    public class ExamViewModel
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Class:")]
        [DataType(DataType.Text)]
        //[Remote("IsStockExist", "ManageSmallTables", AdditionalFields = "StockId", ErrorMessage = " زیرمتون په دې نوم شتون لرې تاسو نه شی کولی په دی نوم بل زیرمتون ذخیره کړی.")]
        public int ClassId { get; set; }

        [Required]
        [Display(Name = "Exam Type:")]
        [ForeignKey("ExamTypeId")]
        public int ExamTypeId { get; set; }

        [Required]
        [Display(Name = "Total Marks:")]
        public int TotalMarks { get; set; }

        [Required]
        [Display(Name = "Obtain Marks:")]
        public int ObtainMarks { get; set; }

        [Display(Name = "Help Marks:")]
        public int HelpMarks { get; set; }

        [Required]
        [Display(Name = "Date:")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Display(Name = "Description:")]
        public string Description { get; set; }
    }
}
