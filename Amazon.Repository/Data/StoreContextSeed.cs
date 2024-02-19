using Amazon.Core.Entities;
using Amazon.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Amazon.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {

            if (!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Amazon.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);


                if (brands is not null && brands.Count > 0)
                {
                    foreach (var brand in brands)
                        await context.Set<ProductBrand>().AddAsync(brand);

                    await context.SaveChangesAsync();
                }

            }

            if (!context.ProductTypes.Any())
            {
                var typsData = File.ReadAllText("../Amazon.Repository/Data/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typsData);


                if (types is not null && types.Count > 0)
                {
                    foreach (var type in types)
                        await context.Set<ProductType>().AddAsync(type);

                    await context.SaveChangesAsync();
                }

            }

            if (!context.Products.Any())
            {
                var productsData = File.ReadAllText("../Amazon.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);


                if (products is not null && products.Count > 0)
                {
                    foreach (var product in products)
                        await context.Set<Product>().AddAsync(product);

                    await context.SaveChangesAsync();
                }

            }

            if (!context.DeliveryMethods.Any())
            {
                var DeliveryMethodsData = File.ReadAllText("../Amazon.Repository/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);


                if (deliveryMethods?.Count > 0 )
                {
                    foreach (var deliveryMethod in deliveryMethods)
                        await context.Set<DeliveryMethod>().AddAsync(deliveryMethod);

                    await context.SaveChangesAsync();
                }

            }

        }
    }
}
