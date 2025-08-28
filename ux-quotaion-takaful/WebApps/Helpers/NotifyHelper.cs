using Apps.Core.Models.SharedModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApps.Helpers
{
    public static class NotifyHelper
    {
        public enum AllertType
        {
            Info, Success, Warning, Error
        }

        private static string getNotif(WebAlertDialogModel alertModel)
        {
            string js = "$.smallBox({title: '" + alertModel.Title + "',content: '" + alertModel.Content + "',color: '" + alertModel.Color + "',iconSmall: 'fa fa-bell bounce animated',timeout: " + alertModel.Timeout + " });";
            return js;
        }

        public static void SetNotification(string title, string message, AllertType allertType, Controller con)
        {
            WebAlertDialogModel alertModel = new WebAlertDialogModel();
            message = message.Replace("'", " ");
            message = message.Replace(System.Environment.NewLine, " ");
            alertModel.Title = title;
            alertModel.Timeout = 9000;
            switch (allertType)
            {
                case AllertType.Info:
                    alertModel.Content = string.Format("Info : {0}", message);
                    alertModel.Color = "#0288D1";
                    break;
                case AllertType.Success:
                    alertModel.Content = string.Format("Success : {0}", message);
                    alertModel.Color = "#64DD17";
                    break;
                case AllertType.Warning:
                    alertModel.Content = string.Format("Warning : {0}", message);
                    alertModel.Color = "#FFD600";
                    break;
                case AllertType.Error:
                    alertModel.Content = string.Format("Error : {0}", message);
                    alertModel.Color = "#FF8A80";
                    break;
            }
            AppsHttpContext.Current.Session.SetString("MessageInfo", getNotif(alertModel));
        }
    }
}
