namespace MusicHub.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using MusicHub.DataProcessor.ExportDtos;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            //        AlbumName": "Devil's advocate",
            //"ReleaseDate": "07/21/2018",
            //"ProducerName": "Evgeni Dimitrov",
            //"Songs": [
            //  {
            //    "SongName": "Numb",
            //    "Price": "13.99",
            //    "Writer": "Kara-lynn Sharpous"
           // vD"mm/DD/yyyy",D"mm/DD/yyyy",
    //  },


             var albums = context
                .Albums
                .Where(p => p.ProducerId == producerId)
                .Select(x => new
                {
                    AlbumName = x.Name,
                    ReleaseDate = x.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = x.Producer.Name,
                    Songs = x.Songs.Select(s => new
                        {
                            SongName = s.Name,
                            Price = s.Price.ToString("F2"),
                            Writer = s.Writer.Name
                        })
                        .OrderByDescending(n => n.SongName)
                        .ThenBy(w => w.Writer)
                        .ToArray(),
                    AlbumPrice = x.Price.ToString("F2")
                })
                .OrderByDescending(p => decimal.Parse(p.AlbumPrice))
                .ToArray();

            var jsonResult = JsonConvert.SerializeObject(albums, Newtonsoft.Json.Formatting.Indented);

            return jsonResult;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {

            //          < Song >
            //< SongName > Away </ SongName >
            //< Writer > Norina Renihan </ Writer >

            //   < Performer > Lula Zuan </ Performer >

            //      < AlbumProducer > Georgi Milkov </ AlbumProducer >

            //         < Duration > 00:05:35 </ Duration >


            var songs = context.Songs
            .Where(d => d.Duration.TotalSeconds > duration)
            .Select(s => new ExportSongDto
            {
                SongName = s.Name,
                Writer = s.Writer.Name,
                Performer = s.SongPerformers.Select(p => p.Performer.FirstName + " " + p.Performer.LastName).FirstOrDefault(),
                AlbumProducer = s.Album.Producer.Name,
                Duration = s.Duration.ToString("c")
            })
            .OrderBy(x => x.SongName)
            .ThenBy(w => w.Writer)
            .ToArray();


            var xmlSerializer = new XmlSerializer(typeof(ExportSongDto[]), new XmlRootAttribute("Songs"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            xmlSerializer.Serialize(new StringWriter(sb),songs, namespaces);

            var result = sb.ToString().TrimEnd();

            return result;
        }
    }
}