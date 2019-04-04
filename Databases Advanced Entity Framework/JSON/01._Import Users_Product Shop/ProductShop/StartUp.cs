using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //string inputJsonUsers = File.ReadAllText(@"D:\Softuni\C# Database 2019\Databases Advanced Entity Framework\JSON\01._Import Users_Product Shop\ProductShop\Datasets\users.json");
            //string inputJsonProducts = File.ReadAllText(@"D:\Softuni\C# Database 2019\Databases Advanced Entity Framework\JSON\01._Import Users_Product Shop\ProductShop\Datasets\products.json");
            //string inputJsonCategories = File.ReadAllText(@"D:\Softuni\C# Database 2019\Databases Advanced Entity Framework\JSON\01._Import Users_Product Shop\ProductShop\Datasets\categories.json");
            //string inputJsonCP = File.ReadAllText(@"D:\Softuni\C# Database 2019\Databases Advanced Entity Framework\JSON\01._Import Users_Product Shop\ProductShop\Datasets\categories-products.json");

            //ImportUsers(context, inputJsonUsers);
            //ImportProducts(context, inputJsonProducts);
            //ImportCategories(context, inputJsonCategories);
            //ImportCategoryProducts(context, inputJsonCP);

            var result = GetUsersWithProducts(context);
            Console.WriteLine(result);
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(users);
            var importedUsers = context.SaveChanges();

            return $"Successfully imported {importedUsers}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);
            var validProducts = products
                .Where(x => x.Name.Trim().Length >= 3 && x.Name != null)
                .ToList();
            context.Products.AddRange(products);
            var importedUsers = context.SaveChanges();

            return $"Successfully imported {importedUsers}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);
            var validCategories = categories
                .Where(x => x.Name != null && x.Name.Trim().Length >= 3 && x.Name.Trim().Length <= 15)
                .ToList();
            context.Categories.AddRange(validCategories);
            var importedUsers = context.SaveChanges();

            return $"Successfully imported {importedUsers}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoriesProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);
            var validCategoriesProducts = new List<CategoryProduct>();

            var categoryExists = new HashSet<int>(
                   context.Categories.Select(x => x.Id)
                 );

            var productExists = new HashSet<int>(
                    context.Products.Select(x => x.Id)
                );


            foreach (var item in categoriesProducts)
            {
                if (categoryExists.Contains(item.CategoryId) && productExists.Contains(item.ProductId))
                {
                    validCategoriesProducts.Add(item);
                }
            }
            context.CategoryProducts.AddRange(validCategoriesProducts);
            var importedUsers = context.SaveChanges();

            return $"Successfully imported {importedUsers}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context
                .Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new
                {
                    Name = x.Name,
                    Price = x.Price,
                    Seller = x.Seller.FirstName + ' ' + x.Seller.LastName
                })
                .ToList();

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string json = JsonConvert.SerializeObject(products, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
            return json;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context
                .Users
                .Where(x => x.ProductsSold.Any(s => s.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold
                        //.Where(ps => ps.Buyer != null)    
                        .Select(y => new
                        {
                            Name = y.Name,
                            Price = y.Price,
                            BuyerFirstName = y.Buyer.FirstName,
                            BuyerLastName = y.Buyer.LastName
                        })
                        .ToArray()
                })
                .ToArray();

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string json = JsonConvert.SerializeObject(users, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
            return json;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context
                .Categories
                .OrderByDescending(x => x.CategoryProducts.Count)
                .Select(x => new {
                    Category = x.Name,
                    ProductsCount = x.CategoryProducts.Count,
                    AveragePrice = Math.Round(x.CategoryProducts.Average(s => s.Product.Price),2).ToString(),
                    TotalRevenue = Math.Round(x.CategoryProducts.Sum(s => s.Product.Price), 2).ToString()
                })
                .ToArray();

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string json = JsonConvert.SerializeObject(categories, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
            return json;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context
                .Users
                .Where(x => x.ProductsSold.Count > 0 && x.ProductsSold.Any(s => s.Buyer != null))
                .OrderByDescending(x => x.ProductsSold.Count(s => s.Buyer != null))
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                                                          
                    SoldProducts = new
                    {
                        Count = x.ProductsSold.Count(s => s.Buyer != null),
                        Products = x.ProductsSold
                            .Where(pr => pr.Buyer != null)
                            .Select(z => new {
                            Name = z.Name,
                            Price = z.Price
                        })
                    }
                })
                .ToArray();

            var result = new
            {
                UsersCount = users.Length,
                Users = users
            };

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string json = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });
            return json;
        }
    }
}