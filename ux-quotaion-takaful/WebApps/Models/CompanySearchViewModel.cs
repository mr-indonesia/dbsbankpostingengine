using Apps.Core.Models.Company;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using X.PagedList;

namespace WebApps.Models
{
    public class CompanySearchViewModel
    {
        // Properti untuk hasil pencarian
        public IPagedList<CompanyViewModel> Companies { get; set; }

        // Properti untuk parameter pencarian
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

        // Dropdown lists
        public IEnumerable<SelectListItem> DdlStatus { get; set; }
        public IEnumerable<SelectListItem> DdlCategory { get; set; }
        public IEnumerable<SelectListItem> DdlLOB { get; set; }
        public IEnumerable<SelectListItem> DdlSubchannel { get; set; }
    }
}
