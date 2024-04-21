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

        [XmlElement("Address")]
        public List<string> Addresses { get; set; }

        [XmlElement("BirthDate")]
        public DateOnly BirthDate { get; set; }
    }
}
