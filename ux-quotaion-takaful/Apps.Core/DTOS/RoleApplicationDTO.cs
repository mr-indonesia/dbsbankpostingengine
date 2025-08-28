using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.DTOS
{
    public class RoleApplicationDTO
    {
        public string RoleApplicationId { get; set; }
        public string RoleName { get; set; }
        public string UserCode { get; set; }
        public string FrontName { get; set; }
        public string MiddleName { get; set; }
        public string LastName {  get; set; }
    }
}
