using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class BasketItem : BaseEntity
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string productName { get; set; }
        //public string Description { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1") ]
        public int Quantity { get; set; } // to choose more items
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be at least 0.1")]
        public decimal Price { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }

    }
}
