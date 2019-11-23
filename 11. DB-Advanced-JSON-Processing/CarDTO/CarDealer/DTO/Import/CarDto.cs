using System.Collections.Generic;
using Newtonsoft.Json;

namespace CarDealer.DTO.Import
{
    public class CarDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        [JsonIgnore]
        public ICollection<int> PartsId { get; set; } = new List<int>();
    }
}