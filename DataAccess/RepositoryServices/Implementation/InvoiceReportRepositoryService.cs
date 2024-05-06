using DataAccess.Mapping;
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
        private readonly IInvoiceReportMapping _mapping;
        private readonly string? connectionString;
        private SqlConnection? _conn;
        public InvoiceReportRepositoryService(IConfiguration configuration, IInvoiceReportMapping invoiceReportMapping)
        {
            _configuration = configuration;
            _mapping = invoiceReportMapping;
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

                return _mapping.DataTableToListInvoiceReport(databaseResponse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
