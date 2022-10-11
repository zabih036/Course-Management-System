using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.ViewModels
{
    public class DeallerViewModel
    {
        public string defalut { get; set; }
        public int DealerId { get; set; }

        [Required(ErrorMessage = "د کهاته نوم ولیکی!!")]
        [Display(Name = "د کهاته دار نوم:")]
        public string Name { get; set; }


        [Required(ErrorMessage = " کهاتدار د پلار نوم ولیکی ")]
        [Display(Name = " د پلار نوم :")]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "  هیواد نوم ولیکی ")]
        [Display(Name = " هیواد  :")]
        [ForeignKey("CountryId")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "  ولایت نوم ولیکی ")]
        [Display(Name = " ولایت  :")]
        public string Province { get; set; }

        [Required(ErrorMessage = "  ولسوالی نوم ولیکی ")]
        [Display(Name = " ولسوالې  :")]
        public string District { get; set; }

        [Required(ErrorMessage = "  کلې نوم ولیکی ")]
        [Display(Name = " کلی  :")]
        public string Vilage { get; set; }

        [Display(Name = " تذکیرې نمبر :")]
        public int TazkiraNo { get; set; }

        //[Required(ErrorMessage = " د مبائیل شمیره ولیکی")]
        [Display(Name = "د مبائیل شمیره:")]
        public string Phone { get; set; }

        //[Required(ErrorMessage = " بریښنالیک ولیکی ")]
        [Display(Name = "بریښنالیک :")]
        // [EmailAddress(ErrorMessage = " بریښنالیک سم نه دی")]
        [Remote("IsCustomerEmailExist", "Employee", AdditionalFields = "Email,DealerId", ErrorMessage = " نوموړی بریښنالیک د بل کهاتدار لپاره استعمال شوی .")]
        public string Email { get; set; }


        [Required(ErrorMessage = " د شروع نیټه ولیکی ")]
        [Display(Name = " شروع نیټه :")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;


        [Required(ErrorMessage = "د کهات دار ډول انتخاب کړئ!!")]
        [Display(Name = "د کهاته دار ډول:")]
        [ForeignKey("DealerTypeId")]
        public int DealerTypeId { get; set; }

        [Display(Name = " تفصیل:")]
        public string Details { get; set; }

        [Required(ErrorMessage = "  د مشترې کټګورې ولیکئ ")]
        [Display(Name = " کټګورې  :")]
        public string Category { get; set; }

        [Display(Name = " د کهاتدار تصویر :")]
        // [DataType(DataType.Upload)

        public IFormFile Image { get; set; }
        public string oldEmail { get; set; }

    }
}
