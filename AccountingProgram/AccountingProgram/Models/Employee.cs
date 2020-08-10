using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Wages = new HashSet<Wages>();
        }

        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? Birthdate { get; set; }
        public int? Ssn { get; set; }

        public virtual ICollection<Wages> Wages { get; set; }
    }
}
