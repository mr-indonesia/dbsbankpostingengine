using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models.Company
{
    public class CompanySearchViewModel
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public string Status { get; set; }
        public string CategoryCode { get; set; }
        public string LOB { get; set; }
        public string SubChannel { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
