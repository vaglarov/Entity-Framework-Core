using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    //<Product>
    //    <name>olio activ mouthwash</name>
    //    <price>206.06</price>

        [XmlType("Product")]
    public class ExportProductsNameAndPriceDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
