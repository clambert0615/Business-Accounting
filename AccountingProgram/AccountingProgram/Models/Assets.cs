using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class Assets
    {
        public int AssetId { get; set; }
        public string Type { get; set; }
        public DateTime? TransDate { get; set; }
        public string Description { get; set; }
        public decimal? Cost { get; set; }
        public decimal? AccDepreciation { get; set; }
        public decimal? UsefulLife { get; set; }
        public decimal? Balance { get; set; }
     
    }
}
