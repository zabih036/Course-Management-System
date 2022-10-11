using System;
using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.ViewModels
{
    public class SalaryViewModel
    {
        public int EmployeeId { get; set; }
        public int SalaryId { get; set; }
        public int ClassId { get; set; }

        [Required]
        [Display(Name = "Amount:")]
        [DataType(DataType.Currency)]
        public int PaidAmount { get; set; }

        [Required]
        [Display(Name = "Date:")]
        [DataType(DataType.Date)]
        public DateTime PaidDate { get; set; }



    }
}
