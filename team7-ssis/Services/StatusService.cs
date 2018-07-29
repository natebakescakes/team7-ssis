using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class StatusService
    {
        private StatusRepository statusRepository;

        public StatusService(ApplicationDbContext context)
        {
            this.statusRepository = new StatusRepository(context);
        }

        public Status FindStatusByStatusId(int statusId)
        {
            return statusRepository.FindById(statusId);
        }

      
    }
}