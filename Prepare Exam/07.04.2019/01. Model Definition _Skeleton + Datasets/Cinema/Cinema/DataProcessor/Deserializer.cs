namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Cinema.Data.Models;
    using Cinema.Data.Models.Enums;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var moviesDto = JsonConvert.DeserializeObject<ImportMoviesDto[]>(jsonString);

            StringBuilder stBuilder = new StringBuilder();
            var movies = new List<Movie>();
            foreach (var movieDto in moviesDto)
            {
                if (!IsValid(movieDto) || movies.Any(x => x.Title == movieDto.Title))
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var genreType = Enum.TryParse<Genre>(movieDto.Genre, out Genre resultGenreType);

                var timeChecker = TimeSpan.TryParseExact(movieDto.Duration, "c", CultureInfo.InvariantCulture, out TimeSpan timeResult);
               
                if (!timeChecker|| !genreType)
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = new Movie
                {
                    Title = movieDto.Title,
                    Genre = resultGenreType,
                    Duration = timeResult,
                    Rating = movieDto.Rating,
                    Director = movieDto.Director
                };
                movies.Add(movie);
                // "Successfully imported {0} with genre {1} and rating {2}!";
                stBuilder.AppendLine(string.Format(SuccessfulImportMovie, movieDto.Title, movieDto.Genre, $"{movie.Rating:f2}"));

            }



            context.Movies.AddRange(movies);
            context.SaveChanges();
            string result = stBuilder.ToString().TrimEnd();

            return result;
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
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