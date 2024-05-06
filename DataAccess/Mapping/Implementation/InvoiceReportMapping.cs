using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapping.Implementation
{
    public class InvoiceReportMapping : IInvoiceReportMapping
    {
        public InvoiceListModel DataTableToListInvoiceReport(DataTable databaseResponse)
        {
            try
            {
                InvoiceListModel report = new InvoiceListModel();
                report.Date = DateTime.Now;
                report.Invoices = InvoicesOfReport(databaseResponse);
                return report;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private InvoicesWrapperModel InvoicesOfReport(DataTable databaseResponse)
        {
            InvoicesWrapperModel invoices = new InvoicesWrapperModel();
            try
            {
                var listInvoices = databaseResponse.AsEnumerable().Select(x => new
                {
                    InvoiceNumber = (string)x["InvoiceNumber"],
                    InvoiceDate = (DateTime)x["InvoiceTimeStamp"],
                    ItemsNumber = (int)x["NumberOfItems"],
                    TotalAmount = (double)x["TotalAmount"]
                }).DistinctBy(x => x.InvoiceNumber).ToList();

                foreach (var invoice in listInvoices)
                {
                    invoices.Invoice.Add(new InvoiceModel()
                    {
                        InvoiceNumber = invoice.InvoiceNumber,
                        InvoiceDate = invoice.InvoiceDate,
                        ItemsNumber = invoice.ItemsNumber,
                        TotalAmount = invoice.TotalAmount,
                        ItemList = ItemsOfInvoicesForReport(databaseResponse, invoice.InvoiceNumber)
                    });
                }

                return invoices;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private ItemsWrapperModel ItemsOfInvoicesForReport(DataTable databaseResponse, string invoiceNumber)
        {
            ItemsWrapperModel items = new ItemsWrapperModel();
            try
            {
                var invoiceItems = databaseResponse.AsEnumerable().Where(x => (string)x["InvoiceNumber"] == invoiceNumber).ToList();
                if (invoiceItems.Count() <= 0 || invoiceItems == null)
                    return items;

                var listItems = invoiceItems
                    .Select(x => new
                    {
                        ItemNumber = (long)x["ItemNumber"],
                        Price = (double)x["Price"],
                        Description = (string)x["Description"],
                        InvoiceNumber = (string)invoiceNumber
                    })
                    .ToList();

                foreach (var item in listItems)
                {
                    items.Item.Add(new ItemModel()
                    {
                        ItemNumber = item.ItemNumber.ToString(),
                        Price = item.Price,
                        Description = item.Description,
                        InvoiceNumber = item.InvoiceNumber
                    });
                }

                return items;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
