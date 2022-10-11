using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class EmployeeType
    {
        public EmployeeType()
        {
            Employee = new HashSet<Employee>();
        }

        public int EmployeeTypeId { get; set; }
        public string EmployeeId { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
