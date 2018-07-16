using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Services
{
    public class DisbursementService
    {
        ApplicationDbContext context;

        public DisbursementService(ApplicationDbContext context)
        {
            this.context = context;
            
        }

        public Disbursement ShowDisbursementDetails(string id)
        {
            throw new NotImplementedException();
        }

        public Disbursement Save(Disbursement disbursement)
        {
            //Edit Disbursement and confirmDeliveryStatus and also delete
            throw new NotImplementedException();

        }


    }
}