using InvoiceXMLTransferService;
using InvoiceXMLTransferService.Services;
using InvoiceXMLTransferService.Services.Implementation;
using DataAccess.RepositoryServices;
using DataAccess.RepositoryServices.Implementation;
using DataAccess.Mapping;
using DataAccess.Mapping.Implementation;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IInvoiceService, InvoiceService>();
        services.AddTransient<IInvoiceRepositoryService, InvoiceRepositoryService>();
        services.AddTransient<IInvoiceReportService, InvoiceReportService>();
        services.AddTransient<IInvoiceReportRepositoryService, InvoiceReportRepositoryService>();
        services.AddTransient<ILoggingRepositoryService, LoggingRepositoryService>();
        services.AddTransient<IInvoiceReportMapping, InvoiceReportMapping>();
        services.AddTransient<IInvoiceMapping, InvoiceMapping>();
        services.AddHostedService<Worker>();
        
    })
    .UseWindowsService()
    .Build();

await host.RunAsync();
