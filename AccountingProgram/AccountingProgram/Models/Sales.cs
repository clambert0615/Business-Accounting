using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public partial class Sales
    {
        public Sales()
        {
            Cash = new HashSet<Cash>();
        }

        public int SalesId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime TransDate { get; set; }
        public decimal? Amount { get; set; }
        public int? InvId { get; set; }
        public int? AccRecId { get; set; }
        public int? CashId { get; set; }
        public decimal? CashAmount { get; set; }
        public decimal? AccRecAmount { get; set; }
        public decimal? SalesTax { get; set; }
        public decimal? Subtotal { get; set; }

        public virtual AccountsReceivable AccRec { get; set; }
        public virtual Cash CashNavigation { get; set; }
        public virtual Inventory Inv { get; set; }
        public virtual ICollection<Cash> Cash { get; set; }
    }
}
