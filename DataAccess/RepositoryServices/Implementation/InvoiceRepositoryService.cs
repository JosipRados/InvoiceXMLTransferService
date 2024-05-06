using DataAccess.Models;
using DataAccess.RepositoryAccess;
using FastMember;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.RepositoryAccess.TypeParameter;

namespace DataAccess.RepositoryServices.Implementation
{
    public class InvoiceRepositoryService : IInvoiceRepositoryService
    {
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;
        private SqlConnection? _conn;

        public InvoiceRepositoryService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionStrings:MainDB"];
            if(connectionString != null)
                _conn = new SqlConnection(connectionString);
        }

        public List<InvoiceTransferStatusModel> ImportInvoices(InvoiceListModel invoices)
        {
            try
            {
                if (_conn == null)
                    throw new Exception("Could not enstablish connection with database!");

                DataTable databaseResponse = new DataTable();
                DataAccessParameterList parameters = new DataAccessParameterList();
                parameters.ParametarAdd("@Modul", "InvoiceXMLTransferService", TypeParametar.NVarChar);
                parameters.ParametarAdd("@Invoices", ListToTableInvoices(invoices), TypeParametar.Structured);
                parameters.ParametarAdd("@InvoiceItems", ListToTableInvoiceItems(invoices), TypeParametar.Structured);

                SqlAccessManager.SelectData(_conn, CommandType.StoredProcedure, databaseResponse, "spImportInvoices", parameters);

                if (databaseResponse == null)
                    throw new Exception("Procedure returned null object.");

                return TableToListInvoiceTransferStatus(databaseResponse);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private DataTable ListToTableInvoices(InvoiceListModel invoiceList)
        {
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(invoiceList.Invoices.Invoice, 
                                                    "InvoiceNumber", "InvoiceDate", "ItemsNumber", "TotalAmount"))
            {
                table.Load(reader);
            }
            return table;
        }

        private DataTable ListToTableInvoiceItems(InvoiceListModel invoiceList)
        {
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(invoiceList.Invoices.Invoice.SelectMany(x => x.ItemList.Item),
                                                    "ItemNumber", "Price", "Description", "InvoiceNumber"))
            {
                table.Load(reader);
            }
            return table;
        }

        private List<InvoiceTransferStatusModel> TableToListInvoiceTransferStatus(DataTable dt)
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
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
