using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{
    //  <CategoryProduct>
    //  <CategoryId>5</CategoryId>
    //  <ProductId>4</ProductId>
    [XmlType("CategoryProduct")]
    public class ImportMappingCategoriesProduct
    {
        [XmlElement("CategoryId")]
        public int CategoryId { get; set; }

        [XmlElement("ProductId")]

        public int ProductId { get; set; }
    }
}
