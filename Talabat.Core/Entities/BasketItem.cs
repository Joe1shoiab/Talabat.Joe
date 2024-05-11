using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class BasketItem : BaseEntity
    {
        public string productName { get; set; }
        //public string Description { get; set; }
        public string Quantity { get; set; } // to choose more items
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }

    }
}
