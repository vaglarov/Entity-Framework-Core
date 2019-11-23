using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public const string successMessage = "Successfully imported {0}.";
        public static void Main(string[] args)
        {


            using (var db = new CarDealerContext())
            {
                // 00 Create DB
                // db.Database.EnsureDeleted();
                //   db.Database.EnsureCreated();


                //Query 9. Import Suppliers
                //string suppliersJson = File.ReadAllText("./../../../Datasets/suppliers.json");
                //var resulSsuppliersJson = ImportSuppliers(db, suppliersJson);
                //Console.WriteLine(resulSsuppliersJson);

                //Query 10. Import Parts
                //string partsJson = File.ReadAllText("./../../../Datasets/parts.json");
                //var resulPartsJson = ImportParts(db, partsJson);
                //Console.WriteLine(resulPartsJson);

                //Query 11. Import Cars
                string carsJson = File.ReadAllText("./../../../Datasets/cars.json");
                var resultCarsJson = ImportParts(db, carsJson);
                Console.WriteLine(resultCarsJson);


            }

        }

        //09
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return string.Format(successMessage, suppliers.Count());
        }

        //10
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<Part[]>(inputJson)
                .Where(p=>context.Suppliers.Any(s=>s.Id==p.SupplierId))
                .ToList(); 

            context.AttachRange(parts);
            context.SaveChanges();

            // Stupped Idea
            // var parts = JsonConvert.DeserializeObject<Part[]>(inputJson);
            //  var suppliers = context
            //      .Suppliers
            //      .Select(s => new { s.Id })
            //      .ToList();
            //
            //  var supID = new List<int>();
            //  foreach (var item in suppliers)
            //  {
            //      int idS = item.Id;
            //      supID.Add(idS);
            //  }
            //
            //  int unmached = 0;
            //  foreach (var part in parts)
            //  {
            //      if (supID.Contains(part.SupplierId))
            //      {
            //          context.Parts.Add(part);
            //          context.SaveChanges();
            //      }
            //      else
            //      {
            //          unmached++;
            //      }
            //
            //  }


            return string.Format(successMessage, parts.Count());
        }

        //11
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var cars = JsonConvert.DeserializeObject<Car[]>(inputJson);


            return string.Format(successMessage, cars.Count() );
        }
    }
}