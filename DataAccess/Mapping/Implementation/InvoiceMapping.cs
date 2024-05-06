using DataAccess.Models;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapping.Implementation
{
    public class InvoiceMapping : IInvoiceMapping
    {
        public DataTable ListToTableInvoices(InvoiceListModel invoiceList)
        {
            DataTable table = new DataTable();
            try
            {
                using (var reader = ObjectReader.Create(invoiceList.Invoices.Invoice,
                                                    "InvoiceNumber", "InvoiceDate", "ItemsNumber", "TotalAmount"))
                {
                    table.Load(reader);
                }
                return table;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable ListToTableInvoiceItems(InvoiceListModel invoiceList)
        {
            DataTable table = new DataTable();
            try
            {
                using (var reader = ObjectReader.Create(invoiceList.Invoices.Invoice.SelectMany(x => x.ItemList.Item),
                                                    "ItemNumber", "Price", "Description", "InvoiceNumber"))
                {
                    table.Load(reader);
                }
                return table;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<InvoiceTransferStatusModel> TableToListInvoiceTransferStatus(DataTable dt)
        {
            List<InvoiceTransferStatusModel> invoiceTransferStatus = new List<InvoiceTransferStatusModel>();
            try
            {
                invoiceTransferStatus = (from DataRow dr in dt.Rows
                                         select new InvoiceTransferStatusModel()
                                         {
                                             InvoiceNumber = dr["InvoiceNumber"].ToString(),
                                             IsOk = dr["IsOk"].ToString()
                                         }).ToList();
                return invoiceTransferStatus;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
