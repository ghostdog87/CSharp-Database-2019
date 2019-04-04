using System;
using ProductShop.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(x => {
                x.AddProfile<ProductShopProfile>();
            });

            using (ProductShopContext context = new ProductShopContext())
            {
                //string inputXml1 = File.ReadAllText(@"D:\Softuni\C# Database 2019\Databases Advanced Entity Framework\XML\ProductShop - Skeleton\ProductShop\Datasets\users.xml");
                //string inputXml2 = File.ReadAllText(@"D:\Softuni\C# Database 2019\Databases Advanced Entity Framework\XML\ProductShop - Skeleton\ProductShop\Datasets\products.xml");
                //string inputXml3 = File.ReadAllText(@"D:\Softuni\C# Database 2019\Databases Advanced Entity Framework\XML\ProductShop - Skeleton\ProductShop\Datasets\categories.xml");
                //string inputXml4 = File.ReadAllText(@"D:\Softuni\C# Database 2019\Databases Advanced Entity Framework\XML\ProductShop - Skeleton\ProductShop\Datasets\categories-products.xml");

                //string result1 = ImportUsers(context, inputXml1);
                //string result2 = ImportProducts(context, inputXml2);
                //string result3 = ImportCategories(context, inputXml3);
                //string result4 = ImportCategoryProducts(context, inputXml4);
                string result = GetCategoriesByProductsCount(context);

                //System.Console.WriteLine(result1);
                //System.Console.WriteLine(result2);
                //System.Console.WriteLine(result3);
                //Console.WriteLine(result4);
                Console.WriteLine(result);
            }
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportUserDto[]),new XmlRootAttribute("Users"));

            var importedUsers = (ImportUserDto[])serializer.Deserialize(new StringReader(inputXml));

            var users = importedUsers.Select(Mapper.Map<User>).ToList();

            context.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportProductDto[]), new XmlRootAttribute("Products"));

            var importedProducts = (ImportProductDto[])serializer.Deserialize(new StringReader(inputXml));

            var products = importedProducts.Select(Mapper.Map<Product>).ToList();

            context.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCategoryDto[]), new XmlRootAttribute("Categories"));

            var importedCategories = (ImportCategoryDto[])serializer.Deserialize(new StringReader(inputXml));

            var categories = importedCategories.Select(Mapper.Map<Category>).ToList();

            context.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCategoryProductDto[]), new XmlRootAttribute("CategoryProducts"));

            var importedCategoryProduct = (ImportCategoryProductDto[])serializer.Deserialize(new StringReader(inputXml));

            var categoryProducts = importedCategoryProduct.Select(Mapper.Map<CategoryProduct>).ToList();

            context.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProductsInRangeDto[]), new XmlRootAttribute("Products"));

            var products = context
                .Products                
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new ProductsInRangeDto
                {
                    Name = x.Name,
                    Price = x.Price,
                    Buyer = x.Buyer.FirstName + " " + x.Buyer.LastName
                })
                .Take(10)
                .ToArray();

            var sb = new StringBuilder();

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(
                new[] {new XmlQualifiedName("")});

            serializer.Serialize(new StringWriter(sb), products, namespaces);

            return sb.ToString();
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UsersDto[]), new XmlRootAttribute("Users"));

            var users = context
                .Users
                .Where(x => x.ProductsSold.Any(s => s.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new UsersDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold  
                        .Select(y => new ProductsDto()
                        {
                            Name = y.Name,
                            Price = y.Price
                        })
                        .ToArray()
                })
                .Take(5)
                .ToArray();

            var sb = new StringBuilder();

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(
                new[] { new XmlQualifiedName("") });

            serializer.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString();
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CategoriesByProductsCountDto[]), new XmlRootAttribute("Categories"));

            var categories = context
                .Categories
                .Select(x => new CategoriesByProductsCountDto
                {
                    Name = x.Name,
                    Count = x.CategoryProducts.Count,
                    AveragePrice = x.CategoryProducts.Average(s => s.Product.Price),
                    TotalRevenue = x.CategoryProducts.Sum(s => s.Product.Price)
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToArray();

            var sb = new StringBuilder();

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(
                new[] { new XmlQualifiedName("") });

            serializer.Serialize(new StringWriter(sb), categories, namespaces);

            return sb.ToString();
        }
    }
}