using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.ViewModels
{
    public class CompanyViewModel
    {

        public int CompanyId { get; set; }

        [Required(ErrorMessage = "د کمپنی نوم ولیکی ")]
        [Display(Name = "د کمپنی نوم:")]
        [DataType(DataType.Text)]
        [Remote("IsCompanyExist", "ManageSmallTables", AdditionalFields = "CompanyId", ErrorMessage = " کمپنې په دې نوم شتون لرې تاسو نه شی کولی په دی نوم بله کمپنی ذخیره کړی.")]

        public string Company { get; set; }



    }


}
