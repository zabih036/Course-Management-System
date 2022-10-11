using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.ViewModels
{
    public class CurrencyViewModel
    {

        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "د اسعارو نوع ولیکی ")]
        [Display(Name = "د اسعارو نوع:")]
        [DataType(DataType.Text)]
        [Remote("IsCurrencyExist", "ManageSmallTables", AdditionalFields = "CurrencyId", ErrorMessage = " د پیسو نوع په دې نوم شتون لرې تاسو نه شی کولی په دی نوم بیا ذخیره کړی.")]

        public string Currency { get; set; }
    }
}
