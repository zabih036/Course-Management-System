using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.ViewModels
{
    public class ExchangeBillViewModel
    {
        [Display(Name = "د بېل نمبر:")]
        [Required(ErrorMessage = "د بېل نمبر داخل کړئ!!")]
        public string billNo { get; set; }


        [Display(Name = "مشتري:")]
        [Required(ErrorMessage = "صحیح مشتري انتخاب کړئ!!")]
        [ForeignKey("DealerId")]
        public int dealerid { get; set; }
    }
}
