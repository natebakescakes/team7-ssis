﻿using System;
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NotificationId { get; set; }
        public virtual NotificationType NotificationType { get; set; }
        [MaxLength(200)]
        public String Contents { get; set; }
        public virtual Status Status { get; set; }
        public virtual ApplicationUser CreatedFor { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}