using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.ViewModels
{
    public class EmployeeRegistratoinViewModel
    {
        public int EmployeeId { get; set; }
        public string defalut { get; set; }

        [Required]
        [Display(Name = " Name:")]
        public string Name { get; set; }

        [Required]
        [Display(Name = " F/Name:")]
        public string FatherName { get; set; }

        [Required]
        [Display(Name = "Phone:")]
        public string Phone { get; set; }

        [EmailAddress]
        [Remote("IsEmployeeEmailExist", "Employee", AdditionalFields = "Email,EmployeeId", ErrorMessage = " This Email is already used.")]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Required]
        [Display(Name = " Province:")]
        public string Province { get; set; }

        [Required]
        [Display(Name = "District:")]
        public string District { get; set; }

        [Required]
        [Display(Name = "Vilage:")]
        public string Vilage { get; set; }

        [Required]
        [Display(Name = "ID Card No:")]
        public string IdCardNo { get; set; }

        [Required]
        [Display(Name = "Employee Type:")]
        [ForeignKey("EmployeeType_Id")]
        public int EmployeeType_Id { get; set; }

        [Required]
        [Display(Name = "Salary Type:")]
        public string SalaryType { get; set; }

        [Required]
        [Display(Name = "Salary:")]
        public int Salary { get; set; }

        [Required]
        [Display(Name = " Hire Date :")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; } = DateTime.Now.Date;

        [Required]
        [Display(Name = " Fire Date :")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FireDate { get; set; } = DateTime.Now.Date;

        [Display(Name = " Photo :")]
        // [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }
        public string oldEmail { get; set; }



    }
}
