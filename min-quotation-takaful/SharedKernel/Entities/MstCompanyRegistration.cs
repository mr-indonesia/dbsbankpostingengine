using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Entities
{
    public class MstCompanyRegistration
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyType { get; set; }
        public string CategoryCode { get; set; }
        public string LobCode { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyAddress2 { get; set; }
        public string KotaMadya { get; set; }
        public string Propinsi { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Pic { get; set; }
        public string Pic2 { get; set; }
        public string PicTitle { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Npwp { get; set; }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public string StatusCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

    }
}
