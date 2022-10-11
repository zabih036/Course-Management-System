using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class ClassTeacher
    {
        public int ClassTeacherId { get; set; }
        public int? ClassId { get; set; }
        public int? EmployeeId { get; set; }

        public virtual Class Class { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
