﻿using System;
using System.Collections.Generic;

namespace AccountingAPI.Models
{
    public partial class Sales
    {
        public int SalesId { get; set; }
        public DateTime? TransDate { get; set; }
        public decimal? Amount { get; set; }
    }
}
