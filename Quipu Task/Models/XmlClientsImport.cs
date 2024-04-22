using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Quipu_Task.Models
{
    [Serializable]
    [XmlRoot("Clients")]
    public class XmlClientsImport
    {

        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("HomeAddress")]
        public List<string> HomeAddress { get; set; }
        [XmlElement("HomeAddress2")]
        public List<string> HomeAddress2 { get; set; }

        [XmlElement("BirthDate")]
        public DateOnly BirthDate { get; set; }
    }
}
