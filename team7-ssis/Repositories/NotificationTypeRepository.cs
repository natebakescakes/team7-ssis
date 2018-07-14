using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class NotificationTypeRepository : CrudRepository<NotificationType, int>
    {
        public NotificationTypeRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.NotificationType;
        }
    }
}