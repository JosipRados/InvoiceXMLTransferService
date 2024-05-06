using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccess.Models
{
    [XmlRoot(ElementName = "InvoiceList")]
    public class InvoiceListModel
    {

        [XmlElement(ElementName = "Date")]
        public DateTime Date { get; set; }

        [XmlElement(ElementName = "Invoices")]
        public InvoicesWrapperModel Invoices { get; set; } = new InvoicesWrapperModel();
    }
}
