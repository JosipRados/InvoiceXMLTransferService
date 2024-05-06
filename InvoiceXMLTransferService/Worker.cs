using DataAccess.Models;
using DataAccess.RepositoryServices;
using InvoiceXMLTransferService.Services;

namespace InvoiceXMLTransferService
{
    public class Worker : BackgroundService
    {
        private readonly ILoggingRepositoryService _logger;
        private readonly IInvoiceService _invoiceService;
        private readonly IInvoiceReportService _invoiceReportService;
        private DateTime _currentDateTime;
        private TimeSpan _low = new TimeSpan(23, 0, 0); 
        private TimeSpan _high = new TimeSpan(23, 59, 59); 

        public Worker(IInvoiceService invoiceService, IInvoiceReportService invoiceReportService,
                      ILoggingRepositoryService loggingRepositoryService)
        {
            _logger = loggingRepositoryService;
            _invoiceService = invoiceService;
            _invoiceReportService = invoiceReportService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.WriteLog(new LoggingModel()
                {
                    IsError = false,
                    Description = "Worker started",
                    Modul = "InvoiceXMLTransferService"
                });
                _currentDateTime = DateTime.Now;

                _invoiceService.InvoiceTransfer();

                if(_currentDateTime.TimeOfDay > _low && _currentDateTime.TimeOfDay < _high)
                    _invoiceReportService.FetchDailyInvoiceReport(DateTime.Now);

                _logger.WriteLog(new LoggingModel()
                {
                    IsError = false,
                    Description = "Worker done",
                    Modul = "InvoiceXMLTransferService"
                });

                await Task.Delay(TimeSpan.FromMinutes(60));
            }
        }
    }
}