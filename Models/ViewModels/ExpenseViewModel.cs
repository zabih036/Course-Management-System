using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.ViewModels
{
    public class ExpenseViewModel
    {

        public int ExpenseId { get; set; }

        [Required]
        [Display(Name = " Amount")]
        [DataType(DataType.Currency, ErrorMessage = "Enter only digits.")]
        public int Expense { get; set; }

        [Required]
        [Display(Name = " Expense Type:")]
        [ForeignKey("ExpenseTypeId")]
        public int ExpenseTypeId { get; set; }

        [Required]
        [Display(Name = " Date :")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpenseDate { get; set; }

        // [Required(ErrorMessage = "د مصرف شوی پیسې تفصیل ولیکی ")]
        [Display(Name = "Details:")]
        [DataType(DataType.Text)]
        public string Detail { get; set; }
    }
}
