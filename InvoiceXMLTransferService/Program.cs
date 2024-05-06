using InvoiceXMLTransferService;
using InvoiceXMLTransferService.Services;
using InvoiceXMLTransferService.Services.Implementation;
using DataAccess.RepositoryServices;
using DataAccess.RepositoryServices.Implementation;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IInvoiceService, InvoiceService>();
        services.AddTransient<IInvoiceRepositoryService, InvoiceRepositoryService>();
        services.AddTransient<IInvoiceReportService, InvoiceReportService>();
        services.AddTransient<IInvoiceReportRepositoryService, InvoiceReportRepositoryService>();
        services.AddTransient<ILoggingService, LoggingService>();
        services.AddTransient<ILoggingRepositoryService, LoggingRepositoryService>();
        services.AddHostedService<Worker>();
        
    })
    .UseWindowsService()
    .Build();

await host.RunAsync();
