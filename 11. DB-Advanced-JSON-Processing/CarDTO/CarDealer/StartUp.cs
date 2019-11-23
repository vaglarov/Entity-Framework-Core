using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO.Import;
using CarDealer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using CarDealer.DTO.Export;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.ExpressionTranslators.Internal;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace CarDealer
{
    public class StartUp
    {
        public const string successMessage = "Successfully imported {0}.";

        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<CarDealerProfile>(); });

            string suppliersJson = File.ReadAllText("./../../../Datasets/suppliers.json");
            string partsJson = File.ReadAllText("./../../../Datasets/parts.json");
            string carsJson = File.ReadAllText("./../../../Datasets/cars.json");
            string customersJson = File.ReadAllText("./../../../Datasets/customers.json");
            string salesJson = File.ReadAllText("./../../../Datasets/sales.json");

            using (var context = new CarDealerContext())
            {
               // Console.WriteLine(ImportSuppliers(context, suppliersJson));
               //Console.WriteLine(ImportParts(context, partsJson));
              // Console.WriteLine(ImportCars(context, carsJson));
               //Console.WriteLine(ImportCustomers(context, customersJson));
               Console.WriteLine(ImportSales(context, salesJson));
              // Console.WriteLine(GetOrderedCustomers(context));
               //Console.WriteLine(GetCarsFromMakeToyota(context));
               //Console.WriteLine(GetLocalSuppliers(context));
               //Console.WriteLine(GetCarsWithTheirListOfParts(context));
             // Console.WriteLine(GetTotalSalesByCustomer(context));
              // Console.WriteLine(GetSalesWithAppliedDiscount(context));
            }
        }

        //9
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            Supplier[] suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);

            context.Suppliers.AddRange(suppliers);

            var importedSuppliers = context.SaveChanges();

            return string.Format(successMessage, importedSuppliers);
        }

        //10
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            int[] suppliersIds = context.Suppliers
                .Select(s => s.Id)
                .ToArray();

            Part[] parts = JsonConvert.DeserializeObject<Part[]>(inputJson)
                .Where(p => suppliersIds.Contains(p.SupplierId))
                .ToArray();

            context.Parts.AddRange(parts);

            var importedParts = context.SaveChanges();

            return string.Format(successMessage, importedParts);
        }

        //11
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var partsId = context.Parts
                .Select(p => p.Id);

            var cars = new List<Car>();

            CarDto[] carsDto = JsonConvert.DeserializeObject<CarDto[]>(inputJson);

            foreach (var carDto in carsDto)
            {
                Car car = Mapper.Map<Car>(carDto);

                foreach (var partId in carDto.PartsId.Distinct())
                {
                    if (partsId.Contains(partId))
                    {
                        car.PartCars.Add(new PartCar()
                        {
                            PartId = partId,
                            Car = car
                        });
                    }
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);

            context.SaveChanges();

            var importedCars = cars.Count;

            return string.Format(successMessage, importedCars);
        }

        //12
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            Customer[] customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            context.Customers.AddRange(customers);

            var importedCustomers = context.SaveChanges();

            return string.Format(successMessage, importedCustomers);
        }

        //13
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            Sale[] sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.AddRange(sales);

            var importedSales = context.SaveChanges();

            return string.Format(successMessage, importedSales);

        }

        //14
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = c.IsYoungDriver
                }).ToArray();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        //15
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            //var cars = context.Cars
            //    .Where(c => c.Make == "Toyota")
            //    .Select(c => new CarDto()
            //    {
            //        Id = c.Id,
            //        Make = c.Make,
            //        Model = c.Model,
            //        TravelledDistance = c.TravelledDistance
            //    })
            //    .OrderBy(c => c.Model)
            //    .ThenByDescending(c => c.TravelledDistance)
            //    .ToArray();


            var toyotas = context.Cars
             .Where(c => string.Compare(c.Make, "Toyota", true) == 0)
             .OrderBy(c => c.Model)
             .ThenByDescending(c => c.TravelledDistance)
             .ToList();

            string json = JsonConvert.SerializeObject(toyotas, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            });

            return json;
        }

        //16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new SupplierDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                }).ToArray();

            return JsonConvert.SerializeObject(suppliers, Formatting.Indented);
        }

        //17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Select(c => new CarPartDto()
                {
                    Car = Mapper.Map<CarDto>(c),
                    Parts = Mapper.Map<PartDto[]>(c.PartCars.Select(pc => pc.Part))
                }).ToArray();

            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        //18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Select(c => new CustomerDto()
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.SpentMoney)
                .ThenByDescending(c => c.BoughtCars)
                .ToArray();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        //19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(s => Mapper.Map<SaleDto>(s))
                .ToArray();

            return JsonConvert.SerializeObject(sales, Formatting.Indented);
        }
    }
}