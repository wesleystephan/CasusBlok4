using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SubCategorie> SubCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
