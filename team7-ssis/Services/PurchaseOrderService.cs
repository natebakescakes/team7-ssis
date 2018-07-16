using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Services
{
    public class PurchaseOrderService
    {
            ApplicationDbContext context;

            public PurchaseOrderService(ApplicationDbContext context)
            {
                this.context = context;

            }

            public void RemoveDraftItemFromPurchaseOrder(string itemCode, PurchaseOrder purchaseOrder)
            {
                throw new NotImplementedException();
            }

            public List<PurchaseOrder> FindAllPurchaseOrders()
            {
                throw new NotImplementedException();
            }

            public PurchaseOrder FindPurchaseOrderById(string purchaseOrderNo)
            {
                throw new NotImplementedException();
            }

            public List<PurchaseOrderDetail> FindPurchaseOrderDetailsById(string purchaseOrderNo)
            {
                throw new NotImplementedException();
            }

            public List<PurchaseOrder> FindPurchaseOrderBySupplier(Supplier supplier)
            {
                throw new NotImplementedException();
            }

            public List<PurchaseOrder> FindPurchaseOrderBySupplier(string supplierCode)
            {
                throw new NotImplementedException();
            }

            public PurchaseOrder FindPurchaseOrderByDeliveryOrder(string deliveryOrderNo)
            {
                throw new NotImplementedException();
            }

            public List<PurchaseOrder> FindPurchaseOrderByStatus(params string[] statusId)
            {
                throw new NotImplementedException();
            }

            public PurchaseOrder Save(PurchaseOrder purchaseOrder)
            {
                throw new NotImplementedException();
            }
   
    
}