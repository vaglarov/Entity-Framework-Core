using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
   //  <User>
   //<v>Almire</firstName>
   //<lastName>Ainslee</lastName>
   //<soldProducts>
   //  <Product>
   //    <name>olio activ mouthwash</name>
   //    <price>206.06</price>

        [XmlType("User")]
    public class ExportSoldProductDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlArray("soldProducts")]
        public ExportProductsNameAndPriceDto[] ProductsSold { get; set; }

    }
}
