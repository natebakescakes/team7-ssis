using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class DisbursementDetailRepository : CrudMultiKeyRepository<DisbursementDetail, String, String>
    {
        public DisbursementDetailRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.DisbursementDetail;
        }
    }
}