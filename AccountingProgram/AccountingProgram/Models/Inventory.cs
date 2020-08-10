using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class Inventory
    {
        public int InvId { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int? BackOrdered { get; set; }
        public int? Received { get; set; }
        public string Message { get; set; }
    }
}
