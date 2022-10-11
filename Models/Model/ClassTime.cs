using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class ClassTime
    {
        public int? ClassId { get; set; }
        public int? TimeId { get; set; }

        public virtual Class Class { get; set; }
        public virtual Time Time { get; set; }
    }
}
