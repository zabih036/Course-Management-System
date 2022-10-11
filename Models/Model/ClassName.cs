using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class ClassName
    {
        public ClassName()
        {
            Class = new HashSet<Class>();
        }

        public int ClassNameId { get; set; }
        public string ClassName1 { get; set; }
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Class> Class { get; set; }
    }
}
