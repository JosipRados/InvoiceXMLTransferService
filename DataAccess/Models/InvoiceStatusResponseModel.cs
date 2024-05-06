using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccess.Models
{
    [XmlRoot(ElementName = "InvoicesStatus")]
    public class InvoiceStatusResponseModel
    {
        [XmlElement(ElementName = "InvoiceName")]
        public string InvoiceName { get; set; } = "";

        [XmlElement(ElementName = "TimeStamp")]
        public string TimeStamp { get; set; } = "";

        [XmlArray("TransferStatus")]
        [XmlArrayItem("InvoiceStatus")]
        public List<InvoiceTransferStatusModel> TransferStatus { get; set; } = new List<InvoiceTransferStatusModel>();
    }
}
