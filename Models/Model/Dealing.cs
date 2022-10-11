using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class Dealing
    {
        public int DealingId { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public DateTime? Date { get; set; }
        public string Detail { get; set; }
        public int? EmployeeId { get; set; }
        public int? DealerId { get; set; }
        public int? CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual Dealer Dealer { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
