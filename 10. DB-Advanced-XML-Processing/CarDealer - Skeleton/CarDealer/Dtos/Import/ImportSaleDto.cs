using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    // <carId>234</carId>
    // <customerId>23</customerId>
    // <discount>50</discount>

    [XmlType("Sale")]
    public class ImportSaleDto
    {
        [XmlElement("discount")]
        public decimal Discount { get; set; }

        [XmlElement("carId")]

        public int CarId { get; set; }

        [XmlElement("customerId")]

        public int CustomerId { get; set; }
    }
}
