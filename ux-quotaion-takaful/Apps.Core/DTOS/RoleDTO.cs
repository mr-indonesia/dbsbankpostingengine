using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.DTOS
{
    public class RoleDTO
    {
        public bool Selected { get; set; }
        [Display(Name = "User Code")]
        public string UserCode { get; set; }
        [Display(Name = "Role Access Code")]
        public string RoleAccessID { get; set; }
        [Display(Name = "Role Access Name")]
        public string RoleAccessName { get; set; }
    }
}
