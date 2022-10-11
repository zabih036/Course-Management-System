using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class Dealer
    {
        public Dealer()
        {
            Dealing = new HashSet<Dealing>();
        }

        public int DealerId { get; set; }
        public string Dealer1 { get; set; }
        public string Mobile { get; set; }

        public virtual ICollection<Dealing> Dealing { get; set; }
    }
}
