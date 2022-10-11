using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.ViewModels
{
    public class ClassViewModel
    {

        public int classId { get; set; }

        [Display(Name ="Class Name")]
        [ForeignKey("ClassNameId")]
        [Required]
        public int ClassName { get; set; }
        
        //[Display(Name = "Department:")]
        //[Required]
        //public int Department { get; set; }

        [Display(Name = "Shift:")]
        [Required]
        public string shift { get; set; }

        [Range(minimum:1,maximum:12,ErrorMessage ="Hours must be 1 - 12")]
        [Required (ErrorMessage ="Hours Required")]
        public int startHours { get; set; }

        [Range(minimum: 0, maximum: 59, ErrorMessage = "Hours must be 0 - 59")]
        [Required(ErrorMessage = "Minutes Required")]
        public int startMinutes { get; set; }

        public string startMiridiam { get; set; }
        /// <End_Time>
        [Range(minimum:1,maximum:12,ErrorMessage ="Hours must be 1 - 12")]
        [Required(ErrorMessage = "Hours Required")]
        public int endHours { get; set; }

        [Range(minimum: 0, maximum: 59, ErrorMessage = "Hours must be 0 - 59")]
        [Required(ErrorMessage = "Minutes Required")]
        public int endMinutes { get; set; }

        public string endMiridiam { get; set; }
        /// </End Time>


        [Display(Name = "Start Date:")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public System.DateTime startDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date:")]
        [DataType(DataType.Date)]
        public DateTime endDate { get; set; }

        [Display(Name = "Class Type:")]
        [ForeignKey("ClassTypeId")]
        [Required]
        public int classType { get; set; }

        [Display(Name = "Total Fee:")]
        [Required]
        public int totalFee { get; set; }
        //[Required(ErrorMessage = " واحد ولیکی ")]
        //[Display(Name = "واحد:")]
        //[DataType(DataType.Text)]
        //[Remote("IsUnitExist", "ManageSmallTables", AdditionalFields = "UnitId", ErrorMessage = " واحد په دې نوم شتون لرې تاسو نه شی کولی په دی نوم بل واحد ذخیره کړی.")]

    }


}
