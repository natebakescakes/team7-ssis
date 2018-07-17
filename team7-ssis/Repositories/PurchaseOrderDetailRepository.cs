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

        public IQueryable<PurchaseOrderDetail> FindPODetailsById(string purchaseOrderNo)
        {
            return context.PurchaseOrderDetail
                .Where(x => x.PurchaseOrder.PurchaseOrderNo==purchaseOrderNo);
        }

        public void DeleteItemFromPO(string itemCode, string purchaseOrder)
        {
            PurchaseOrderDetail r = context.PurchaseOrderDetail.Where(x => x.PurchaseOrderNo == purchaseOrder && x.ItemCode == itemCode).First();
            entity.Remove(r);
            context.SaveChanges();
        }

        


    }
}