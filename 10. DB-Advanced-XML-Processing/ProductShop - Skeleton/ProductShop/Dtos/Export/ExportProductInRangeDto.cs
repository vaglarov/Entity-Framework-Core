using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    //  <Products>
    //<Product>
    //  <name>TRAMADOL HYDROCHLORIDE</name>
    //  <price>516.48</price>
    //   <buyer>Brendin Predohl</buyer>

    [XmlType("Product")]
    public class ExportProductInRangeDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("buyer")]

        public string BuyerName { get; set; }

    }
}
