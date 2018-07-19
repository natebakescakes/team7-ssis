using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;


namespace team7_ssis.Services
{
    public class PurchaseOrderService
    {
        PurchaseOrderRepository purchaseOrderRepository;
        PurchaseOrderDetailRepository purchaseOrderDetailRepository;
        StatusRepository statusRepository;
        ApplicationDbContext context;

        public PurchaseOrderService(ApplicationDbContext context)
        {
            this.context = context;
            purchaseOrderRepository = new PurchaseOrderRepository(context);
            purchaseOrderDetailRepository = new PurchaseOrderDetailRepository(context);
            statusRepository = new StatusRepository(context);
        }

        public void DeleteItemFromPurchaseOrder(PurchaseOrder purchaseOrder,params string[] itemCodes)
        {
            if (purchaseOrder.Status.StatusId == 11)
            {
                foreach (string s in itemCodes)
                {
                    PurchaseOrderDetail detail = purchaseOrderDetailRepository.FindById(purchaseOrder.PurchaseOrderNo, s);
                    purchaseOrderDetailRepository.Delete(detail);
                }
            }
          
        }

        public List<PurchaseOrder> FindAllPurchaseOrders()
        {
            return purchaseOrderRepository.FindAll().ToList();
        }

        public PurchaseOrder FindPurchaseOrderById(string purchaseOrderNo)
        {
            return purchaseOrderRepository.FindById(purchaseOrderNo);
        }

        


        public List<PurchaseOrder> FindPurchaseOrderBySupplier(Supplier supplier)
        {
            return purchaseOrderRepository.FindBySupplier(supplier.SupplierCode).ToList();
        }

        public List<PurchaseOrder> FindPurchaseOrderBySupplier(string supplierCode)
        {
            return purchaseOrderRepository.FindBySupplier(supplierCode).ToList();
        }

        
        public List<PurchaseOrder> FindPurchaseOrderByStatus(params int[] statusId)
        {
            return purchaseOrderRepository.FindByStatus(statusId).ToList();
        }


        public PurchaseOrder Save(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrderRepository.FindById(purchaseOrder.PurchaseOrderNo)==null)
            {
                purchaseOrder.Status = statusRepository.FindById(11);
                return purchaseOrderRepository.Save(purchaseOrder);
            }

            else
            {
                return purchaseOrderRepository.Save(purchaseOrder);
            }
            
        }

    }
}