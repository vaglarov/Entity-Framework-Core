using CarDealer.DTO.Import;
using Newtonsoft.Json;

namespace CarDealer.DTO.Export
{
    public class CarPartDto
    {
        [JsonProperty(PropertyName = "car")]
        public CarDto Car { get; set; }

        [JsonProperty(PropertyName = "parts")]
        public PartDto[] Parts { get; set; }
    }
}
