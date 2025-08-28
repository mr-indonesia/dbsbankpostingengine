using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.DTOS
{
    public class UserMappingRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public List<string> RoleCodes { get; set; } = new List<string>();

        [Required]
        public List<string> BranchCodes { get; set; } = new List<string>();
    }

    public class UserMappingResponse
    {
        public string UserId { get; set; }
        public List<string> RoleCodes { get; set; }
        public List<string> BranchCodes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
