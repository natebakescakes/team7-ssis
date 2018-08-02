using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.ViewModels
{
    public class NotificationViewModel
    {
        public int NotificationId { get; set; }
        public string NotificationType { get; set; }
        public string Contents { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDateTIme { get; set; }
     
    }

    public class FirebaseObject
    {
       
        public string to { get; set; }
        public NotificationDetail notification { get; set; }

    }

    public class NotificationDetail
    {
        public string body;
        public string title;

    }


}