namespace Cinema.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {

            //        "MovieName": "SIS",
            //"Rating": "10.00",
            //"TotalIncomes": "184.04",
            //"Customers": [
            //  {
            //    "FirstName": "Davita",
            //    "LastName": "Lister",
            //    "Balance": "279.76"
            //  },
            var movies = context.Movies.Where(x => x.Rating >= rating && x.Projections.Any(p => p.Tickets.Count > 0))
                .OrderByDescending(x => x.Rating)
                .ThenByDescending(p => p.Projections.Sum(t => t.Tickets.Sum(pc => pc.Price)))//Take first 10 records and order the movies by rating (descending), then by total incomes (descending).
                .Select(x => new
                {
                    MovieName = x.Title,
                    Rating = x.Rating.ToString("f2"),
                    TotalIncomes = x.Projections.SelectMany(p => p.Tickets).Sum(p => p.Price).ToString("f2"),
                    Customers = x.Projections.SelectMany(p => p.Tickets).Select(c => c.Customer).Select(cus => new
                    {
                        FirstName = cus.FirstName,
                        LastName = cus.LastName,
                        Balance = cus.Balance.ToString("f2")
                    })
                    .OrderByDescending(c => c.Balance)
                    .ThenBy(c => c.FirstName)
                    .ThenBy(c => c.LastName)
                    .ToArray()    //Order the customers by balance (descending), then by first name (ascending) and last name (ascending

                })
                .Take(10)
                .ToArray();



            var jsonResult = JsonConvert.SerializeObject(movies, Newtonsoft.Json.Formatting.Indented);

            return jsonResult;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customers = context
               .Customers
               .Where(a => a.Age >= age)
               .OrderByDescending(x => x.Tickets.Sum(p => p.Price))
               .Take(10)
               .Select(x => new ExportCustomerDto
               {
                   FirstName = x.FirstName,
                   LastName = x.LastName,
                   SpentMoney = x.Tickets.Sum(p => p.Price).ToString("F2"),
                   SpentTime = TimeSpan.FromSeconds(
                           x.Tickets.Sum(s => s.Projection.Movie.Duration.TotalSeconds))
                       .ToString(@"hh\:mm\:ss")
               })
               .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportCustomerDto[]), new XmlRootAttribute("Customers"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            xmlSerializer.Serialize(new StringWriter(sb), customers, namespaces);

            var result = sb.ToString().TrimEnd();

            return result;
        }
    }
}