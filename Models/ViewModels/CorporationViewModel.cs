using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.ViewModels
{
    public class CorporationViewModel
    {

        public int CorporationId { get; set; }

        [Required(ErrorMessage = "د شرکت نوم ولیکی ")]
        [Display(Name = "د شرکت نوم:")]
        [DataType(DataType.Text)]
        [Remote("IsCorporationExist", "ManageSmallTables", AdditionalFields = "CorporationId", ErrorMessage = " شرکت په دې نوم شتون لرې تاسو نه شی کولی په دی نوم بل شرکت ذخیره کړی.")]

        public string Corporation { get; set; }

    }
}
