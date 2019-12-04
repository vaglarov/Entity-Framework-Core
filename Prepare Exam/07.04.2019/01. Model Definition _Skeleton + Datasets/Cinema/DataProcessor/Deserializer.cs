namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
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

                if (!timeChecker || !genreType)
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
            var hallsDto = JsonConvert.DeserializeObject<ImportHallsSeatDto[]>(jsonString);

            StringBuilder stBuilder = new StringBuilder();
            var halls = new List<Hall>();
            foreach (var hallDto in hallsDto)
            {


                var numberSeats = int.Parse(hallDto.Seats);
                if (!IsValid(hallDto) || numberSeats <= 0)
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                bool is3d = bool.Parse(hallDto.Is3D);
                bool is4d = bool.Parse(hallDto.Is4Dx);
                var hall = new Hall
                {
                    Name = hallDto.Name,
                    Is3D = is3d,
                    Is4Dx = is4d
                };

                var seats = new List<Seat>();

                for (int i = 1; i <= numberSeats; i++)
                {
                    var seat = new Seat
                    {
                        Hall = hall
                    };
                    seats.Add(seat);
                }
                context.Seats.AddRange(seats);

                halls.Add(hall);
                // "Successfully imported {0}({1}) with {2} seats!";

                if (hall.Is3D && hall.Is4Dx)
                {
                    stBuilder.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, "4Dx/3D", seats.Count));
                }
                else if (hall.Is3D && !hall.Is4Dx)
                {
                    stBuilder.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, "3D", seats.Count));
                }
                else if (!hall.Is3D && hall.Is4Dx)
                {
                    stBuilder.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, "4Dx", seats.Count));
                }
                else
                {
                    stBuilder.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, "Normal", seats.Count));
                }
            }

            context.Halls.AddRange(halls);
            context.SaveChanges();
            string result = stBuilder.ToString().TrimEnd();

            return result;
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            StringBuilder stBuilder = new StringBuilder();

            var xmlSeserializer = new XmlSerializer(typeof(ImportProjectionDto[]),
                                                          new XmlRootAttribute("Projections"));

            var projectionsDto = (ImportProjectionDto[])xmlSeserializer.Deserialize(new StringReader(xmlString));

            List<Projection> projectionsList = new List<Projection>();

            foreach (var projectionDto in projectionsDto)
            {
                if (!IsValid(projectionDto))
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = context.Movies.Where(x => x.Id == projectionDto.MovieId).FirstOrDefault();
                var hall = context.Halls.Where(x => x.Id == projectionDto.HallId).FirstOrDefault();
                var dateResult = new DateTime();  //2019-04-27 13:33:20 format

                var datechecker = DateTime.TryParseExact(projectionDto.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateResult);
                if (!datechecker || movie==null || hall==null)
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var projection = new Projection
                {
                    MovieId = projectionDto.MovieId,
                    Movie= movie,
                    HallId = projectionDto.HallId,
                    Hall=hall,
                    DateTime = dateResult
                };

                var stringDate = dateResult.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                projectionsList.Add(projection);
                //"Successfully imported projection {0} on {1}!";
                stBuilder.AppendLine(string.Format(SuccessfulImportProjection, movie.Title, stringDate));
            }

            context.Projections.AddRange(projectionsList);
            context.SaveChanges();

            string resultBuilder = stBuilder.ToString().TrimEnd();

            return resultBuilder;
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            StringBuilder stBuilder = new StringBuilder();

            var xmlSeserializer = new XmlSerializer(typeof(ImportCustomerDto[]),
                                                          new XmlRootAttribute("Customers"));

            var customersDto = (ImportCustomerDto[])xmlSeserializer.Deserialize(new StringReader(xmlString));

            List<Customer> customerList = new List<Customer>();

            foreach (var customerDto in customersDto)
            {
                if (!IsValid(customerDto))
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var customer = new Customer
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Age = customerDto.Age,
                    Balance=customerDto.Balance
                };

                bool checkTickets = true;

                var tikets = new List<Ticket>();
                foreach (var ticketDto in customerDto.Tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        checkTickets = false;
                        break;
                    }

                    var projection = context.Projections.Where(x => x.Id == ticketDto.Id).FirstOrDefault();

                    if (projection==null)
                    {
                        checkTickets = false;
                        break;
                    }

                    var ticket = new Ticket
                    {
                        Customer = customer,
                        Projection = projection,
                        Price = ticketDto.Price
                    };
                    tikets.Add(ticket);
                }

                if (!checkTickets)
                {
                    stBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                context.Tickets.AddRange(tikets);

                customerList.Add(customer);
                //= "Successfully imported customer {0} {1} with bought tickets: {2}!";
                stBuilder.AppendLine(string.Format(SuccessfulImportCustomerTicket, customer.FirstName,customer.LastName,tikets.Count()));
            }

            context.Customers.AddRange(customerList);
            context.SaveChanges();

            string resultBuilder = stBuilder.ToString().TrimEnd();

            return resultBuilder;
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