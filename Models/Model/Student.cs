using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class Student
    {
        public Student()
        {
            Attendance = new HashSet<Attendance>();
            AttendanceSheet = new HashSet<AttendanceSheet>();
            ExamMarks = new HashSet<ExamMarks>();
            Fees = new HashSet<Fees>();
            StudentClass = new HashSet<StudentClass>();
        }

        public int StudentId { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string IdCardNo { get; set; }
        public string School { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? RegDate { get; set; }
        public int ChairNumber { get; set; }
        public string Photo { get; set; }

        public virtual ICollection<Attendance> Attendance { get; set; }
        public virtual ICollection<AttendanceSheet> AttendanceSheet { get; set; }
        public virtual ICollection<ExamMarks> ExamMarks { get; set; }
        public virtual ICollection<Fees> Fees { get; set; }
        public virtual ICollection<StudentClass> StudentClass { get; set; }
    }
}
