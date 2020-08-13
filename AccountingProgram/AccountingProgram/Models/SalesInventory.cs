using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class SalesInventory
    {
        public int SalesInventoryId { get; set; }
        public int? SalesId { get; set; }
        public int? InventoryId { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual Sales Sales { get; set; }
    }
}
