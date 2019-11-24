using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Dtos.Export;
using ProductShop.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using AutoMapper.QueryableExtensions;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg =>
                             cfg.AddProfile<ProductShopProfile>());

            using (var db = new ProductShopContext())
            {

                //00 CreateDB
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();
                //db.SaveChanges();

                // Query 1.Import Users
                //string usersXml = File.ReadAllText("./../../../Datasets/users.xml");
                //string imputUsersXmlResult = ImportUsers(db, usersXml);
                //Console.WriteLine(imputUsersXmlResult);

                //Query 2. Import Products
                //string productsXml = File.ReadAllText("./../../../Datasets/products.xml");
                //string imputProductsXmlResult = ImportProducts(db, productsXml);
                //Console.WriteLine(imputProductsXmlResult);

                //Query 3. Import Categories
                //string categoriesXml = File.ReadAllText("./../../../Datasets/categories.xml");
                //string imputCategoriesXmlResult = ImportCategories(db, categoriesXml);
                //Console.WriteLine(imputCategoriesXmlResult);

                //Query 4. Import Categories and Products
                //string mappingTableCategoriesProductXml = File.ReadAllText("./../../../Datasets/categories-products.xml");
                //string imputMappingTableCatProdXmlResult = ImportCategoryProducts(db, mappingTableCategoriesProductXml);
                //Console.WriteLine(imputMappingTableCatProdXmlResult);


                //Query 5. Products In Range
                // string exportProductInRange = GetProductsInRange(db);
                // Console.WriteLine(exportProductInRange);

                //Query 6. Sold Products
                //string exportSoldProducts = GetSoldProducts(db);
                //Console.WriteLine(exportSoldProducts);

                //Query 7. Categories By Products Count
                // string exportCategoriesByProduct = GetCategoriesByProductsCount(db);
                // Console.WriteLine(exportCategoriesByProduct);

                //Query 8. Users and Products
                string exportUsersAndProducts = GetUsersWithProducts(db);
                Console.WriteLine(exportUsersAndProducts);


            }
        }
        //01
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportUsersDto[])
                                                        , new XmlRootAttribute("Users"));

            ImportUsersDto[] usersDtos;

            using (var reader = new StringReader(inputXml))
            {
                usersDtos = (ImportUsersDto[])xmlSerializer.Deserialize(reader);
            }

            var users = Mapper.Map<User[]>(usersDtos);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        //02
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportProductsDto[])
                                                       , new XmlRootAttribute("Products"));

            ImportProductsDto[] productssDtos;

            using (var reader = new StringReader(inputXml))
            {
                productssDtos = (ImportProductsDto[])xmlSerializer.Deserialize(reader);
            }

            var product = Mapper.Map<Product[]>(productssDtos);

            context.Products.AddRange(product);
            context.SaveChanges();

            return $"Successfully imported {product.Length}";
        }

        //03
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportCategoriesDto[])
                                                     , new XmlRootAttribute("Categories"));

            ImportCategoriesDto[] categoriesDtos;

            using (var reader = new StringReader(inputXml))
            {
                categoriesDtos = ((ImportCategoriesDto[])xmlSerializer.Deserialize(reader))
                    .Where(c => c.Name != null)
                    .ToArray();
            }

            var categeries = Mapper.Map<Category[]>(categoriesDtos);

            context.Categories.AddRange(categeries);
            context.SaveChanges();

            return $"Successfully imported {categeries.Length}";
        }

        //04
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportMappingCategoriesProduct[])
                                                     , new XmlRootAttribute("CategoryProducts"));

            ImportMappingCategoriesProduct[] mappingCattegoriesProductDtos;

            using (var reader = new StringReader(inputXml))
            {
                mappingCattegoriesProductDtos = ((ImportMappingCategoriesProduct[])xmlSerializer.Deserialize(reader))
                    .Where(mp => (context.Categories.Any(s => s.Id == mp.CategoryId) && context.Products.Any(s => s.Id == mp.ProductId)))
                    .ToArray();
            }

            var categeriesAndProducts = Mapper.Map<CategoryProduct[]>(mappingCattegoriesProductDtos);

            context.CategoryProducts.AddRange(categeriesAndProducts);
            context.SaveChanges();

            return $"Successfully imported {categeriesAndProducts.Length}";
        }

        //05
        public static string GetProductsInRange(ProductShopContext context)
        {
            StringBuilder outPutStringBuilder = new StringBuilder();

            var products = context.Products
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .Take(10)
            .ProjectTo<ExportProductInRangeDto>()
            .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportProductInRangeDto[])
                , new XmlRootAttribute("Products"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(outPutStringBuilder))
            {
                xmlSerializer.Serialize(writer, products, namespaces);
            }

            return outPutStringBuilder.ToString().TrimEnd();
        }

        //06
        public static string GetSoldProducts(ProductShopContext context)
        {
            StringBuilder outPutStringBuilder = new StringBuilder();

            var products = context.Users
                .Where(u => u.ProductsSold.Count >= 1)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
            .Take(5)
            .ProjectTo<ExportSoldProductDto>()
            .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportSoldProductDto[])
                , new XmlRootAttribute("Users"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(outPutStringBuilder))
            {
                xmlSerializer.Serialize(writer, products, namespaces);
            }

            return outPutStringBuilder.ToString().TrimEnd();
        }

        //07
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            StringBuilder outPutStringBuilder = new StringBuilder();

            var categories = context.Categories
                .Include(c => c.CategoryProducts)
                .ThenInclude(cp => cp.Product)
                .Select(c => new ExportCategoriesByProductDto()
                {
                    Name = c.Name,
                    CountProduct = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Select(cp => cp.Product.Price).DefaultIfEmpty(0).Average(),
                    TotalRevenue = c.CategoryProducts.Select(cp => cp.Product.Price).DefaultIfEmpty(0).Sum()
                })
                .ToArray()
                .OrderByDescending(cp => cp.CountProduct)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportCategoriesByProductDto[])
                , new XmlRootAttribute("Categories"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(outPutStringBuilder))
            {
                xmlSerializer.Serialize(writer, categories, namespaces);
            }

            return outPutStringBuilder.ToString().TrimEnd();

        }

        //08
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            StringBuilder outPutStringBuilder = new StringBuilder();

            var usersWithSoldProducts = context.Users
                .Include(u => u.ProductsSold)
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .ToArray();

            var users = new ExportUsersWithProductsDto()
            {
                Count = usersWithSoldProducts.Length,
                Users = usersWithSoldProducts
                    .Select(u => new ExportUserDto()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        SoldProducts = new ExportSoldProductSecondDto()
                        {
                            Count = u.ProductsSold.Count,
                            Products = u.ProductsSold.Select(ps => new ExportProductsDto()
                            {
                                Name = ps.Name,
                                Price = ps.Price
                            })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                        }
                    })
                    .OrderByDescending(u => u.SoldProducts.Count)
                    .Take(10)
                    .ToArray()
            };

            var xmlSerializer = new XmlSerializer(typeof(ExportUsersWithProductsDto[])
            , new XmlRootAttribute("Users"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(outPutStringBuilder))
            {
                xmlSerializer.Serialize(writer, users, namespaces);
            }

            return outPutStringBuilder.ToString().TrimEnd();
        }
    }
}
