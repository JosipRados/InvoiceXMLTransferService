﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceXMLTransferService.Services
{
    public interface IInvoiceReportService
    {
        void FetchDailyInvoiceReport(DateTime today);
    }
}
