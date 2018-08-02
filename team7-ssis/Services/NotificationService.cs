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
        NotificationTypeRepository notificationtypeRepository;
        StatusRepository statusRepository;

        public NotificationService(ApplicationDbContext context)
        {
            this.context = context;
            notificationRepository = new NotificationRepository(context);
            notificationtypeRepository = new NotificationTypeRepository(context);
            statusRepository = new StatusRepository(context);
        }

        public Notification ReadNotification(int notificationId)
        {
            var notification = FindNotificationById(notificationId);

            if (notification.Status.StatusId == 15)
                throw new ArgumentException("Notification already read");

            notification.Status = statusRepository.FindById(15);

            return Save(notification);
        }

        public Notification FindNotificationById(int notificationId)
        {
            return notificationRepository.FindById(notificationId);
        }

        private Notification InstantiateNotification(ApplicationUser recipient)
        {
            //instantiate new notification object and populating the fields
            return new Notification {
                NotificationId = IdService.GetNewNotificationId(context),
                CreatedDateTime = DateTime.Now,
                CreatedFor = recipient,

                Status = statusRepository.FindById(14)
            };

        }

        public Notification CreateNotification(Disbursement disbursement, ApplicationUser recipient)
        {
            Notification notification = InstantiateNotification(recipient);
                
            notification.NotificationType = notificationtypeRepository.FindById(1);
            notification.Contents = String.Format("Your Disbursement with ID: {0} is ready for collection", disbursement.DisbursementId);
            return this.Save(notification);

        }

        public Notification CreateNotification(Requisition requisition, ApplicationUser recipient)
        {
            Notification notification = InstantiateNotification(recipient);
           
            notification.NotificationType = notificationtypeRepository.FindById(2);
            notification.Contents = String.Format("New Stationery Requisition Request: {0} is pending approval", requisition.RequisitionId);
            return this.Save(notification);
        }

        public Notification CreateNotification(StockAdjustment SA, ApplicationUser recipient)
        {
            Notification notification = InstantiateNotification(recipient);

            notification.NotificationType = notificationtypeRepository.FindById(3);
            notification.Contents = String.Format("New Stock Adjustment Request: {0} is pending your approval", SA.StockAdjustmentId);

            return this.Save(notification);
        }

       
        public List<Notification> FindNotificationsByUser(ApplicationUser user)
        {
            if (user == null) return new List<Notification>();

            return notificationRepository.FindByUser(user).ToList();
        }

        public List<Notification> FindNotificationByType(NotificationType type)
        {
            return notificationRepository.FindByType(type).ToList();

        }

        public Notification Save(Notification notification)
        {
            return notificationRepository.Save(notification);
        }

    }
}