using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.DTOS
{
    public class WorkflowDTO
    {
        [Display(Name = "Workflow Id")]
        public string HeaderID { get; set; }
        [Display(Name = "Workflow Name")]
        public string Name { get; set; }
        [Display(Name = "Product Group")]
        public string CategoryTypeID { get; set; }
        [Display(Name = "Product Name")]
        public string CategoryTypeName { get; set; }
        [Display(Name = "Status")]
        public bool Status { get; set; }
        [Display(Name = "Status Desc")]
        public string StatusDesc {  get; set; }
        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }
        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }
        [Display(Name = "Company Code")]
        public string DataAreaID { get; set; }
        [Display(Name = "Company Desc")]
        public string DataAreaDesc { get; set; }
        [Display(Name = "COA")]
        public string COA { get; set; }
        [Display(Name = "COA Desc")]
        public string COADesc { get; set; }
        [Display(Name = "Created BY")]
        public List<string> CreatedBy { get; set; }
        [Display(Name = "Workflow Details")]
        public List<WorkflowDetailDTO> Details { get; set; }
    }

    public class WorkflowDetailDTO
    {
        [Display(Name = "Workflow Id")]
        public string HeaderID { get; set; }
        [Display(Name = "Seq")]
        public int State { get; set; }
        [Display(Name = "Role Id")]
        public string RoleID { get; set; }
        [Display(Name = "Role Name")]
        public string Name { get; set; }
        [Display(Name = "Approver")]
        public string Approver { get; set; }
        [Display(Name = "Escalation Method")]
        public string EscalationMethod { get; set; }
        [Display(Name = "Amout Id")]
        public string AmountID { get; set; }
        [Display(Name = "Amount Desc")]
        public string AmountDesc { get; set; }
    }

    public class WorkflowStep
    {
        //properties here
    }
}
