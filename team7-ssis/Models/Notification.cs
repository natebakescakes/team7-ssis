using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace team7_ssis.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        [MaxLength(200)]
        public String Contents { get; set; }
        public Status Status { get; set; }
        public ApplicationUser CreatedFor { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}