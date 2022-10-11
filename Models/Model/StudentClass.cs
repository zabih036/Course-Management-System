using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class StudentClass
    {
        public int StdClassId { get; set; }
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public int? RollNumber { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }

        public virtual Class Class { get; set; }
        public virtual Student Student { get; set; }
    }
}
