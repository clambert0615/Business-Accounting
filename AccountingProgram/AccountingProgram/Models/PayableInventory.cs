using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class PayableInventory
    {
        public int PayInvId { get; set; }
        public int? PayableId { get; set; }
        public int? InventoryId { get; set; }
        public int? InvQuantity { get; set; }
        public decimal? InvPrice { get; set; }
        public int? InvBackOrdered { get; set; }
        public int? InvReceived { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual AccountsPayable Payable { get; set; }
    }
}
