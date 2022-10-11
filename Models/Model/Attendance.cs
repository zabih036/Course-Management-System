using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class Attendance
    {
        public int AttendenceId { get; set; }
        public int? ClassId { get; set; }
        public int? StudentId { get; set; }

        public virtual Class Class { get; set; }
        public virtual Student Student { get; set; }
    }
}
