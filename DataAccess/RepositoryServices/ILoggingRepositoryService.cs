using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.RepositoryServices
{
    public interface ILoggingRepositoryService
    {
        void WriteLog(LoggingModel log);
        void WriteLogTxt(string message);
    }
}
