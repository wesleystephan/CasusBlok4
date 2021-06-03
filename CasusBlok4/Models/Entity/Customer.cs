using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Models.Entity
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short Saldo { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
