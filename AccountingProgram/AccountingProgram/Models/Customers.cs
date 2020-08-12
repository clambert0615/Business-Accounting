using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class Customers
    {
        public Customers()
        {
            AccountsReceivable = new HashSet<AccountsReceivable>();
        }

        public int CustId { get; set; }
        public string Name { get; set; }
        public string StreetAdd { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public virtual ICollection<AccountsReceivable> AccountsReceivable { get; set; }
    }
}
