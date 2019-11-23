using Newtonsoft.Json;

namespace CarDealer.DTO.Export
{
    public class CustomerDto
    {
        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "boughtCars")]
        public int BoughtCars { get; set; }

        [JsonProperty(PropertyName = "spentMoney")]
        public decimal SpentMoney { get; set; }
    }
}