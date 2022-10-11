using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class Salary
    {
        public int SalaryId { get; set; }
        public int? PaidAmount { get; set; }
        public DateTime? Date { get; set; }
        public int? EmployeeId { get; set; }
        public int? PaidBy { get; set; }
        public int? ClassId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
