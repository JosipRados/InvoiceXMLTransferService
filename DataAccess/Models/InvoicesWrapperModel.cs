using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccess.Models
{
    [XmlRoot(ElementName = "Invoices")]
    public class InvoicesWrapperModel
    {
        [XmlElement(ElementName = "Invoice")]
        public List<InvoiceModel> Invoice { get; set; } = new List<InvoiceModel>();
    }
}
