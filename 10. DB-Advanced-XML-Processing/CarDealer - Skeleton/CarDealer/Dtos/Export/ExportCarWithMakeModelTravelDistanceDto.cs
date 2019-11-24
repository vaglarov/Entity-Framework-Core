using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    //<car make="BMW" model="M5 F10" travelled-distance="435603343" />
    [XmlType("car")]
    public class ExportCarWithMakeModelTravelDistanceDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public string TravelledDistance { get; set; }
    }
}
