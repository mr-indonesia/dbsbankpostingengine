using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http.Extensions;
using System.Linq;
using System.Text;
using System.Security.Policy;

namespace WebApps
{
    public static class HtmlHelperExt
    {
        public static Task RenderPartialIf(this IHtmlHelper htmlHelper, string partialViewName, bool condition)
        {
            ArgumentNullException.ThrowIfNull(htmlHelper);
            ArgumentNullException.ThrowIfNull(partialViewName);
            return htmlHelper.RenderPartialAsync(partialViewName);
        }

        public static HtmlString RouteIf(this IHtmlHelper helper, string value, string attribute, string strAction = "")
        {
            var currentController =
                (helper.ViewContext.RouteData.Values["controller"] ?? string.Empty).ToString().UnDash();
            var currentAction =
                (AppsHttpContext.Current.Request.GetEncodedPathAndQuery() ?? string.Empty).ToString().UnDash();

            var arrValue = value.Split("/");
            string strval = arrValue.Count() > 1 ? arrValue[1].ToString() : arrValue[0].ToString();

            bool hasAction = false;

            var hasController = strval.Equals(currentController, StringComparison.InvariantCultureIgnoreCase);            

            if (!string.IsNullOrEmpty(strAction))
            {
                int index = strAction.IndexOf('?');
                string result = index >= 0 ? strAction.Substring(0, index) : strAction;
                strAction = result;
                currentAction = index >= 0 ? strAction.Substring(0, index) : currentAction;

                if (strAction == "/")
                    hasAction = value.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase) || strAction.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase);
                else
                    hasAction =  value.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                hasAction = value.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase) || strAction.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase);
            }
            

            return (hasAction && hasController ? new HtmlString(attribute) : new HtmlString(string.Empty));
        }

		public static HtmlString ValidationBootstrap(this IHtmlHelper htmlHelper, string alertType = "danger",
			string heading = "")
		{
			if (htmlHelper.ViewData.ModelState.IsValid)
				return new HtmlString(string.Empty);

			var sb = new StringBuilder();

			sb.AppendFormat("<div class=\"alert alert-{0} alert-block\">", alertType);
			sb.Append("<button class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button>");

			if (!string.IsNullOrEmpty(heading))
			{
				sb.AppendFormat("<h4 class=\"alert-heading\">{0}</h4>", heading);
			}

			//sb.Append(htmlHelper.ValidationSummary());
			sb.Append(htmlHelper.ViewBag.errmsg);
			sb.Append("</div>");

			return new HtmlString(sb.ToString());
		}

		public static string GetRawUrl(this IHtmlHelper helper)
        {
			var currentAction =
				(AppsHttpContext.Current.Request.GetEncodedPathAndQuery() ?? string.Empty).ToString().UnDash();
            return currentAction;
		}

		public static string GetCurrentController(this IHtmlHelper helper)
		{
			var currentController =
				(helper.ViewContext.RouteData.Values["controller"] ?? string.Empty).ToString().UnDash();
			return currentController;
		}
	}

    public static class Settings
    {
        public static readonly bool EnableTiles = true;
    }

    public static class StringExt
    {
        public static string UnDash(this object value)
        {
            return ((value as string) ?? string.Empty).UnDash();
        }

        public static string UnDash(this string value)
        {
            return (value ?? string.Empty).Replace("-", string.Empty);
        }
    }
}
