using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class Sales
    {
        public Sales()
        {
            Cash = new HashSet<Cash>();
        }

        public int SalesId { get; set; }
        public DateTime TransDate { get; set; }
        public decimal? Amount { get; set; }
        public int? InvId { get; set; }
        public int? AccRecId { get; set; }
        public int? CashId { get; set; }
        public decimal? CashAmount { get; set; }
        public decimal? AccRecAmount { get; set; }

        public virtual AccountsReceivable AccRec { get; set; }
        public virtual Cash CashNavigation { get; set; }
        public virtual Inventory Inv { get; set; }
        public virtual ICollection<Cash> Cash { get; set; }
    }
}
