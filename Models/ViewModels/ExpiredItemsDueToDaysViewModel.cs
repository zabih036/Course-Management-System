using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.ViewModels
{
    public class ExpiredItemsDueToDaysViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "د ورځو تعداد ورسوئ!!")]
        [Display(Name = "د ورځو تعداد")]
        public int Days { get; set; }

    }
}
