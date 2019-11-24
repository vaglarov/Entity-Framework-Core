using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    //  <cars>
    //<car make = "Opel" model="Astra" travelled-distance="516628215">
    //  <parts>

    [XmlType("car")]
    public class ExportCarWithPartsDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }

        [XmlArray("parts")]
        public ExportPartNameAndPriceDto[] Parts { get; set; }

    }
}
