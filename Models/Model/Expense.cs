using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class Expense
    {
        public int ExpenseId { get; set; }
        public int Amount { get; set; }
        public DateTime? Date { get; set; }
        public int? ExpenseTypeId { get; set; }
        public string Description { get; set; }

        public virtual ExpenseType ExpenseType { get; set; }
    }
}
