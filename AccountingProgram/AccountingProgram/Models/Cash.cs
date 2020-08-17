using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public partial class Cash
    {
        public Cash()
        {
            Arreceipts = new HashSet<Arreceipts>();
            Payments = new HashSet<Payments>();
            SalesNavigation = new HashSet<Sales>();
            Wages = new HashSet<Wages>();
        }

        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime TransDate { get; set; }
        public decimal? Deposit { get; set; }
        public decimal? Withdrawl { get; set; }
        public int? SalesId { get; set; }
        public decimal? BeginAmount { get; set; }
        public decimal? Balance { get; set; }

        public virtual Sales Sales { get; set; }
        public virtual ICollection<Arreceipts> Arreceipts { get; set; }
        public virtual ICollection<Payments> Payments { get; set; }
        public virtual ICollection<Sales> SalesNavigation { get; set; }
        public virtual ICollection<Wages> Wages { get; set; }
    }
}
