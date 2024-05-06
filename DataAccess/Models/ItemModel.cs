using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccess.Models
{
    [XmlRoot(ElementName = "Item", IsNullable = false)]
    public class ItemModel
    {
        [XmlElement("ItemNumber")]
        public string ItemNumber { get; set; } = "";

        [XmlElement("Price")]
        public double Price { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; } = "";

        [XmlElement("InvoiceNumber")]
        public string InvoiceNumber { get; set; } = "";
    }
}
