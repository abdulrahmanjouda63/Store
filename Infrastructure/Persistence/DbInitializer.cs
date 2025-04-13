using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _context;

        public DbInitializer(StoreDbContext context)
        {
            _context = context;
        }
        public async Task InitializeAsync()
        {
            try
            {
                // Create Database If it does not exist && Apply To Any Pending Migrations

                if ((await _context.Database.GetPendingMigrationsAsync()).Any())
                {
                    await _context.Database.MigrateAsync();
                }
                // Data Seeding

                // Seeding ProductTypes From JSON Files

                if (!await _context.ProductTypes.AnyAsync())
                {
                    // 1. Read All Data From types JSON File as String
                    var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");
                    // 2. Transform String To C# Objects [ProductTypes]

                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    // 3. Add List<ProductTypes> To Database
                    if (types is not null && types.Any())
                    {
                        await _context.ProductTypes.AddRangeAsync(types);
                        await _context.SaveChangesAsync();
                    }
                }

                // Seeding ProductBrands From JSON Files

                if (!await _context.ProductBrands.AnyAsync())
                {
                    // 1. Read All Data From Brands JSON File as String
                    var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");
                    // 2. Transform String To C# Objects [ProductBrands]

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    // 3. Add List<ProductBrands> To Database
                    if (brands is not null && brands.Any())
                    {
                        await _context.ProductBrands.AddRangeAsync(brands);
                        await _context.SaveChangesAsync();
                    }
                }

                // Seeding Products From JSON Files

                if (!await _context.Products.AnyAsync())
                {
                    // 1. Read All Data From Products JSON File as String
                    var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");
                    // 2. Transform String To C# Objects [Products]

                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    // 3. Add List<Products> To Database
                    if (products is not null && products.Any())
                    {
                        await _context.Products.AddRangeAsync(products);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred during database initialization: {ex.Message}");
                throw;
            }
        }
    }
}
