using System;
using System.Collections.Generic;

namespace JahanInstitute
{
    public partial class ExpenseType
    {
        public ExpenseType()
        {
            Expense = new HashSet<Expense>();
        }

        public int ExpenseTypeId { get; set; }
        public string ExpenseType1 { get; set; }

        public virtual ICollection<Expense> Expense { get; set; }
    }
}
