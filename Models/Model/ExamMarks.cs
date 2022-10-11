using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class ExamMarks
    {
        public int ExamId { get; set; }
        public int? ClassId { get; set; }
        public int? StudentId { get; set; }
        public int? ExamTypeId { get; set; }
        public int? TotalMarks { get; set; }
        public int? ObtainMarks { get; set; }
        public int? HelpMarks { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }

        public virtual Class Class { get; set; }
        public virtual ExamType ExamType { get; set; }
        public virtual Student Student { get; set; }
    }
}
