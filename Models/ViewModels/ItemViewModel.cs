using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.ViewModels
{
    public class ItemViewModel
    {
        public ItemViewModel()
        {
            //CategoryId = new ICollection<Category>();
        }
        public int ItemId { get; set; }

        [Required(ErrorMessage = "د جنس نوم ولیکی ")]
        [Display(Name = "د جنس نوم:")]
        [Remote("IsItemExist", "Item", AdditionalFields = "ItemId", ErrorMessage = "نوموړی جنس شتون لرې تاسو نه شی کولی په دی نوم بل جنس ذخیره کړی.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "د جنس لاتینی نوم ولیکی ")]
        [Display(Name = "لاتینې نوم:")]
        public string LatinName { get; set; }



        [Required(ErrorMessage = "د جنس کټګورې انتخاب کړی ")]
        [Display(Name = "د جنس کټګورې:")]
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }


        //[Required(ErrorMessage = "د کمپنی نوم انتخاب کړی ")]
        //[Display(Name = "کمپنی نوم")]
        //[ForeignKey("CompanyId")]
        //public int CompanyId { get; set; }

        //[Required(ErrorMessage = "د هیواد نوم انتخاب کړی ")]
        //[Display(Name = "هیواد ")]
        //[ForeignKey("CountryId")]
        //public int CountryId { get; set; }


    }


}
