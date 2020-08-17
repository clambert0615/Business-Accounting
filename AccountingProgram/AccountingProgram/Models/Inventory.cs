using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class Inventory
    {
        public Inventory()
        {
            Invoice = new HashSet<Invoice>();
            InvoiceInventory = new HashSet<InvoiceInventory>();
            PayableInventory = new HashSet<PayableInventory>();
            Sales = new HashSet<Sales>();
            SalesInventory = new HashSet<SalesInventory>();
        }

        public int InvId { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public decimal Price { get; set; }
        public int? BackOrdered { get; set; }
        public int? Received { get; set; }
        public string Message { get; set; }

        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<InvoiceInventory> InvoiceInventory { get; set; }
        public virtual ICollection<PayableInventory> PayableInventory { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }
        public virtual ICollection<SalesInventory> SalesInventory { get; set; }
    }
}
