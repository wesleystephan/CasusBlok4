using CasusBlok4.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CasusBlok4.Models
{
    public class TransactionAddProductViewModel
    {
        public IEnumerable<Product> Products;
        public IEnumerable<SelectListItem> GetSelectListItems()
        {
            return Products?.Select(q => new SelectListItem(q.Name, q.Id.ToString())) ?? Array.Empty<SelectListItem>();
        }

        [Required]
        public int SelectedProductId { get; set; }
        
        [Required]
        [Range(0, 15)]
        public byte NumberOfProducts { get; set; }
        
        [Range(0, 150)]
        public short? Points { get; set; }

        public bool IsForSell { get; set; }
    }
}
