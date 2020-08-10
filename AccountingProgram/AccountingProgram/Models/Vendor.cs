using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class Vendor
    {
        public Vendor()
        {
            AccountsPayable = new HashSet<AccountsPayable>();
        }

        public int VenId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public virtual ICollection<AccountsPayable> AccountsPayable { get; set; }
    }
}
