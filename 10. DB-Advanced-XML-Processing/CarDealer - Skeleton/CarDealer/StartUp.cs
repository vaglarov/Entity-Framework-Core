using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Dtos.Export;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg =>
                               cfg.AddProfile<CarDealerProfile>());

            using (var db = new CarDealerContext())
            {
                //00 CreateDB
                // db.Database.EnsureDeleted();
                // db.Database.EnsureCreated();
                // db.SaveChanges();

                //Query 9. Import Suppliers
                // string suppliersXml = File.ReadAllText("./../../../Datasets/suppliers.xml");
                // string imputSuppliersXmlResult = ImportSuppliers(db, suppliersXml);
                // Console.WriteLine(imputSuppliersXmlResult);


                //Query 10.Import Parts
                //string partsXml = File.ReadAllText("./../../../Datasets/parts.xml");
                //string imputPartsXmlResult = ImportParts(db, partsXml);
                //Console.WriteLine(imputPartsXmlResult);

                //Query 11. Import Cars
               // string carsXml = File.ReadAllText("./../../../Datasets/cars.xml");
               // string imputCarsXmlResult = ImportCars(db, carsXml);
               // Console.WriteLine(imputCarsXmlResult);

                //Query 12.Import Customers
              // string customersXml = File.ReadAllText("./../../../Datasets/customers.xml");
              // string imputCustomersXmlResult = ImportCustomers(db, customersXml);
              // Console.WriteLine(imputCustomersXmlResult);

                //Query 13. Import Sales
                //  string salesXml = File.ReadAllText("./../../../Datasets/sales.xml");
                //  string imputSalesXmlResult = ImportSales(db, salesXml);
                //  Console.WriteLine(imputSalesXmlResult);

                //Query 14. Cars With Distance
                //  string exportCarWithDistanceXmlResult = GetCarsWithDistance(db);
                //  Console.WriteLine(exportCarWithDistanceXmlResult);

                //  Query 15.Cars from make BMW
                // string exportCarWithMakeWMWXmlResult = GetCarsFromMakeBmw(db);
                // Console.WriteLine(exportCarWithMakeWMWXmlResult);


                //Query 16. Local Suppliers
                // string exportLocalSuppliersXmlResult = GetLocalSuppliers(db);
                // Console.WriteLine(exportLocalSuppliersXmlResult);

                //Query 17. Cars with Their List of Parts
                //string exportCarWithListOfPartsXmlResult = GetCarsWithTheirListOfParts(db);
                //Console.WriteLine(exportCarWithListOfPartsXmlResult);

                //Query 18. Total Sales by Customer
             // string exportTotalSalesByCustomerXmlResult = GetTotalSalesByCustomer(db);
             // Console.WriteLine(exportTotalSalesByCustomerXmlResult);

                //Query 19. Sales with Applied Discount
                string exportGetSalesWithAppliedDiscountrXmlResult = GetSalesWithAppliedDiscount(db);
                Console.WriteLine(exportGetSalesWithAppliedDiscountrXmlResult);

            }
        }
        //09
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[])
                                                        , new XmlRootAttribute("Suppliers"));

            ImportSupplierDto[] supplierDtos;

            using (var reader = new StringReader(inputXml))
            {
                supplierDtos = (ImportSupplierDto[])xmlSerializer.Deserialize(reader);
            }

            var suppliers = Mapper.Map<Supplier[]>(supplierDtos);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }
        //10
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportPartDto[])
                                                        , new XmlRootAttribute("Parts"));

            ImportPartDto[] partsDtos;

            using (var reader = new StringReader(inputXml))
            {
                partsDtos = ((ImportPartDto[])xmlSerializer.Deserialize(reader))
                    .Where(p => context.Suppliers.Any(s => s.Id == p.SupplierId))
                .ToArray();
            }

            var parts = Mapper.Map<Part[]>(partsDtos);


            context.Parts.AddRange(parts);
            context.SaveChanges();


            return $"Successfully imported {parts.Length}";
        }
        //11
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCarDto[]), new XmlRootAttribute("Cars"));

            var carsDto = ((ImportCarDto[])serializer.Deserialize(new StringReader(inputXml)));

            var cars = new List<Car>();
            var partCars = new List<PartCar>();

            foreach (var carDto in carsDto)
            {
                Car car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TraveledDistance
                };

                var parts = carDto.Parts
                    .Where(pDto => context.Parts.Any(p => p.Id == pDto.Id))
                    .Select(p => p.Id)
                    .Distinct();

                foreach (var partId in parts)
                {
                    var partCar = new PartCar()
                    {
                        PartId = partId,
                        Car = car
                    };
                    partCars.Add(partCar);
                }

                cars.Add(car);
            }


            context.Cars.AddRange(cars);
            context.PartCars.AddRange(partCars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count}";
        }
        //12
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportCustomerDto[])
                                                      , new XmlRootAttribute("Customers"));

            ImportCustomerDto[] customerDtos;

            using (var reader = new StringReader(inputXml))
            {
                customerDtos = (ImportCustomerDto[])xmlSerializer.Deserialize(reader);
            }

            var customers = Mapper.Map<Customer[]>(customerDtos);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }
        //13
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportSaleDto[])
                                                   , new XmlRootAttribute("Sales"));

            var salesDto = ((ImportSaleDto[])xmlSerializer.Deserialize(new StringReader(inputXml)))
                  .Where(s => context.Cars.Find(s.CarId) != null);

            var sales = Mapper.Map<Sale[]>(salesDto);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}";
        }

        //14
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            StringBuilder outPutStringBuilder = new StringBuilder();

            var cars = context.Cars
            .Where(c => c.TravelledDistance >= 200000)
            .OrderBy(c => c.Make)
            .ThenBy(c => c.Model)
            .Take(10)
            .ProjectTo<ExportCarsWithDistanceDto>()
            .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportCarsWithDistanceDto[])
                , new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(outPutStringBuilder))
            {
                xmlSerializer.Serialize(writer, cars, namespaces);
            }

            return outPutStringBuilder.ToString().TrimEnd();
        }

        //15
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            StringBuilder outPutStringBuilder = new StringBuilder();

            var cars = context.Cars
            .Where(c => c.Make == "BMW")
            .OrderBy(c => c.Model)
            .ThenByDescending(c => c.TravelledDistance)
            .ProjectTo<ExportCarsWithMakerBMWDto>()
            .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportCarsWithMakerBMWDto[])
                , new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(outPutStringBuilder))
            {
                xmlSerializer.Serialize(writer, cars, namespaces);
            }

            return outPutStringBuilder.ToString().TrimEnd();
        }


        //16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            StringBuilder outPutStringBuilder = new StringBuilder();

            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .ProjectTo<ExportLocalSupplierDto>()
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportLocalSupplierDto[])
                , new XmlRootAttribute("suppliers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(outPutStringBuilder))
            {
                xmlSerializer.Serialize(writer, suppliers, namespaces);
            }

            return outPutStringBuilder.ToString().TrimEnd();
        }

        //17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            StringBuilder outPutStringBuilder = new StringBuilder();

            var cars = context.Cars
                .OrderByDescending(c=>c.TravelledDistance)
                .ThenBy(c=>c.Model)
                .Take(5)
                .ProjectTo<ExportCarWithPartsDto>()
                .ToArray();

            //SortingParts
         //  foreach (var car in cars)
         //  {
         //      car.Parts= car.Parts
         //          .OrderByDescending(x => x.Price)
         //          .ToArray();
         //  }



            var xmlSerializer = new XmlSerializer(typeof(ExportCarWithPartsDto[])
                , new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(outPutStringBuilder))
            {
                xmlSerializer.Serialize(writer, cars, namespaces);
            }

            return outPutStringBuilder.ToString().TrimEnd();
        }

        //18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            StringBuilder outPutStringBuilder = new StringBuilder();

            var customers = context.Customers
                .Include(c => c.Sales)
                .ThenInclude(s => s.Car)
                .ThenInclude(c => c.PartCars)
                .ThenInclude(pc => pc.Part)
                .Where(c => c.Sales.Count > 0)
                .Select(c => Mapper.Map<ExportTotalSalecByCustomerDto>(c))
                .ToArray()
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportTotalSalecByCustomerDto[])
                , new XmlRootAttribute("customers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(outPutStringBuilder))
            {
                xmlSerializer.Serialize(writer, customers, namespaces);
            }

            return outPutStringBuilder.ToString().TrimEnd();
        }

        //19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            StringBuilder outPutStringBuilder = new StringBuilder();

            var sales = context.Sales
                .ProjectTo<ExportSalesWithAppliedDto>()
                .ToArray();
                

            var xmlSerializer = new XmlSerializer(typeof(ExportSalesWithAppliedDto[])
                            , new XmlRootAttribute("sales"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(outPutStringBuilder))
            {
                xmlSerializer.Serialize(writer, sales, namespaces);
            }

            return outPutStringBuilder.ToString().TrimEnd();
        }
    }
}