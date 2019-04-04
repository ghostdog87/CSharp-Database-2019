//ReSharper disable InconsistentNaming, CheckNamespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using CarDealer;
using CarDealer.Data;
using CarDealer.Models;

[TestFixture]
public class Test_000_001
{
    private IServiceProvider serviceProvider;

    private static readonly Assembly CurrentAssembly = typeof(StartUp).Assembly;

    [SetUp]
    public void Setup()
    {
        Mapper.Reset();
        Mapper.Initialize(cfg => cfg.AddProfile(GetType("CarDealerProfile")));

        this.serviceProvider = ConfigureServices<CarDealerContext>("CarDealer");
    }

    [Test]
    public void ExportOrderedCustomersZeroTests()
    {
        var context = this.serviceProvider.GetService<CarDealerContext>();

        SeedDatabase(context);

        var expectedOutputValue = "[{\"Name\":\"Louann Holzworth\",\"BirthDate\":\"01/10/1960\",\"IsYoungDriver\":false},{\"Name\":\"Donnetta Soliz\",\"BirthDate\":\"01/10/1963\",\"IsYoungDriver\":true},{\"Name\":\"Rema Revelle\",\"BirthDate\":\"01/05/1970\",\"IsYoungDriver\":true},{\"Name\":\"Carri Knapik\",\"BirthDate\":\"02/02/1972\",\"IsYoungDriver\":true},{\"Name\":\"Garret Capron\",\"BirthDate\":\"01/07/1975\",\"IsYoungDriver\":false},{\"Name\":\"Teddy Hobby\",\"BirthDate\":\"01/10/1975\",\"IsYoungDriver\":true},{\"Name\":\"Audrea Cardinal\",\"BirthDate\":\"01/10/1976\",\"IsYoungDriver\":false},{\"Name\":\"Francis Mckim\",\"BirthDate\":\"01/10/1977\",\"IsYoungDriver\":true},{\"Name\":\"Brett Brickley\",\"BirthDate\":\"01/10/1980\",\"IsYoungDriver\":true},{\"Name\":\"Kristian Engberg\",\"BirthDate\":\"03/10/1981\",\"IsYoungDriver\":false},{\"Name\":\"Hipolito Lamoreaux\",\"BirthDate\":\"01/08/1982\",\"IsYoungDriver\":true},{\"Name\":\"Zada Attwood\",\"BirthDate\":\"01/10/1982\",\"IsYoungDriver\":true},{\"Name\":\"Sylvie Mcelravy\",\"BirthDate\":\"01/10/1983\",\"IsYoungDriver\":false},{\"Name\":\"Rico Peer\",\"BirthDate\":\"01/10/1984\",\"IsYoungDriver\":true},{\"Name\":\"Hai Everton\",\"BirthDate\":\"01/10/1985\",\"IsYoungDriver\":true},{\"Name\":\"Lino Subia\",\"BirthDate\":\"21/12/1985\",\"IsYoungDriver\":false},{\"Name\":\"Jimmy Grossi\",\"BirthDate\":\"01/07/1986\",\"IsYoungDriver\":true},{\"Name\":\"Carole Witman\",\"BirthDate\":\"01/10/1987\",\"IsYoungDriver\":false},{\"Name\":\"Oren Perlman\",\"BirthDate\":\"01/10/1988\",\"IsYoungDriver\":true},{\"Name\":\"Natalie Poli\",\"BirthDate\":\"04/10/1990\",\"IsYoungDriver\":false},{\"Name\":\"Marcelle Griego\",\"BirthDate\":\"04/10/1990\",\"IsYoungDriver\":true},{\"Name\":\"Yvonne Mccalla\",\"BirthDate\":\"02/03/1992\",\"IsYoungDriver\":true},{\"Name\":\"Ann Mcenaney\",\"BirthDate\":\"02/03/1992\",\"IsYoungDriver\":true},{\"Name\":\"Cinthia Lasala\",\"BirthDate\":\"01/11/1992\",\"IsYoungDriver\":true},{\"Name\":\"Emmitt Benally\",\"BirthDate\":\"20/11/1993\",\"IsYoungDriver\":true},{\"Name\":\"Nisha Markwell\",\"BirthDate\":\"04/04/1994\",\"IsYoungDriver\":false},{\"Name\":\"Faustina Burgher\",\"BirthDate\":\"01/06/1994\",\"IsYoungDriver\":false},{\"Name\":\"Taina Achenbach\",\"BirthDate\":\"01/10/1994\",\"IsYoungDriver\":false},{\"Name\":\"Johnette Derryberry\",\"BirthDate\":\"01/10/1995\",\"IsYoungDriver\":true},{\"Name\":\"Daniele Zarate\",\"BirthDate\":\"05/10/1995\",\"IsYoungDriver\":false}]";

        var expectedOutput = JToken.Parse(expectedOutputValue);
        var actualOutputValue = StartUp.GetOrderedCustomers(context);
        var actualOutput = JToken.Parse(actualOutputValue);

        var expected = expectedOutput.ToString(Formatting.None);
        var actual = actualOutput.ToString(Formatting.None);

        Assert.That(actual, Is.EqualTo(expected).NoClip,
            $"{nameof(StartUp.GetOrderedCustomers)} output is incorrect!");
    }

    private static void SeedDatabase(CarDealerContext context)
    {
        var customersJson =
           "[{\"name\":\"Emmitt Benally\",\"birthDate\":\"1993-11-20T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Natalie Poli\",\"birthDate\":\"1990-10-04T00:00:00\",\"isYoungDriver\":false},{\"name\":\"Marcelle Griego\",\"birthDate\":\"1990-10-04T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Zada Attwood\",\"birthDate\":\"1982-10-01T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Hai Everton\",\"birthDate\":\"1985-10-01T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Lino Subia\",\"birthDate\":\"1985-12-21T00:00:00\",\"isYoungDriver\":false},{\"name\":\"Rico Peer\",\"birthDate\":\"1984-10-01T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Teddy Hobby\",\"birthDate\":\"1975-10-01T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Johnette Derryberry\",\"birthDate\":\"1995-10-01T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Audrea Cardinal\",\"birthDate\":\"1976-10-01T00:00:00\",\"isYoungDriver\":false},{\"name\":\"Francis Mckim\",\"birthDate\":\"1977-10-01T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Carole Witman\",\"birthDate\":\"1987-10-01T00:00:00\",\"isYoungDriver\":false},{\"name\":\"Oren Perlman\",\"birthDate\":\"1988-10-01T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Brett Brickley\",\"birthDate\":\"1980-10-01T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Kristian Engberg\",\"birthDate\":\"1981-10-03T00:00:00\",\"isYoungDriver\":false},{\"name\":\"Carri Knapik\",\"birthDate\":\"1972-02-02T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Rema Revelle\",\"birthDate\":\"1970-05-01T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Taina Achenbach\",\"birthDate\":\"1994-10-01T00:00:00\",\"isYoungDriver\":false},{\"name\":\"Yvonne Mccalla\",\"birthDate\":\"1992-03-02T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Nisha Markwell\",\"birthDate\":\"1994-04-04T00:00:00\",\"isYoungDriver\":false},{\"name\":\"Cinthia Lasala\",\"birthDate\":\"1992-11-01T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Daniele Zarate\",\"birthDate\":\"1995-10-05T00:00:00\",\"isYoungDriver\":false},{\"name\":\"Faustina Burgher\",\"birthDate\":\"1994-06-01T00:00:00\",\"isYoungDriver\":false},{\"name\":\"Jimmy Grossi\",\"birthDate\":\"1986-07-01T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Sylvie Mcelravy\",\"birthDate\":\"1983-10-01T00:00:00\",\"isYoungDriver\":false},{\"name\":\"Hipolito Lamoreaux\",\"birthDate\":\"1982-08-01T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Garret Capron\",\"birthDate\":\"1975-07-01T00:00:00\",\"isYoungDriver\":false},{\"name\":\"Donnetta Soliz\",\"birthDate\":\"1963-10-01T00:00:00\",\"isYoungDriver\":true},{\"name\":\"Louann Holzworth\",\"birthDate\":\"1960-10-01T00:00:00\",\"isYoungDriver\":false},{\"name\":\"Ann Mcenaney\",\"birthDate\":\"1992-03-02T00:00:00\",\"isYoungDriver\":true}]";

        var customers = JsonConvert.DeserializeObject<List<Customer>>(customersJson);

        context.Customers.AddRange(customers);
        context.SaveChanges();
    }

    private static Type GetType(string modelName)
    {
        var modelType = CurrentAssembly
            .GetTypes()
            .FirstOrDefault(t => t.Name == modelName);

        Assert.IsNotNull(modelType, $"{modelName} model not found!");

        return modelType;
    }

    private static IServiceProvider ConfigureServices<TContext>(string databaseName)
        where TContext : DbContext
    {
        var services = ConfigureDbContext<TContext>(databaseName);

        var context = services.GetService<TContext>();

        try
        {
            context.Model.GetEntityTypes();
        }
        catch (InvalidOperationException ex) when (ex.Source == "Microsoft.EntityFrameworkCore.Proxies")
        {
            services = ConfigureDbContext<TContext>(databaseName, useLazyLoading: true);
        }

        return services;
    }

    private static IServiceProvider ConfigureDbContext<TContext>(string databaseName, bool useLazyLoading = false)
        where TContext : DbContext
    {
        var services = new ServiceCollection();

        services
            .AddDbContext<TContext>(
                options => options
                    .UseInMemoryDatabase(databaseName)
                    .UseLazyLoadingProxies(useLazyLoading)
            );

        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider;
    }
}