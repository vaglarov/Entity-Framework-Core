using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("Part")]
    public class ImportPartDto
    {
        //  <name>Unexposed bumper</name>
        // <price>1003.34</price>
        // <quantity>10</quantity>
        // <supplierId>12</supplierId>


        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]

        public decimal Price { get; set; }

        [XmlElement("quantity")]

        public int Quantity { get; set; }

        [XmlElement("supplierId")]

        public int SupplierId { get; set; }

    }
}
