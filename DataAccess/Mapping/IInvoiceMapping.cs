using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapping
{
    public interface IInvoiceMapping
    {
        DataTable ListToTableInvoices(InvoiceListModel invoiceList);
        DataTable ListToTableInvoiceItems(InvoiceListModel invoiceList);
        List<InvoiceTransferStatusModel> TableToListInvoiceTransferStatus(DataTable dt);
    }
}
