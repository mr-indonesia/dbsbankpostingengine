using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.DTOS
{
    public class CompanyUserDTO
    {
        [Display(Name = "User Name")]
        public string UserCode { get; set; }
        [Display(Name = "Company Code")]
        public string BranchCode { get; set; }
    }
}
