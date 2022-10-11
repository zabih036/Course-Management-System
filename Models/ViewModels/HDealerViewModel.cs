using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.ViewModels
{
    public class HDealerViewModel
    {

        public int HDealerID { get; set; }

        [Display(Name = "Name:")]
        [Required]
        public string HDealer { get; set; }

        [Display(Name = "Phone:")]
        [Required]
        public string Mobile { get; set; }
    }
}
