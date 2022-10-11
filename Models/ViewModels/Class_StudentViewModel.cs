using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JahanInstitute.Models.ViewModels
{
    public class Class_StudentViewModel
    {
        [ForeignKey("ClassId")]
        public int Class_Id { get; set; }

        [ForeignKey("EmployeeId")]
        public int Teacher_Id { get; set; }
    }
}
