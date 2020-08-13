using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class InvoiceInventory
    {
        public int InvInvId { get; set; }
        public int? InventoryId { get; set; }
        public int? InvoiceId { get; set; }
        public int? InventoryQty { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
