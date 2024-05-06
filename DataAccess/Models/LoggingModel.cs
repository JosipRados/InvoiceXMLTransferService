using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class LoggingModel
    {
        public string Modul { get; set; } = "";
        public string Parameters { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsError { get; set; }
    }
}
