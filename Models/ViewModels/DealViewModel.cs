using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.ViewModels
{
    public class DealViewModel
    {
        public int DealId { get; set; }

        [Required]
        [Display(Name = "Loan:")]
        public float Debit { get; set; }

        [Required]
        [Display(Name = "Pay:")]
        public float Credit { get; set; }
        ///////////////////////////
        [Required]
        [ForeignKey("DealerID")]
        public int DealerID { get; set; }
        ////////////////////
        [Required]
        [ForeignKey("CurrencyID")]
        public int CurrencyID { get; set; }


        public string Detail { get; set; }


    }
}
