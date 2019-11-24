using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{

    // <Product>
    //  <name>SNORING HP</name>
    //  <price>53.59</price>
    //  <sellerId>24</sellerId>
    //  <buyerId>30</buyerId>

    [XmlType("Product")]
    public class ImportProductsDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]

        public decimal Price { get; set; }

        [XmlElement("sellerId")]

        public int SellerId { get; set; }

        [XmlElement("buyerId")]

        public int? BuyerId { get; set; }
    }
}
