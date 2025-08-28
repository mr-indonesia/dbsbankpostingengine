using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Models.Company
{
    public class CompanyViewModel
    {
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyType { get; set; }
        public string CompanyTypeDescr { get; set; }
        public string CategoryCode { get; set; }
        public string CompanyCategoryDesc { get; set; }
        public string LobCode { get; set; }
        public string CompanyLobDescr { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyAddress2 { get; set; }
        public string KotaMadya { get; set; }
        public string Propinsi { get; set; }
        public string PropinsiDesc { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Pic { get; set; }
        public string Pic2 { get; set; }
        public string PicTitle { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RegisterDate { get; set; }
        public string Npwp { get; set; }
        public string StatusCode { get; set; }
        public string StatusDesc { get; set; }
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public string ChanelCode { get; set; }
        public string ChannelDesc { get; set; }
        public string SubChannelCode { get; set; }
        public string SubChannelDistDesc { get; set; }
        public DateTime? AgentStartDate { get; set; }
    }
}
