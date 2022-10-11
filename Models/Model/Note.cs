using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class Note
    {
        public int NoteId { get; set; }
        public string UserEmail { get; set; }
        public string Note1 { get; set; }
        public string NoteStatus { get; set; }
        public DateTime? RemainDate { get; set; }
        public DateTime? TargetDate { get; set; }
    }
}
