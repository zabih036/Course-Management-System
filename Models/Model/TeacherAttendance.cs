using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class TeacherAttendance
    {
        public int SheetId { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }
        public int? EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
