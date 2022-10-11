using System;
using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.ViewModels
{
    public class FeesViewModel
    {
        public int OldPaid { get; set; }
        public int FeesId { get; set; }

        [Required]
        [Display(Name = "Amount :")]
        [DataType(DataType.Currency)]
        public int PaidAmount { get; set; }
    }
}
