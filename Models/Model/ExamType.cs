using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class ExamType
    {
        public ExamType()
        {
            ExamMarks = new HashSet<ExamMarks>();
        }

        public int ExamTypeId { get; set; }
        public string ExamType1 { get; set; }

        public virtual ICollection<ExamMarks> ExamMarks { get; set; }
    }
}
