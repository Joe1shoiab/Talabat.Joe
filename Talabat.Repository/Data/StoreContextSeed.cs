using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDbContext _dbContext)
        {
            if (!_dbContext.ProductBrands.Any()) // you can use .Count() instead of .Any() with better performance
            {
                var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);

                if (brands?.Count()>0)
                {
                    foreach (var item in brands)
                    {
                        _dbContext.ProductBrands.Add(item);
                    } 
                }
                await _dbContext.SaveChangesAsync(); 

            }

            if (!_dbContext.ProductCategories.Any()) // you can use .Count() instead of .Any() with better performance
            {
                var CategoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoriesData);

                if (categories?.Count() > 0)
                {
                    foreach (var item in categories)
                    {
                        _dbContext.ProductCategories.Add(item);
                    }
                }
                await _dbContext.SaveChangesAsync();
            }

            if (!_dbContext.Products.Any()) // you can use .Count() instead of .Any() with better performance
            {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                if (products?.Count() > 0)
                {
                    foreach (var item in products)
                    {
                        _dbContext.Products.Add(item);
                    }
                }
                await _dbContext.SaveChangesAsync();
            }

        }
    }
}
