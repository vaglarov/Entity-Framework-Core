using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    //<parts>
    //  <part name = "Master cylinder" price="130.99" />
    //  <part name = "Water tank" price="100.99" />
    //  <part name = "Front Right Side Inner door handle" price="100.99" />

    [XmlType("part")]
    public class ExportPartNameAndPriceDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}
