using DataAccess.Models;
using DataAccess.RepositoryServices;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InvoiceXMLTransferService.Services.Implementation
{
    public class InvoiceReportService : IInvoiceReportService
    {
        private readonly IInvoiceReportRepositoryService _invoiceReportRepositoryService;
        private readonly IConfiguration _configuration;
        private readonly ILoggingRepositoryService _logger;
        private string? SFTPServer;
        private string? SFTPUsername;
        private string? SFTPPassword;

        public InvoiceReportService(IInvoiceReportRepositoryService invoiceReportRepositoryService, IConfiguration configuration,
                                    ILoggingRepositoryService loggingRepositoryService)
        {
            _invoiceReportRepositoryService = invoiceReportRepositoryService;
            _configuration = configuration;
            _logger = loggingRepositoryService;
            SFTPServer = _configuration["SFTPConnection:MainServer:ServerIP"];
            SFTPUsername = _configuration["SFTPConnection:MainServer:Username"];
            SFTPPassword = _configuration["SFTPConnection:MainServer:Password"];
        }

        public void FetchDailyInvoiceReport(DateTime today)
        {
            try
            {
                var report = _invoiceReportRepositoryService.DailyInvoiceReport(DateTime.Now);
                if (report == null)
                    throw new Exception("Report is null.");
                SerializeInvoiceReport(report);
            }
            catch (Exception ex)
            {
                _logger.WriteLog(new LoggingModel()
                {
                    IsError = true,
                    Description = ex.Message,
                    Modul = "InvoiceXMLTransferService"
                });
            }
            

        }

        public void SerializeInvoiceReport(InvoiceListModel report)
        {
            try
            {
                if (SFTPServer == null || SFTPUsername == null || SFTPPassword == null)
                    throw new Exception("Not valid connection data.");

                using (SftpClient client = new SftpClient(new PasswordConnectionInfo(SFTPServer, SFTPUsername, SFTPPassword)))
                {
                    client.Connect();
                    MemoryStream ms = new MemoryStream();
                    XmlSerializer serializer = new XmlSerializer(typeof(InvoiceListModel));
                    serializer.Serialize(ms, report);
                    ms.Position = 0;
                    client.UploadFile(ms, @"\Invoice_Transfer\InvoiceReports\" + DateTime.Now.ToString("yyyy-MM-dd") + "_Report.xml");

                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
