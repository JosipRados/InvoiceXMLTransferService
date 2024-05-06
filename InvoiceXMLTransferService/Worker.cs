using InvoiceXMLTransferService.Services;

namespace InvoiceXMLTransferService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IInvoiceService _invoiceService;
        private readonly IInvoiceReportService _invoiceReportService;

        public Worker(ILogger<Worker> logger, IInvoiceService invoiceService, IInvoiceReportService invoiceReportService)
        {
            _logger = logger;
            _invoiceService = invoiceService;
            _invoiceReportService = invoiceReportService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //await Task.Delay(1000, stoppingToken);
                //_invoiceService.InvoiceTransfer();
                invoiceReportService.FetchDailyInvoiceReport(DateTime.Now);
            }
        }
    }
}