using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public partial class LongTermAssets
    {
        public LongTermAssets()
        {
            AccumulatedDepreciation = new HashSet<AccumulatedDepreciation>();
        }

        public int LtassetId { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Balance { get; set; }
        public decimal? UsefulLife { get; set; }
        public decimal? LifeRemaining { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime PurchaseDate { get; set; }

        public virtual ICollection<AccumulatedDepreciation> AccumulatedDepreciation { get; set; }
    }
}
