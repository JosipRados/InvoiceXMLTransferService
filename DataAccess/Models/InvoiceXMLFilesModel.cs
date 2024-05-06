using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataAccess.Models
{
    public class InvoiceXMLFilesModel
    {
        public string fileName { get; set; } = "";
        public XmlDocument xml { get; set; } = new XmlDocument();
    }
}
