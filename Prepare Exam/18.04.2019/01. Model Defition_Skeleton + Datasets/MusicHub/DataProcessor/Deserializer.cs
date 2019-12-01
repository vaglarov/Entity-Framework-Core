namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using MusicHub.Data.Models;
    using MusicHub.Data.Models.Enums;
    using MusicHub.DataProcessor.ImportDtos;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            StringBuilder stBuilder = new StringBuilder();

            var wtritersDto = JsonConvert.DeserializeObject<ImportWritersDto[]>(jsonString);

            List<Writer> witersList = new List<Writer>();
            foreach (var writerDto in wtritersDto)
            {
                if (!IsValid(writerDto))
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }
                var writer = new Writer
                {
                    Name = writerDto.Name,
                    Pseudonym = writerDto.Pseudonym
                };
                witersList.Add(writer);
                string appendWrites = string.Format(SuccessfullyImportedWriter, writerDto.Name);
                stBuilder.AppendLine(appendWrites);
            }

            string result = stBuilder.ToString().TrimEnd();
            context.Writers.AddRange(witersList);
            context.SaveChanges();

            return result;
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            StringBuilder stBuilder = new StringBuilder();

            var ImportProducersAlbumsDto = JsonConvert.DeserializeObject<ImportProducers[]>(jsonString);

            var producerList = new List<Producer>();
            foreach (var importProsucerAlbumDto in ImportProducersAlbumsDto)
            {
                if (!IsValid(importProsucerAlbumDto))
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var alnumList = new List<Album>();
                bool albumChechParser = true;
                foreach (var albumDto in importProsucerAlbumDto.Albums)
                {
                    if (!IsValid(albumDto))
                    {
                        stBuilder.AppendLine(ErrorMessage);
                        albumChechParser = false;
                        break;
                    }

                    var dateResult = new DateTime();

                    var datechecker = DateTime.TryParseExact(albumDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateResult);
                    if (!datechecker)
                    {
                        stBuilder.AppendLine(ErrorMessage);
                        albumChechParser = false;
                        break;
                    }

                    var album = new Album
                    {
                        Name = albumDto.Name,
                        ReleaseDate = dateResult
                    };
                    alnumList.Add(album);
                }

                if (!albumChechParser)
                {
                    continue;
                }
                context.Albums.AddRange(alnumList);

                var producer = new Producer
                {
                    Name = importProsucerAlbumDto.Name,
                    Pseudonym = importProsucerAlbumDto.Pseudonym,
                    PhoneNumber = importProsucerAlbumDto.PhoneNumber,
                    Albums = alnumList.ToArray()
                };
                producerList.Add(producer);
                var addPruducer = string.Empty;
                if (producer.PhoneNumber == null)
                {
                    addPruducer = string.Format(SuccessfullyImportedProducerWithNoPhone, producer.Name, producer.Albums.Count);
                }
                else
                {
                    addPruducer = string.Format(SuccessfullyImportedProducerWithPhone, producer.Name, producer.PhoneNumber, producer.Albums.Count);
                }
                stBuilder.AppendLine(addPruducer);
            }


            string result = stBuilder.ToString().TrimEnd();
            context.Producers.AddRange(producerList);
            context.SaveChanges();

            return result;
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            StringBuilder stBuilder = new StringBuilder();

            var xmlSeserializer = new XmlSerializer(typeof(ImportSongDto[]),
                                                          new XmlRootAttribute("Songs"));

            var songsDto = (ImportSongDto[])xmlSeserializer.Deserialize(new StringReader(xmlString));

            List<Song> songsList = new List<Song>();

            foreach (var songDto in songsDto)
            {
                if (!IsValid(songDto))
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var writer = context.Writers.Where(wr => wr.Id == songDto.WriterId).FirstOrDefault();
                if (writer == null)
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var album = context.Albums.Where(a => a.Id == songDto.AlbumId).FirstOrDefault();
                if (album == null)
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }


                var timeDuration = new TimeSpan();
                var timechecker = TimeSpan.TryParseExact(songDto.Duration, "c", CultureInfo.InvariantCulture, out timeDuration);

                if (!timechecker)
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var dateResult = new DateTime();

                var datechecker = DateTime.TryParseExact(songDto.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateResult);
                if (!datechecker)
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }


                var genre = Enum.TryParse<Genre>(songDto.Genre, out Genre genreType);

                if (!genre)
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var uniqueSong = songsList.Where(s => s.Name == songDto.Name).FirstOrDefault();

                if (uniqueSong!=null)
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var song = new Song
                {
                    Name = songDto.Name,
                    Duration = timeDuration,
                    CreatedOn = dateResult,
                    Genre = genreType,
                    AlbumId = songDto.AlbumId,
                    WriterId = songDto.WriterId,
                    Price = songDto.Price
                };
                songsList.Add(song);

                //Imported { song name} ({ song genre}
                //genre) with duration { song duration}
                string correctLine = string.Format(SuccessfullyImportedSong, song.Name, song.Genre, song.Duration);
                stBuilder.AppendLine(correctLine);
            }

            string result = stBuilder.ToString().TrimEnd();
            context.Songs.AddRange(songsList);
            context.SaveChanges();

            return result;
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validatinResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validatinResult, true);

            return isValid;
        }
    }
}