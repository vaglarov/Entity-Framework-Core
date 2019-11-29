using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExpotDto
{
    [XmlType("User")]
    public class ExportUserDto
    {
        [XmlAttribute("username")]
        public string Username { get; set; }

        [XmlArray("Purchases")]
        public ExportPurchaseDto[] Purchases { get; set; }

        [XmlElement("TotalSpent")]
        public decimal TotalSpet { get; set; }
    }


    [XmlType("Purchase")]
    public class ExportPurchaseDto
    {
      //         <Card>7991 7779 5123 9211</Card>
      // <Cvc>340</Cvc>
      // <Date>2017-08-31 17:09</Date>
      // <Game title = "Counter-Strike: Global Offensive" >
      //   < Genre > Action </ Genre >
      //   < Price > 12.49 </ Price >
      [XmlElement("Card")]
        public string Card { get; set; }

        [XmlElement("Cvc")]
        public string  Cvc { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlElement("Game")]
        public ExportGameDto Game { get; set; }

    }

    [XmlType("Game")]
    public class ExportGameDto
    {
        //   < Genre > Action </ Genre >
        //   < Price > 12.49 </ Price >
        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlElement("Genre")]
        public string Genre { get; set; }

        [XmlElement("Price")]
        public decimal Price { get; set; }

    }
}
