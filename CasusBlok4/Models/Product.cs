using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public short? PointsWorth { get; set; }
        public int? CategorieId { get; set; }
        public int? SubCategorieId { get; set; }
        public virtual Categorie Categorie { get; set; }
        public virtual SubCategorie SubCategorie { get; set; }
    }
}
