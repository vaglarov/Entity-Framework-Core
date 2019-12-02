using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MusicHub.DataProcessor.ExportDtos
{
    [XmlType("Song")]
    public class ExportSongDto
    {
        //<Song>
        //<SongName>Away</SongName>
        //<Writer>Norina Renihan</Writer>
        //<Performer>Lula Zuan</Performer>
        //<AlbumProducer>Georgi Milkov</AlbumProducer>
        //<Duration>00:05:35</Duration>
        [XmlElement("SongName")]
        public string SongName { get; set; }

        [XmlElement("Writer")]
        public string Writer { get; set; }

        [XmlElement("Performer")]
        public string Performer { get; set; }

        [XmlElement("AlbumProducer")]
        public string AlbumProducer { get; set; }

        [XmlElement("Duration")]
        public string Duration { get; set; }
    }
}
