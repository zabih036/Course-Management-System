using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class HourlyClass
    {
        public int HourlyTimeId { get; set; }
        public int? ClassId { get; set; }
        public int? EmployeeId { get; set; }
        public int? TotalHours { get; set; }

        public virtual Class Class { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
