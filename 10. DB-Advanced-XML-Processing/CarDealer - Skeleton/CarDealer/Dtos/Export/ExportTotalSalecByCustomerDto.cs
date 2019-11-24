using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    // <customers>
    // <customer full-name="Taina Achenbach" bought-cars="1" spent-money="5588.17" />

    [XmlType("customer")]
    public class ExportTotalSalecByCustomerDto
    {
        [XmlAttribute("full-name")]
        public string Name { get; set; }

        [XmlAttribute("bought-cars")]
        public int BoughtCars { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpentMoney { get; set; }

    }
}
