using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapping
{
    public interface IInvoiceReportMapping
    {
        InvoiceListModel DataTableToListInvoiceReport(DataTable databaseResponse);
    }
}
