using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class Cash
    {
        public Cash()
        {
            Payments = new HashSet<Payments>();
            SalesNavigation = new HashSet<Sales>();
            Wages = new HashSet<Wages>();
        }

        public int Id { get; set; }
        public DateTime? TransDate { get; set; }
        public decimal? Deposit { get; set; }
        public decimal? Withdrawl { get; set; }
        public int? SalesId { get; set; }

        public virtual Sales Sales { get; set; }
        public virtual ICollection<Payments> Payments { get; set; }
        public virtual ICollection<Sales> SalesNavigation { get; set; }
        public virtual ICollection<Wages> Wages { get; set; }
    }
}
