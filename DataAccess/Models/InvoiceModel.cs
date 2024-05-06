using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccess.Models
{
    [XmlRoot(ElementName = "Invoice")]
    public class InvoiceModel
    {

        [XmlElement(ElementName = "InvoiceNumber")]
        public string InvoiceNumber { get; set; } = "";

        [XmlElement(ElementName = "InvoiceDate")]
        public DateTime InvoiceDate { get; set; }

        [XmlElement(ElementName = "ItemsNumber")]
        public int ItemsNumber { get; set; }

        [XmlElement(ElementName = "ItemList")]
        public ItemsWrapperModel ItemList { get; set; } = new ItemsWrapperModel();

        [XmlElement(ElementName = "TotalAmount")]
        public double TotalAmount { get; set; }
    }
}
