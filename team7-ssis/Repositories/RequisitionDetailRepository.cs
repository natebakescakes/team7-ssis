using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class RequisitionDetailRepository : CrudMultiKeyRepository<RequisitionDetail, String, String>
    {
        public RequisitionDetailRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.RequisitionDetail;
        }
    }
}