namespace LabCarsData
{
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using var db = new CarDbContext();
            db.Database.Migrate();

            //db.Makes.Add(new Make
            //{
            //    Name = "Audi"
            //});

            //db.Makes.Add(new Make
            //{
            //    Name = "Opel"
            //});

            //db.Makes.Add(new Make
            //{
            //    Name = "Mazda"
            //});

            //db.Makes.Add(new Make
            //{
            //    Name = "Mercedes"
            //});

            //db.Makes.Add(new Make
            //{
            //    Name = "BMW"
            //});

            //db.Makes.Add(new Make
            //{
            //    Name = "Renaugh"
            //});

            //var opelMake = db.Makes.FirstOrDefault(m => m.Name== "Opel");

            //opelMake.Models.Add(new Model
            //{
            //    Name = "Astra",
            //    Year=2017,
            //    Modification="2.2 TDI"
            //});

            //opelMake.Models.Add(new Model
            //{
            //    Name = "Kadet",
            //    Year = 1986,
            //    Modification = "1.3i"
            //});

            //opelMake.Models.Add(new Model
            //{
            //    Name = "Insingnia",
            //    Year = 2015,
            //    Modification = "OPC"
            //});

            //var kadetModels = db.Models.FirstOrDefault(m => m.Name =="Kadet");


            //kadetModels.Cars.Add(new Car {
            //Color="Pink",
            //Price=1500,
            //ProductionDate=DateTime.Now.AddYears( 1986),
            //VIN="12335496"
            //});

            //kadetModels.Cars.Add(new Car
            //{
            //    Color = "Red",
            //    Price = 100,
            //    ProductionDate = DateTime.Now.AddYears(1980),
            //    VIN = "1233ss5496"
            //});
            //kadetModels.Cars.Add(new Car
            //{
            //    Color = "Black",
            //    Price = 2500,
            //    ProductionDate = DateTime.Now.AddYears(1991),
            //    VIN = "12335496"
            //});



            //var carId = db.Cars.FirstOrDefault(c => c.CarId == 1);

            //var newCustomer = new Customer
            //{
            //    FirstName = "Ivan",
            //    LastName = "Ivanov",
            //    Age = 21

            //};

            //newCustomer.Purcheses.Add(new CarPurchase
            //{
            //    Car = carId,
            //    PurchaseDate = DateTime.Now.AddDays(-10),
            //    Price = carId.Price * (decimal)0.8
            //});

            //db.Customers.Add(newCustomer);

            var customer = db.Customers.FirstOrDefault(c => c.Id == 1);

            customer.Address = new Address
            {
                Text = "3-A-7",
                Town = "Sofia"
            }; 
          
            db.SaveChanges();
        }
    }
}
