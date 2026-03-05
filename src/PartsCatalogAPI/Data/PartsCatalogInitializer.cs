using System;
using System.Collections.Generic;
using System.Data.Entity;
using PartsCatalogAPI.Models;

namespace PartsCatalogAPI.Data
{
    public class PartsCatalogInitializer : DropCreateDatabaseIfModelChanges<PartsCatalogContext>
    {
        protected override void Seed(PartsCatalogContext context)
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Name = "Brakes",
                    Description = "Brake pads, rotors, calipers and brake system components",
                    ImageUrl = "/images/categories/brakes.jpg",
                    IsActive = true
                },
                new Category
                {
                    Name = "Engine Parts",
                    Description = "Engine components, filters, belts and accessories",
                    ImageUrl = "/images/categories/engine.jpg",
                    IsActive = true
                },
                new Category
                {
                    Name = "Suspension",
                    Description = "Shocks, struts, control arms and suspension components",
                    ImageUrl = "/images/categories/suspension.jpg",
                    IsActive = true
                },
                new Category
                {
                    Name = "Electrical",
                    Description = "Batteries, alternators, starters and electrical components",
                    ImageUrl = "/images/categories/electrical.jpg",
                    IsActive = true
                },
                new Category
                {
                    Name = "Oil & Fluids",
                    Description = "Motor oil, transmission fluid, coolant and other fluids",
                    ImageUrl = "/images/categories/fluids.jpg",
                    IsActive = true
                }
            };

            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();

            var products = new List<Product>
            {
                // Brakes
                new Product
                {
                    Name = "Premium Brake Pad Set",
                    Description = "High-performance ceramic brake pads for superior stopping power",
                    Price = 89.99m,
                    StockQuantity = 45,
                    Sku = "BRK-001",
                    CategoryId = 1,
                    ImageUrl = "/images/products/brake-pads.jpg",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new Product
                {
                    Name = "Front Brake Rotor Pair",
                    Description = "Ventilated disc brake rotors for front axle",
                    Price = 149.99m,
                    StockQuantity = 30,
                    Sku = "BRK-002",
                    CategoryId = 1,
                    ImageUrl = "/images/products/brake-rotors.jpg",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                // Engine Parts
                new Product
                {
                    Name = "Oil Filter",
                    Description = "High-efficiency oil filter with enhanced filtration",
                    Price = 12.99m,
                    StockQuantity = 200,
                    Sku = "ENG-001",
                    CategoryId = 2,
                    ImageUrl = "/images/products/oil-filter.jpg",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new Product
                {
                    Name = "Air Filter",
                    Description = "Performance air filter for improved engine breathing",
                    Price = 24.99m,
                    StockQuantity = 150,
                    Sku = "ENG-002",
                    CategoryId = 2,
                    ImageUrl = "/images/products/air-filter.jpg",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new Product
                {
                    Name = "Serpentine Belt",
                    Description = "Durable rubber belt for accessory drive system",
                    Price = 34.99m,
                    StockQuantity = 75,
                    Sku = "ENG-003",
                    CategoryId = 2,
                    ImageUrl = "/images/products/serpentine-belt.jpg",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                // Suspension
                new Product
                {
                    Name = "Front Shock Absorber",
                    Description = "Gas-charged shock absorber for smooth ride quality",
                    Price = 79.99m,
                    StockQuantity = 60,
                    Sku = "SUS-001",
                    CategoryId = 3,
                    ImageUrl = "/images/products/shock-absorber.jpg",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new Product
                {
                    Name = "Rear Strut Assembly",
                    Description = "Complete strut assembly with coil spring",
                    Price = 189.99m,
                    StockQuantity = 40,
                    Sku = "SUS-002",
                    CategoryId = 3,
                    ImageUrl = "/images/products/strut-assembly.jpg",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                // Electrical
                new Product
                {
                    Name = "12V Car Battery",
                    Description = "Maintenance-free lead-acid battery with 600 CCA",
                    Price = 129.99m,
                    StockQuantity = 50,
                    Sku = "ELC-001",
                    CategoryId = 4,
                    ImageUrl = "/images/products/battery.jpg",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new Product
                {
                    Name = "Alternator",
                    Description = "High-output alternator for reliable charging",
                    Price = 199.99m,
                    StockQuantity = 25,
                    Sku = "ELC-002",
                    CategoryId = 4,
                    ImageUrl = "/images/products/alternator.jpg",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                // Oil & Fluids
                new Product
                {
                    Name = "Synthetic Motor Oil 5W-30",
                    Description = "Full synthetic motor oil - 5 quart bottle",
                    Price = 29.99m,
                    StockQuantity = 100,
                    Sku = "FLD-001",
                    CategoryId = 5,
                    ImageUrl = "/images/products/motor-oil.jpg",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new Product
                {
                    Name = "Coolant/Antifreeze",
                    Description = "Pre-mixed coolant/antifreeze - 1 gallon",
                    Price = 19.99m,
                    StockQuantity = 80,
                    Sku = "FLD-002",
                    CategoryId = 5,
                    ImageUrl = "/images/products/coolant.jpg",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                }
            };

            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
