using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Prisoner")]
    public class ImportPrisonersDtoCheck
    {
        //       <Prisoners>
        // <Prisoner id = "4" />
        // < Prisoner id="1" />
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
