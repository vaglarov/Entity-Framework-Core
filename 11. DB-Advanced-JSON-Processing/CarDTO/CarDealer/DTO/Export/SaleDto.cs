using CarDealer.DTO.Import;
using Newtonsoft.Json;

namespace CarDealer.DTO.Export
{
    public class SaleDto
    {
        [JsonProperty(PropertyName = "car")]
        public CarDto Car { get; set; }

        [JsonProperty(PropertyName = "customerName")]
        public string CustomerName { get; set; }

        public string Discount { get; set; }

        [JsonProperty(PropertyName = "price")]
        public string Price { get; set; }

        [JsonProperty(PropertyName = "priceWithDiscount")]
        public string PriceWithDiscount { get; set; }
    }
}