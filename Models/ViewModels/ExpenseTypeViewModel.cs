using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.ViewModels
{
    public class ExpenseTypeViewModel
    {
        public int ExpenseTypeId { get; set; }

        [Required]
        [Display(Name = " Expense Type :")]
        [DataType(DataType.Text)]
        [Remote("IsExpenseTypeExist", "Expense", AdditionalFields = "ExpenseTypeId", ErrorMessage = "This type is already added.")]

        public string ExpenseType { get; set; }
    }
}
