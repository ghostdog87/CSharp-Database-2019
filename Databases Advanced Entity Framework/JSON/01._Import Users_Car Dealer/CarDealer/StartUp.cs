using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {

        public static void Main(string[] args)
        {

            Mapper.Initialize(cfg => cfg.AddProfile(new CarDealerProfile()));

            var context = new CarDealerContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //string inputJsonSuppliers = File.ReadAllText(@"D:\Softuni\C# Database 2019\Databases Advanced Entity Framework\JSON\01._Import Users_Car Dealer\CarDealer\Datasets\suppliers.json");
            //string inputJsonParts = File.ReadAllText(@"D:\Softuni\C# Database 2019\Databases Advanced Entity Framework\JSON\01._Import Users_Car Dealer\CarDealer\Datasets\parts.json");
            //string inputJsonCars = File.ReadAllText(@"D:\Softuni\C# Database 2019\Databases Advanced Entity Framework\JSON\01._Import Users_Car Dealer\CarDealer\Datasets\cars.json");
            //string inputJsonCustomers = File.ReadAllText(@"D:\Softuni\C# Database 2019\Databases Advanced Entity Framework\JSON\01._Import Users_Car Dealer\CarDealer\Datasets\customers.json");
            //string inputJsonSales = File.ReadAllText(@"D:\Softuni\C# Database 2019\Databases Advanced Entity Framework\JSON\01._Import Users_Car Dealer\CarDealer\Datasets\sales.json");

            //string result1 = ImportSuppliers(context, inputJsonSuppliers);
            //string result2 = ImportParts(context, inputJsonParts);
            //string result3 = ImportCars(context, inputJsonCars);
            //string result4 = ImportCustomers(context, inputJsonCustomers);
            //string result5 = ImportSales(context, inputJsonSales);
            //Console.WriteLine(result1);
            //Console.WriteLine(result2);
            //Console.WriteLine(result3);
            //Console.WriteLine(result4);
            //Console.WriteLine(result5);

            string result = GetSalesWithAppliedDiscount(context);
            Console.WriteLine(result);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(users);
            var importedUsers = context.SaveChanges();

            return $"Successfully imported {importedUsers}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);
            var suppliers = new HashSet<int>(
                context.Suppliers.Select(x => x.Id)
                );

            foreach (var part in parts)
            {
                if (suppliers.Contains(part.SupplierId))
                {
                    context.Parts.Add(part);
                }
            }

            var importedParts = context.SaveChanges();

            return $"Successfully imported {importedParts}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var cars = JsonConvert.DeserializeObject<CarsDto[]>(inputJson);
            var mappedCars = new List<Car>();

            foreach (var car in cars)
            {
                Car vehicle = Mapper.Map<CarsDto, Car>(car);
                mappedCars.Add(vehicle);

                var partIds = car
                .PartsId
                .Distinct()
                .ToList();

                if (partIds == null)
                {
                    continue;
                }

                partIds.ForEach(pid =>
                {
                    var currentPair = new PartCar()
                    {
                        Car = vehicle,
                        PartId = pid
                    };
                    vehicle.PartCars.Add(currentPair);
                }
                );
            }

            context.Cars.AddRange(mappedCars);
            context.SaveChanges();

            int affectedRows = context.Cars.Count();
            return $"Successfully imported {affectedRows}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

            context.Customers.AddRange(customers);
            var importedUsers = context.SaveChanges();

            return $"Successfully imported {importedUsers}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.Sales.AddRange(sales);
            var importedUsers = context.SaveChanges();

            return $"Successfully imported {importedUsers}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {

            //Get all customers ordered by their birth date ascending.If two customers are born on the same date first print those who are not young drivers(e.g.print experienced drivers first).Export the list of customers to JSON in the format provided below.

            var customers = context.
                Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new
                {
                    x.Name,
                    x.BirthDate,
                    x.IsYoungDriver
                })
                .ToArray();

            //DefaultContractResolver contractResolver = new DefaultContractResolver()
            //{
            //    NamingStrategy = new CamelCaseNamingStrategy()
            //};

            string json = JsonConvert.SerializeObject(customers, new JsonSerializerSettings()
            {
                //ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                DateFormatString = "dd/MM/yyyy"
            });
            return json;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {

            //Get all cars from make Toyota and order them by model alphabetically and by travelled distance descending.Export the list of cars to JSON in the format provided below.

            var cars = context
                .Cars
                .Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToArray();

            string json = JsonConvert.SerializeObject(cars, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });
            return json;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            //Get all suppliers that do not import parts from abroad.Get their id, name and the number of parts they can offer to supply.

            var suppliers = context
                .Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToArray();

            string json = JsonConvert.SerializeObject(suppliers, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                //NullValueHandling = NullValueHandling.Ignore
            });
            return json;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            //Get all cars along with their list of parts.For the car get only make, model and travelled distance and for the parts get only name and price(formatted to 2nd digit after the decimal point).

            var cars = context
                .Cars
                .Select(x => new
                {
                    car = new
                    {
                        x.Make,
                        x.Model,
                        x.TravelledDistance
                    },
                    parts = x.PartCars.Select(p => new
                    {
                        p.Part.Name,
                        Price = string.Format("{0:F2}", p.Part.Price)
                    })
                    .ToArray()
                })
                .ToArray();

            string json = JsonConvert.SerializeObject(cars, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                //NullValueHandling = NullValueHandling.Ignore
            });
            return json;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            //Get all customers that have bought at least 1 car and get their names, bought cars count and total spent money on cars.Order the result list by total spent money descending then by total bought cars again in descending order.

            var customers = context
                .Customers
                .Select(x => new
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpentMoney = x.Sales.Select(y => new
                    {
                        SpentMoneyPerCar = y.Car.PartCars.Select(z => z.Part.Price).Sum()
                    })
                    .ToArray()
                })
                .ToList();

            var result = customers.Select(x => new
            {
                x.FullName,
                x.BoughtCars,
                SpentMoney = x.SpentMoney.Select(y => y.SpentMoneyPerCar).Sum()
            })
            .OrderByDescending(x => x.SpentMoney)
            .ThenByDescending(x => x.BoughtCars)
            .ToList();

            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {

                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string json = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                //NullValueHandling = NullValueHandling.Ignore
            });
            return json;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {

            //Get first 10 sales with information about the car, customer and price of the sale with and without discount

            var sales = context.Sales
                .Take(10)
                .Select(x => new
                {
                    car = new
                    {
                        x.Car.Make,
                        x.Car.Model,
                        x.Car.TravelledDistance
                    },
                    customerName = x.Customer.Name,
                    Discount = $"{x.Discount:F2}",
                    price = $"{x.Car.PartCars.Sum(y => y.Part.Price):F2}",
                    priceWithDiscount = $"{x.Car.PartCars.Sum(y => y.Part.Price) - (x.Car.PartCars.Sum(y => y.Part.Price) * (x.Discount / 100)):F2}",
                })
                .ToList();

            //DefaultContractResolver contractResolver = new DefaultContractResolver()
            //{

            //    NamingStrategy = new CamelCaseNamingStrategy()
            //};

            string json = JsonConvert.SerializeObject(sales, new JsonSerializerSettings()
            {
                //ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });
            return json;
        }
    }
}