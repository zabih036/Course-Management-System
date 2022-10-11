using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class Class
    {
        public Class()
        {
            Attendance = new HashSet<Attendance>();
            AttendanceSheet = new HashSet<AttendanceSheet>();
            ClassTeacher = new HashSet<ClassTeacher>();
            ExamMarks = new HashSet<ExamMarks>();
            Fees = new HashSet<Fees>();
            HourlyClass = new HashSet<HourlyClass>();
            StudentClass = new HashSet<StudentClass>();
        }

        public int ClassId { get; set; }
        public int? ClassNameId { get; set; }
        public int? StartHour { get; set; }
        public int? StartMinute { get; set; }
        public string StartMiridiam { get; set; }
        public string EndMiridiam { get; set; }
        public int? EndHour { get; set; }
        public int? EndMinute { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TotalFee { get; set; }
        public string Shift { get; set; }
        public string Status { get; set; }

        public virtual ClassName ClassName { get; set; }
        public virtual ICollection<Attendance> Attendance { get; set; }
        public virtual ICollection<AttendanceSheet> AttendanceSheet { get; set; }
        public virtual ICollection<ClassTeacher> ClassTeacher { get; set; }
        public virtual ICollection<ExamMarks> ExamMarks { get; set; }
        public virtual ICollection<Fees> Fees { get; set; }
        public virtual ICollection<HourlyClass> HourlyClass { get; set; }
        public virtual ICollection<StudentClass> StudentClass { get; set; }
    }
}
