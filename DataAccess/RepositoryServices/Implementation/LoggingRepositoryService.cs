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
using static DataAccess.RepositoryAccess.TypeParameter;

namespace DataAccess.RepositoryServices.Implementation
{
    public class LoggingRepositoryService : ILoggingRepositoryService
    {
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;
        private SqlConnection? _conn;

        public LoggingRepositoryService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionStrings:MainDB"];
            if (connectionString != null)
                _conn = new SqlConnection(connectionString);
        }

        public void WriteLog(LoggingModel log)
        {
            try
            {
                if (_conn == null)
                    throw new Exception("Could not enstablish connection with database!");

                DataAccessParameterList parameters = new DataAccessParameterList();
                parameters.ParametarAdd("@Modul", log.Modul, TypeParametar.NVarChar);
                parameters.ParametarAdd("@Parameters", log.Parameters, TypeParametar.NVarChar);
                parameters.ParametarAdd("@Description", log.Description, TypeParametar.NVarChar);
                parameters.ParametarAdd("@ProcedureName", "none", TypeParametar.NVarChar);
                parameters.ParametarAdd("@IsError", log.IsError, TypeParametar.Bit);

                SqlAccessManager.ExecuteQuery(_conn, CommandType.StoredProcedure, "spWriteLog", parameters);

            }
            catch (Exception ex)
            {
                WriteLogTxt(ex.Message);
            }
        }

        public void WriteLogTxt(string message)
        {
            string logPath = "logFile.txt";

            using (StreamWriter stream = new StreamWriter(logPath, true))
            {
                stream.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " " + message);
            }
        }
    }
}
