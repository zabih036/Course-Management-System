using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class TeacherType
    {
        public int TeacherTypeId { get; set; }
        public int? EmployeeId { get; set; }
        public string Type { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
