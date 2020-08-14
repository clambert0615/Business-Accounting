using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingProgram.Models
{
    public class AccRecReiptsView
    {
        public AccountsReceivable AR {get; set;}
        public List<Arreceipts> Recipts { get; set; }
    }
}
