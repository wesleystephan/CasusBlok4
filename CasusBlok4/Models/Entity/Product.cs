using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Models.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short PointsWorth { get; set; }
        public int CategorieId { get; set; }
        public string SerialNumber { get; set; }

        public virtual Category Categorie { get; set; }
        public virtual ICollection<TransactionProduct> TransactionProducts { get; set; }
    }
}
