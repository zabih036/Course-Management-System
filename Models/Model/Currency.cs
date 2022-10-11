using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class Currency
    {
        public Currency()
        {
            Dealing = new HashSet<Dealing>();
        }

        public int CurrencyId { get; set; }
        public string Currency1 { get; set; }

        public virtual ICollection<Dealing> Dealing { get; set; }
    }
}
