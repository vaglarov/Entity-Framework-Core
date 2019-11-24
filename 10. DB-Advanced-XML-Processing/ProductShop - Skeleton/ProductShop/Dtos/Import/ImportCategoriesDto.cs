using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{
   //<Categories>
   //<Category>
   //    <name>Drugs</name>
   [XmlType("Category")]
   public class ImportCategoriesDto
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
