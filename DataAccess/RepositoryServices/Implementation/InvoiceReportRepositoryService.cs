using DataAccess.Models;
using DataAccess.RepositoryAccess;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static DataAccess.RepositoryAccess.TypeParameter;

namespace DataAccess.RepositoryServices.Implementation
{
    public class InvoiceReportRepositoryService : IInvoiceReportRepositoryService
    {
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;
        private SqlConnection? _conn;
        public InvoiceReportRepositoryService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionStrings:MainDB"];
            if (connectionString != null)
                _conn = new SqlConnection(connectionString);
        }

        public InvoiceListModel DailyInvoiceReport(DateTime today)
        {
            try
            {
                if (_conn == null)
                    throw new Exception("Could not enstablish connection with database!");

                DataTable databaseResponse = new DataTable();
                DataAccessParameterList parameters = new DataAccessParameterList();
                parameters.ParametarAdd("@Modul", "InvoiceXMLTransferService", TypeParametar.NVarChar);
                parameters.ParametarAdd("@Date", today, TypeParametar.DateTime);

                SqlAccessManager.SelectData(_conn, CommandType.StoredProcedure, databaseResponse, "spDailyInvoiceReport", parameters);

                if (databaseResponse == null)
                    throw new Exception("Procedure returned null object.");

                return DataTableToListInvoiceReport(databaseResponse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal InvoiceListModel DataTableToListInvoiceReport(DataTable databaseResponse)
        {
            InvoiceListModel report = new InvoiceListModel();
            report.Date = DateTime.Now;
            report.Invoices = InvoicesOfReport(databaseResponse);
            return report;
        }

        private InvoicesWrapperModel InvoicesOfReport(DataTable databaseResponse)
        {
            InvoicesWrapperModel invoices = new InvoicesWrapperModel();

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

        private ItemsWrapperModel ItemsOfInvoicesForReport(DataTable databaseResponse, string invoiceNumber)
        {
            ItemsWrapperModel items = new ItemsWrapperModel();

            var invoiceItems = databaseResponse.AsEnumerable().Where(x => (string)x["InvoiceNumber"] == invoiceNumber).ToList();
            if (invoiceItems.Count() <= 0 || invoiceItems == null)
                return items;

            var listItems = invoiceItems
                .Select(x => new
                 {
                     ItemNumber = (long) x["ItemNumber"],
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
    }
}
