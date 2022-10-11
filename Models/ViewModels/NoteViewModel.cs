using System;
using System.ComponentModel.DataAnnotations;

namespace JahanInstitute.Models.ViewModels
{
    public class NoteViewModel
    {
        public int NoteId { get; set; }

        [Required]
        [Display(Name = "Note:")]
        public string Note { get; set; }

        /////////////////////////////////////////////////////////////
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date for note:")]
        [DataType(DataType.Date)]
        public DateTime TargetDate { get; set; }
        ///////////////////////////////////////////////////////////
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Remind date:")]
        [DataType(DataType.Date)]
        public DateTime RemindDate { get; set; }
    }
}
