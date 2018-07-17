using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class PurchaseOrderDetailRepository : CrudMultiKeyRepository<PurchaseOrderDetail, String, String>
    {
        public PurchaseOrderDetailRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.PurchaseOrderDetail;
        }

        public IQueryable<PurchaseOrderDetail> FindDetailsById(string purchaseOrderNo)
        {
            return context.PurchaseOrderDetail
                .Where(x => x.PurchaseOrder.PurchaseOrderNo == purchaseOrderNo);
        }






    }
}