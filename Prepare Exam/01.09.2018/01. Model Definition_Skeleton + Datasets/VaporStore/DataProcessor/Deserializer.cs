namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.ImportDto;

    public static class Deserializer
    {

        //Mapping table json
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesDto = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);

            StringBuilder stBuilder = new StringBuilder();
            List<Game> gameList = new List<Game>();
            foreach (var gameDto in gamesDto)
            {
                if (!IsValid(gameDto) ||
                    gameDto.Tags.Count == 0)
                {
                    stBuilder.AppendLine("Invalid Data");
                    continue;
                }

                var developer = GetDeveloper(context, gameDto.Developer);
                var genre = GetGenre(context, gameDto.Genre);

                var game = new Game
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Developer = developer,
                    Genre = genre
                };

                foreach (var currentTag in gameDto.Tags)
                {
                    var tag = GetTag(context, currentTag);
                    game.GameTags.Add(new GameTag { Tag = tag });
                }

                gameList.Add(game);
                stBuilder.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }

            context.Games.AddRange(gameList);
            context.SaveChanges();
            string result = stBuilder.ToString().TrimEnd();

            return result;
        }


        //Nested class json
        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var usersDto = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString);

            StringBuilder stBuilder = new StringBuilder();
            List<User> usersList = new List<User>();

            foreach (var userDto in usersDto)
            {
                if (!IsValid(userDto)
                    
                    || !userDto.Cards.All(IsValid))
                {
                    stBuilder.AppendLine("Invalid Data");
                    continue;
                }

                bool isValidEnum = true;
                foreach (var card in userDto.Cards)
                {

                    var cardType = Enum.TryParse<CardType>(card.Type, out CardType resultCardType);

                    if (cardType == false)
                    {
                        stBuilder.AppendLine("Invalid Data");
                        isValidEnum = false;
                        break;
                    }
                }
                if (isValidEnum==true)
                {

                    var user = new User
                    {
                        Username = userDto.Username,
                        FullName = userDto.FullName,
                        Email = userDto.Email,
                        Age = userDto.Age,
                    };

                    foreach (var card in userDto.Cards)
                    {

                        user.Cards.Add(new Card
                        {
                            Number = card.Number,
                            Cvc = card.CVC,
                            Type = Enum.Parse<CardType>(card.Type)
                        });
                    }

                  usersList.Add(user);
                    stBuilder.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
                }


            }


            context.Users.AddRange(usersList);
            context.SaveChanges();
            string result = stBuilder.ToString().TrimEnd();
            return result;
        }


        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
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
        private static Developer GetDeveloper(VaporStoreDbContext context, string gameDtoDeveloper)
        {
            var developer = context.Developers.FirstOrDefault(d => d.Name == gameDtoDeveloper);
            if (developer == null)
            {
                developer = new Developer
                {
                    Name = gameDtoDeveloper
                };
                context.Developers.Add(developer);
                context.SaveChanges();
            }
            return developer;
        }
        private static Genre GetGenre(VaporStoreDbContext context, string genreDto)
        {
            var genre = context.Genres.FirstOrDefault(g => g.Name == genreDto);
            if (genre == null)
            {
                genre = new Genre
                {
                    Name = genreDto
                };
                context.Genres.Add(genre);
                context.SaveChanges();
            }
            return genre;
        }
        private static Tag GetTag(VaporStoreDbContext context, string currentTag)
        {
            var tag = context.Tags.FirstOrDefault(t => t.Name == currentTag);
            if (tag == null)
            {
                tag = new Tag
                {
                    Name = currentTag
                };
                context.Tags.Add(tag);
                context.SaveChanges();
            }
            return tag;
        }



    }
}