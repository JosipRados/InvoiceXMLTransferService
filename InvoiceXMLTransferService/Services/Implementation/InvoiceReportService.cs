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
        private string? SFTPServer;
        private string? SFTPUsername;
        private string? SFTPPassword;

        public InvoiceReportService(IInvoiceReportRepositoryService invoiceReportRepositoryService, IConfiguration configuration)
        {
            _invoiceReportRepositoryService = invoiceReportRepositoryService;
            _configuration = configuration;
            SFTPServer = _configuration["SFTPConnection:MainServer:ServerIP"];
            SFTPUsername = _configuration["SFTPConnection:MainServer:Username"];
            SFTPPassword = _configuration["SFTPConnection:MainServer:Password"];
        }

        public void FetchDailyInvoiceReport(DateTime today)
        {
            //Idem u bazu i skupljam sve račune današnjeg dana
            //vraćam ih u metodu i kreiram xml od njih
            //spremam xml na server
            try
            {
                var report = _invoiceReportRepositoryService.DailyInvoiceReport(DateTime.Now);
                if (report == null)
                    throw new Exception("Report is null.");
                SerializeInvoiceReport(report);
            }
            catch (Exception ex)
            {
                //log
            }
            

        }

        public void SerializeInvoiceReport(InvoiceListModel report)
        {
            try
            {
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
