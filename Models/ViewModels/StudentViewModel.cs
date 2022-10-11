using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.ViewModels
{
    public class StudentViewModel
    {
        public string defalut { get; set; }
        public int StudentID { get; set; }
        public string camera { get; set; }

        [Required]
        [Display(Name = "Name:")]
        public string Name { get; set; }


        [Required]
        [Display(Name = "F/Name:")]
        public string FatherName { get; set; }

       
        [Display(Name = "ID Card Number:")]
        [Remote("IsStudentExist", "Student", AdditionalFields = "idCardNumber,StudentID", ErrorMessage = "This student is already registered.")]
        public string idCardNumber { get; set; }

        [Display(Name = "School:")]
        public string School { get; set; }

        [Required]
        [Display(Name = "Address:")]
        public string Adress { get; set; }

        [Required]
        [Display(Name = "Gender:")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Phone:")]
        public string Phone { get; set; }

       
        [Display(Name = "Email:")]
        //[Remote("IsCustomerEmailExist", "Employee", AdditionalFields = "Email,DealerId", ErrorMessage = " نوموړی بریښنالیک د بل کهاتدار لپاره استعمال شوی .")]
        public string Email { get; set; }

        //[Required]
        //[Display(Name = "Chair Nunber:")]
        //public string Chair_No { get; set; }

        public IFormFile Image { get; set; }
        public string oldEmail { get; set; }

    }
}
