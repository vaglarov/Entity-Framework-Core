using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var db = new ProductShopContext())
            {
                // 00 Create DB
                //      db.Database.EnsureDeleted();
                //      db.Database.EnsureCreated();

                //01 import Users 
                //  var imputJson = File.ReadAllText("./../../../Datasets/users.json");
                // 
                //  var result = ImportUsers(db, imputJson);
                //  Console.WriteLine(result);

                //02 Import Products

                // var importProducts = File.ReadAllText("./../../../Datasets/products.json");
                // var resultProducts = ImportProducts(db, importProducts);
                // Console.WriteLine(resultProducts);

                //03 Import Categories

                //   var imputJsonCategories = File.ReadAllText("./../../../Datasets/categories.json");
                //   var resultCategories = ImportCategories(db, imputJsonCategories);
                //   Console.WriteLine(resultCategories);

                //04 Import CategoryProducts
                //var imputJsonCategoryProducts = File.ReadAllText("./../../../Datasets/categories-products.json");
                //var resultCategoryProducts = ImportCategoryProducts(db, imputJsonCategoryProducts);
                //Console.WriteLine(resultCategoryProducts);

                //05 Query 5. Export Products In Range
                string result = GetProductsInRange(db);
                Console.WriteLine(result);


                //06 Query 6. Export Successfully Sold Products
                // string result = GetSoldProducts(db);
                // Console.WriteLine(result);

                //07 Query 7. Export Categories By Products Count
                // string result = GetCategoriesByProductsCount(db);
                // Console.WriteLine(result);

                //08Query 8. Export Users and Products
               // string result = GetUsersWithProducts(db);
               // Console.WriteLine(result);

            }

        }

        //01
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);
            context.Users.AddRange(users);
            context.SaveChanges();

            string returtMessage = $"Successfully imported {users.Length}";
            return returtMessage;
        }
        //02
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);
            context.Products.AddRange(products);
            context.SaveChanges();

            string result = $"Successfully imported {products.Length}";
            return result;
        }

        //03
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {

            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson);
            int nullValue = 0;
            foreach (var category in categories)
            {
                if (category.Name != null)
                {
                    context.Categories.Add(category);
                }
                else
                {
                    nullValue += 1;
                }
            }
            context.SaveChanges();

            string message = $"Successfully imported {categories.Length - nullValue}";
            return message;
        }

        //04
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);
            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();
            string message = $"Successfully imported {categoryProducts.Count}";
            return message;
        }

        //05
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .ToList();

            var json = JsonConvert.SerializeObject(products, Formatting.Indented);
            return json;
        }

        //06
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context
                .Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                            buyerFirstName = p.Buyer.FirstName,
                            buyerLastName = p.Buyer.LastName
                        }).ToList()

                })
                .ToList();

            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            return json;
        }
        //07
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var category = context
                .Categories
                .OrderByDescending(c => c.CategoryProducts.Count)
                .Select(
                c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = $"{c.CategoryProducts.Average(cp => cp.Product.Price):F2}",
                    totalRevenue = $"{c.CategoryProducts.Sum(cp => cp.Product.Price):F2}"
                })
                .ToList();

            var json = JsonConvert.SerializeObject(category, Formatting.Indented);
            return json;
        }

        //08
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersWithProduct = context
                .Users
                 .Where(u => u.ProductsSold.Any(p=>p.Buyer!=null))
                 .Select(u => new
                 {
                     firstName = u.FirstName,
                     lastName = u.LastName,
                     age = u.Age,
                     soldProducts = new
                     {
                         count = u.ProductsSold
                         .Where(p=>p.Buyer!=null)
                         .Count(),
                         products = u.ProductsSold
                                    .Where(p => p.Buyer != null)
                                    .Select(ps => new
                                    {
                                        name = ps.Name,
                                        price = ps.Price
                                    })
                                    .ToList()
                     }

                 })
                 .OrderByDescending(u=>u.soldProducts.count)
                 .ToArray();

            var userOutPut = new
            {
                usersCount = usersWithProduct.Count(),
                users = usersWithProduct
            };

            var json = JsonConvert.SerializeObject(userOutPut, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });
            return json;
        }
    }
}