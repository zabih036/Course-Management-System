using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JahanInstitute.Models.ViewModels
{
    public class StudentClassViewModel
    {
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public int EmployeeId { get; set; }
        public int RollNumber { get; set; }
        public string IsEnrolled { get; set; }
        [Required]
        [Display(Name ="Class Fee:")]
        public int Fee { get; set; }
        [Required]
        [Display(Name = "Paid Amount:")]
        public int PaidAmount { get; set; }
        [Display(Name = "Remain:")]
        public int Remain { get; set; }
        [Display(Name = "Discount:")]
        public int Discount { get; set; }
        [Display(Name = "Bill No:")]
        public int BillNo { get; set; }
        [Display(Name = "Description:")]
        public string Description { get; set; }
    }
}
