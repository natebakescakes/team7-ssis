using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class NotificationService
    {
        ApplicationDbContext context;
        NotificationRepository notificationRepository; 

        public NotificationService(ApplicationDbContext context)
        {
            this.context = context;
            notificationRepository = new NotificationRepository(context);
        }

        public Notification CreateCollectionNotification(Disbursement disbursement, ApplicationUser recipient)
        {
            throw new NotImplementedException();
        }

        public Notification CreateStockAdjustmentApprovalNotification(StockAdjustment stockAdjustment, ApplicationUser recpient)
        {
            throw new NotImplementedException();

        }

        public Notification CreateRequisitionApprovalNotification(Requisition requisition, ApplicationUser recipient)
        {
            throw new NotImplementedException();

        }

        public List<Notification> FindNotificationsByUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public List<Notification> FindNotificationByType(NotificationType type)
        {
            throw new NotImplementedException();

        }

    }
}