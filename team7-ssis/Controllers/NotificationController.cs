using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Services;

namespace team7_ssis.Controllers
{
    public class NotificationController : Controller
    {
        public NotificationController()
        {
            Context = new ApplicationDbContext();
        }

        public ApplicationDbContext Context { get; set; }

        // POST: Notification
        [HttpPost]
        public ActionResult Read(int notificationId)
        {
            Notification notification;
            try
            {
                notification = new NotificationService(Context).ReadNotification(notificationId);
            } catch (ArgumentException)
            {
                notification = new NotificationService(Context).FindNotificationById(notificationId);
            }

            switch (notification.NotificationType.NotificationTypeId) {
                case 1:
                    // Ready for Collection
                    // Redirect to Disbursement Details
                    var disbursementId = Regex.Match(notification.Contents, @"DSB-\d{6}-\d{3}");
                    return RedirectToAction("DisbursementDetails", "Disbursement", new { did = disbursementId });
                case 2:
                    // Redirect to Requisition Details
                    var requisitionId = Regex.Match(notification.Contents, @"REQ-\d{6}-\d{3}");
                    return RedirectToAction("RequisitionDetails", "Requisition", new { rid = requisitionId });
                case 3:
                    // Redirect to Stock Adjustment Details
                    var stockAdjustmentId = Regex.Match(notification.Contents, @"ADJ-\d{6}-\d{3}");
                    return RedirectToAction("Process", "StockAdjustment", new { Id = stockAdjustmentId.Value });
                default:
                    return RedirectToAction("Index", "Home");
            }
        }
    }
}