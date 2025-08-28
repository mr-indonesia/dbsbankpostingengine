using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Models.Migrations
{
	public class Migration
	{
		public partial class AgentMigrationViewModel
		{
            [Display(Name = "Agent Code")]
            public string AgentCode { get; set; }
            [Display(Name = "Channel Code")]
            public string ChanelCode { get; set; }
            [Display(Name = "Channel Description")]
            public string ChannelDesc { get; set; }
            [Display(Name = "Sub Channel Code")]
            public string SubChannelCode { get; set; }
            [Display(Name = "Sub Channel Description")]
            public string SubChannelDesc { get; set; }
            [Display(Name = "First Name")]
            public string FRONT_NAME { get; set; }
            [Display(Name = "Last Name")]
            public string LAST_NAME { get; set; }
            [Display(Name = "Middle Name")]
            public string MID_NAME { get; set; }
            [Display(Name = "Full Name")]
            public string FULLNAME { get; set; }
            [Display(Name = "Date Of Birth")]
            public DateTime DateOfBirth { get; set; }
            [Display(Name = "Gender")]
            public string Gender { get; set; }
            [Display(Name = "Join Date")]
            public DateTime? JoinDate { get; set; }
			public string Phone { get; set; }
			public string Email { get; set; }
			public string Upliner { get; set; }
            [Display(Name = "Upliner Name")]
            public string UplinerName { get; set; }

		}
	}
}
