using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Constants
{
	public static class LogIds
	{
		public static readonly string FINISH_SUCCESSFULLY = "0";
		public static readonly string FINISH_BUSINESS_ERROR = "1";
		public static readonly string FINISH_SYSTEM_ERROR = "2";
		public static readonly string FINISH_WITH_WARNING = "3";
		public static readonly string INPROGRESS = "4";
		public static readonly string TIMEOUT = "5";
	}
}
