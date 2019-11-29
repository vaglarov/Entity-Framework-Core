namespace VaporStore.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.ExpotDto;

    public static class Serializer
    {
        //       "Id": 4,
        //"Genre": "Violent",
        //"Games": [
        //  {
        //    "Id": 49,
        //    "Title": "Warframe",
        //    "Developer": "Digital Extremes",
        //    "Tags": "Single-player, In-App Purchases, Steam Trading Cards, Co-op, Multi-player, Partial Controller Support",
        //    "Players": 6


        //  },
        //"TotalPlayers": 10

        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genres = context
                .Genres
                .Where(g => genreNames.Contains(g.Name))
                .Select(x => new
                {
                    Id = x.Id,
                    Genre = x.Name,
                    Games = x.Games
                    .Where(p => p.Purchases.Any())
                    .Select(y => new
                    {
                        Id = y.Id,
                        Title = y.Name,
                        Deleloper = y.Developer.Name,
                        Tags = string.Join(", ", y.GameTags.Select(t => new { t.Tag.Name })),
                        Players = y.Purchases.Count()
                    })
                    .OrderByDescending(z => z.Players)
                    .ThenBy(s => s.Id)
                    .ToArray(),
                    TotalPlayers = x.Games.Sum(p => p.Purchases.Count)
                })
                .OrderByDescending(p => p.TotalPlayers)
                .ThenBy(g => g.Id)
                .ToArray();

            var jsonResult = JsonConvert.SerializeObject(genres, Newtonsoft.Json.Formatting.Indented);

            return jsonResult;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            var enumType = Enum.Parse<PurchaseType>(storeType);

            var users = context.Users
                .Select(x => new ExportUserDto
                {
                    Username = x.Username,
                    Purchases = x.Cards.SelectMany(p => p.Purchases).Where(p=>p.Type== enumType).Select(c => new ExportPurchaseDto
                    {

                        Card = c.Card.Number,
                        Cvc = c.Card.Cvc,
                        Date = c.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Game = new ExportGameDto
                        {
                            Genre = c.Game.Genre.Name,
                            Title = c.Game.Name,
                            Price = c.Game.Price
                        }
                    })
                        .OrderBy(c => c.Date)
                        .ToArray(),
                    TotalSpet = x.Cards.SelectMany(p => p.Purchases).Sum(p => p.Game.Price)
                })
                .OrderByDescending(t => t.TotalSpet)
                .ThenBy(u => u.Username)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportUserDto[]), new XmlRootAttribute("Users"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty});

            xmlSerializer.Serialize(new StringWriter(sb), users, namespaces);

            var result = sb.ToString().TrimEnd();

            return result;
        }
    }
}