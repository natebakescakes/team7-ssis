using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using team7_ssis.Models;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    [Authorize]
    [RoutePrefix("api/Notification")]
    public class NotificationApiController : ApiController
    {
        public ApplicationDbContext context { get; set; }
        private static string pixel= "c4rWWOQR6Ms:APA91bEaZlfPSZ6_P17Y-ZK5w1T0cIW6knoA8Vf8heX9NnNptP1sEIKVWJlXrEHb2_qiso1JQeQ02QUg78bhKwGXi2P5lip1UK0JZ39T5j8w7xTX8e6QkA8uH-i0Q3GL1GYQ3zYQw9BkQMG1iFhvyg7SpK0W-nJt1w";
        private static string s8 = "eR2HvAS_ib8:APA91bH4KVd5vdTdW--GnYTDrCUceCDyt_RSlGqWyxRJ3PFYKriP_HrxUSTuBpm_47TC48rywctxnmNj5iuxWuY__qb3sRErFlyV8kEJFrVZ-mCce6I1lf10e8BpI7-ngq1lrdp9JAs5P7digvsy0NsHOh1oRRCnTw";
        private static string nexus = "cbyMssJA0mE:APA91bF8DdZVBtnag_4UrjdthkrU_t489E4nVH_TAoEpmhIu023u-dhTWoWdh1_FSuWceWYhrHxv336G-WYLoz8gs-4IF-NuVEZXVqMts-HnSfLeHQU67_0gZbTDaSzyGNFibh0ksqJvzl_iHZKmdiT8Mf9RrDMhgw";
        private static string ServerKey = "AAAASdSX054:APA91bEPhId59S4Qtl00O3llHaXrPay5RjXDiMnip9ofs1IbIssamoFS20PrSOyC47wIFetCJyWwbxy0SzTIm0hvOh_titJu5OfNvXUpEMFZL2g4vVRY5n1B_bOBrgaZ5tUWQ-jQFJz4Xh7O3gw9seWV86p7Oon5Eg";
        
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

        [Route("GetCurrentUser")]
        public IHttpActionResult GetCurrentUser()
        {
            var user = new UserService(context).FindUserByEmail(CurrentUserName);
            var notifications = new NotificationService(context).FindNotificationsByUser(user);

            if (notifications.Count == 0) return NotFound();

            return Ok(notifications.OrderByDescending(n => n.CreatedDateTime).Select(notification => new NotificationViewModel()
            {
                NotificationId = notification.NotificationId,
                NotificationType = notification.NotificationType.Name,
                Contents = notification.Contents,
                Status = notification.Status.Name,
                CreatedDateTIme = notification.CreatedDateTime
            }));
        }

        [Route("send/{id}")]
        [HttpGet]
        public IHttpActionResult SendNotification(string id)
        {

            //get Notification object from db
            NotificationService notificationService = new NotificationService(context);
            Notification notification = notificationService.FindNotificationById(int.Parse(id));

            string result;
            var webAddr = "https://fcm.googleapis.com/fcm/send";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("Authorization:key=" + ServerKey);
            httpWebRequest.Method = "POST";

            string AppKey;
            if (notification.CreatedFor == null) AppKey = s8;
            else AppKey = nexus;

            FirebaseObject obj = new FirebaseObject()
            {
                to = AppKey,
                notification = new NotificationDetail()
                {
                    body=notification.Contents,
                    title = notification.NotificationType.Name
                }
            };
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(obj);
      
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                 result = streamReader.ReadToEnd();
            }

            return Ok(result);
        }

        [Route("send/email/{id}")]
        [HttpGet]
        public IHttpActionResult SendEmail(string id)
        {
            //get Notification object from db
            NotificationService notificationService = new NotificationService(context);
            UserService userService = new UserService(context);
            Notification n = notificationService.FindNotificationById(int.Parse(id));

            string result = "";
            string header = "<h2>TEAM 7 STATIONERY STORE</h2>";
            string link = "Please use this url to to check the status : http://localhost:50831/";
            string disclaimer = "<i>This is a computer-generated email. Please do not reply to this email. For enquiries, please contact your system administrator.</i>";

            MailMessage m = new MailMessage();
            SmtpClient sc = new SmtpClient();
            try
            {
                m.From = new MailAddress("team7stationery@gmail.com");
                m.To.Add(n.CreatedFor.Email);
                //m.To.Add("e0282927@u.nus.edu");
                m.To.Add("e0284048@u.nus.edu"); //for UAT we will hardcode the email to streamline the notifications

                m.Subject =String.Format("Team7 Stationery Store [{0}]",n.NotificationType.Name);
                m.IsBodyHtml = true;
                m.Body = String.Format("{0}<br />{1}<br /><br />{2}<br /><br />{3}", header,n.Contents,link, disclaimer);
                sc.Host = "smtp.gmail.com";
                sc.Port = 587;
                sc.Credentials = new System.Net.NetworkCredential("team7stationery@gmail.com", "passwordq1w2");

                sc.EnableSsl = true;
                sc.Send(m);
               
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return Ok(result);

        }

    }
}
