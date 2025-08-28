using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Entities
{
	public class MGRAgent
	{
		public string CODE { get; set; }
		public string CD { get; set; }
		public string CD_DESCR { get; set; }
		public string SUBCD { get; set; }
		public string SUBCD_DESCR { get; set; }
		public string FRONT_NAME { get; set; }
		public string LAST_NAME { get; set; }
		public string MID_NAME { get; set; }
		public string FULLNAME { get; set; }
		public DateTime DOB { get; set; }
		public string POB { get; set; }
		public string GENDER { get; set; }
		public DateTime? JOINTDATE { get; set; }
		public string PHONE { get; set; }
		public string EMAIL { get; set; }
		public string UPLINER { get; set; }
		public string UPLINER_NAME { get; set; }
		public string BRANCH_CODE { get; set; }
		public string BRANCH_DESCR { get; set; }
		public int ACTIVE { get; set; }
		public string ACTIVE_DESCR { get; set; }
		public int READ_ONLY { get; set; }
		public int? TRACK { get; set; }
		public string TRACK_DESCR { get; set; }
		public string AGENCY_CODE { get; set; }
		public string AGENCY_NAME { get; set; }
		public string REFERRAL_CODE { get; set; }
		public string REFERRAL_NAME { get; set; }
		public string MARKET_SEGMENT { get; set; }
		public string MARKET_SEGMENT_DESCR { get; set; }
		public string CREATEBY { get; set; }
		public string AREA_CODE { get; set; }
		public string AREA_NAME { get; set; }
	}
}
