using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Models
{
    public class SubCategorie
    {
        public int Id { get; set; }
        public int HeadCategorie { get; set; }
        public string Name { get; set; }

        public virtual Categorie Categorie { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
