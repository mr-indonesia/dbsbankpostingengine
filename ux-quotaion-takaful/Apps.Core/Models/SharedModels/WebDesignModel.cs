using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Core.Models.SharedModels
{
    public class WebAlertDialogModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public int Timeout { get; set; }
    }

    public class ActionButtonModel
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
    }
}
