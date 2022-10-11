using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class Department
    {
        public Department()
        {
            ClassName = new HashSet<ClassName>();
        }

        public int DepartmentId { get; set; }
        public string Department1 { get; set; }

        public virtual ICollection<ClassName> ClassName { get; set; }
    }
}
