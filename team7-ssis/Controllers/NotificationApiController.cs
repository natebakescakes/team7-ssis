using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using team7_ssis.Models;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    public class NotificationApiController : ApiController
    {
        private ApplicationDbContext context;

        public NotificationApiController()
        {
            context = new ApplicationDbContext();
            try
            {
                CurrentUserName = System.Web.HttpContext.Current.User.Identity.Name;
            }
            catch (NullReferenceException) { }
        }

        public String CurrentUserName { get; set; }

        [Route("api/notifications")]
        public IHttpActionResult GetNotificationsCurrentUser()
        {
            var user = new UserService(context).FindUserByEmail(CurrentUserName);
            var notifications = new NotificationService(context).FindNotificationsByUser(user);

            if (notifications.Count == 0) return NotFound();

            return Ok(notifications.Select(notification => new NotificationViewModel()
            {
                NotificationType = notification.NotificationType.Name,
                Contents = notification.Contents,
                Status = notification.Status.Name,
                CreatedDateTIme = notification.CreatedDateTime
            }));
        }
    }
}
