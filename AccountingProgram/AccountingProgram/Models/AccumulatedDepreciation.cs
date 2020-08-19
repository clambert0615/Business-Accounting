using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class AccumulatedDepreciation
    {
        public AccumulatedDepreciation()
        {
            Expenses = new HashSet<Expenses>();
        }

        public int AccDepId { get; set; }
        public int? LongTermAssetId { get; set; }
        public int? ExpenseId { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TotalAmount { get; set; }

        public virtual Expenses Expense { get; set; }
        public virtual LongTermAssets LongTermAsset { get; set; }
        public virtual ICollection<Expenses> Expenses { get; set; }
    }
}
