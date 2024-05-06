using DataAccess.Mapping;
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
        private readonly IInvoiceMapping _mapping;
        private readonly string? connectionString;
        private SqlConnection? _conn;

        public InvoiceRepositoryService(IConfiguration configuration, IInvoiceMapping invoiceMapping)
        {
            _configuration = configuration;
            _mapping = invoiceMapping;
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
                parameters.ParametarAdd("@Invoices", _mapping.ListToTableInvoices(invoices), TypeParametar.Structured);
                parameters.ParametarAdd("@InvoiceItems", _mapping.ListToTableInvoiceItems(invoices), TypeParametar.Structured);

                SqlAccessManager.SelectData(_conn, CommandType.StoredProcedure, databaseResponse, "spImportInvoices", parameters);

                if (databaseResponse == null)
                    throw new Exception("Procedure returned null object.");

                return _mapping.TableToListInvoiceTransferStatus(databaseResponse);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
