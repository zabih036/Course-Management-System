using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class Fees
    {
        public int FeesId { get; set; }
        public int? StudentId { get; set; }
        public int? ClassId { get; set; }
        public int? Paid { get; set; }
        public DateTime? Date { get; set; }
        public int? BillNo { get; set; }
        public int? Discount { get; set; }
        public string Description { get; set; }

        public virtual Class Class { get; set; }
        public virtual Student Student { get; set; }
    }
}
