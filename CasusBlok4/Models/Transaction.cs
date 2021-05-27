using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Models
{
    public class Transaction
    {
        public string TransactionId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
    }
}
