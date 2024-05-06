using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccess.Models
{
    [XmlRoot(ElementName = "InvoiceStatus")]
    public class InvoiceTransferStatusModel
    {
        [XmlElement(ElementName = "InvoiceNumber")]
        public string InvoiceNumber { get; set; } = "";

        [XmlElement(ElementName = "Status")]
        public string IsOk { get; set; } = "";
    }
}
