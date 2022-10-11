using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class Employee
    {
        public Employee()
        {
            ClassTeacher = new HashSet<ClassTeacher>();
            Dealing = new HashSet<Dealing>();
            HourlyClass = new HashSet<HourlyClass>();
            SalaryNavigation = new HashSet<Salary>();
            TeacherAttendance = new HashSet<TeacherAttendance>();
            TeacherType = new HashSet<TeacherType>();
        }

        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Village { get; set; }
        public string IdCardNo { get; set; }
        public int? Salary { get; set; }
        public string SalaryType { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? FireDate { get; set; }
        public int? EmployeeTypeId { get; set; }
        public string Photo { get; set; }

        public virtual EmployeeType EmployeeType { get; set; }
        public virtual ICollection<ClassTeacher> ClassTeacher { get; set; }
        public virtual ICollection<Dealing> Dealing { get; set; }
        public virtual ICollection<HourlyClass> HourlyClass { get; set; }
        public virtual ICollection<Salary> SalaryNavigation { get; set; }
        public virtual ICollection<TeacherAttendance> TeacherAttendance { get; set; }
        public virtual ICollection<TeacherType> TeacherType { get; set; }
    }
}
