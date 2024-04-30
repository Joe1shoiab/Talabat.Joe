using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications()
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
        }
        
        public ProductWithBrandAndCategorySpecifications(int id) : base(p => p.Id == id)
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
        }

        public ProductWithBrandAndCategorySpecifications(ProductSpecParams productSpecParams)
            : base(p =>
                           (!productSpecParams.BrandId.HasValue || p.BrandId == productSpecParams.BrandId) &&
                           (!productSpecParams.CategoryId.HasValue || p.CategoryId == productSpecParams.CategoryId) &&
                           (string.IsNullOrEmpty(productSpecParams.Search) || p.Name.ToLower().Contains(productSpecParams.Search))
                       )
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
            if(productSpecParams.Sort != null) 
            switch (productSpecParams.Sort)
            {
                case "priceAsc":
                    OrderBy = p => p.Price;
                    break;
                case "priceDesc":
                    OrderByDescending = p => p.Price;
                    break;
                default:
                    OrderBy = p => p.Name;
                    break;
            }
            ApplyPagination(productSpecParams.PageSize * (productSpecParams.PageIndex - 1), productSpecParams.PageSize);
            
        }

    }
}
