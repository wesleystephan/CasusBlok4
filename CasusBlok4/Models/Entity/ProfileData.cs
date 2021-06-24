using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Models.Entity
{
    public class ProfileData
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName {get; set; }
        public string LastName { get; set; }
        public int Balans { get; set; }
        public int? AccountTypeId { get; set; }
        public int? MemberCardId { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public DateTimeOffset? AccountCreated { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
