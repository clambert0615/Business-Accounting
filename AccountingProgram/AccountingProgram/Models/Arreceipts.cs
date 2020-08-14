using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public partial class Arreceipts
    {
        public int ArreciptsId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime ReceiptDate { get; set; }
        public decimal? Amount { get; set; }
        public int? SalesId { get; set; }
        public int? CashId { get; set; }
        public int? AccountsRecId { get; set; }

        public virtual AccountsReceivable AccountsRec { get; set; }
        public virtual Cash Cash { get; set; }
        public virtual Sales Sales { get; set; }
    }
}
