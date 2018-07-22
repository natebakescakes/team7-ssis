using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Services;

namespace team7_ssis.Controllers
{
    public class NotificationController : Controller
    {
        private ApplicationDbContext context;
        
        public NotificationController()
        {
            context = new ApplicationDbContext();
        }

        // POST: Notification
        [HttpPost]
        [Route("Notification/Read")]
        public ActionResult ReadNotification(int notificationId)
        {
            var notification = new NotificationService(context).ReadNotification(notificationId);

            switch (notification.NotificationType.NotificationTypeId) {
                case 1:
                    // Ready for Collection
                    // Redirect to Requisition Details
                    return RedirectToAction("Index", "Home");
                case 2:
                    // Awaiting Requisition Approval
                    // Redirect to Requisition Details
                    return RedirectToAction("Index", "Home");
                case 3:
                    // Awaiting Stock Adjustment Approval
                    // Redirect to Stock Adjustment Details
                    return RedirectToAction("Index", "Home");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }
    }
}