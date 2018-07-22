using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.ViewModels
{
    public class NotificationViewModel
    {
        public string NotificationType { get; set; }
        public string Contents { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDateTIme { get; set; }
    }
}