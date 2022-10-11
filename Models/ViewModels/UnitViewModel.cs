using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.ViewModels
{
    public class UnitViewModel
    {

        public int UnitId { get; set; }

        [Required(ErrorMessage = " واحد ولیکی ")]
        [Display(Name = "واحد:")]
        [DataType(DataType.Text)]
        [Remote("IsUnitExist", "ManageSmallTables", AdditionalFields = "UnitId", ErrorMessage = " واحد په دې نوم شتون لرې تاسو نه شی کولی په دی نوم بل واحد ذخیره کړی.")]

        public string Unit { get; set; }



    }


}
