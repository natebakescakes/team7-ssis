using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class DeliveryOrderService
    {
        ApplicationDbContext context;
        DeliveryOrderRepository deliveryOrderRepository;
        PurchaseOrderRepository purchaseOrderRepository;
        //PurchaseOrderDetailRepository purchaseOrderDetailsRepository;
        //DeliveryOrderDetailRepository deliveryOrderDetailRepository;
        StockMovementRepository stockMovementRepository;
        StatusRepository statusRepository;
        InventoryRepository inventoryRepository;
        ItemRepository itemRepository;

        public DeliveryOrderService(ApplicationDbContext context)
        {
            this.context = context;
            this.deliveryOrderRepository = new DeliveryOrderRepository(context);
            this.purchaseOrderRepository = new PurchaseOrderRepository(context);
           // this.deliveryOrderDetailRepository = new DeliveryOrderDetailRepository(context);
            this.stockMovementRepository = new StockMovementRepository(context);
            this.purchaseOrderRepository = new PurchaseOrderRepository(context);
            this.statusRepository = new StatusRepository(context);
            this.inventoryRepository = new InventoryRepository(context);
            this.itemRepository = new ItemRepository(context);
        }

        public List<DeliveryOrder> FindAllDeliveryOrders()
        {
            return deliveryOrderRepository.FindAll().ToList();
        }

        public DeliveryOrder FindDeliveryOrderById(string deliveryOrderNo)
        {
            // Exceptions
            if (deliveryOrderRepository.FindById(deliveryOrderNo)==null)
            {
                throw new ArgumentException();
            }
             return deliveryOrderRepository.FindById(deliveryOrderNo);
        }

        
        public DeliveryOrder FindDeliveryOrderByPurchaseOrderNo(string purchaseOrderNo)
        {
            // Exceptions
            if (deliveryOrderRepository.FindDeliveryOrderByPurchaseOrderNo(purchaseOrderNo) == null)
            {
                throw new ArgumentException();
            }
            return deliveryOrderRepository.FindDeliveryOrderByPurchaseOrderNo(purchaseOrderNo);
        }

        

        public DeliveryOrder FindDeliveryOrderBySupplier(string supplierCode)
        {
            // Exceptions
            if (deliveryOrderRepository.FindDeliveryOrderBySupplier(supplierCode) == null)
            {
                throw new ArgumentException();
            }
            return deliveryOrderRepository.FindDeliveryOrderBySupplier(supplierCode);
        }
  
        public void Save(DeliveryOrder deliveryOrder)
        {
            //Inventory inv;
            //StockMovement sm;
            //foreach (DeliveryOrderDetail dod in deliveryOrder.DeliveryOrderDetails)
            //    if (dod.ActualQuantity == dod.PlanQuantity)
            //        deliveryOrder.PurchaseOrder.Status = statusRepository.FindById(13);
            //    else
            //        deliveryOrder.PurchaseOrder.Status = statusRepository.FindById(12);
            //deliveryOrderRepository.Save(deliveryOrder);
            //purchaseOrderRepository.Save(deliveryOrder.PurchaseOrder);
            //foreach (DeliveryOrderDetail dod in deliveryOrder.DeliveryOrderDetails)
            //{
            //    inv = inventoryRepository.FindById(dod.ItemCode);
            //    inventoryRepository.Save(inv);
            //    sm = new StockMovement();
            //    sm.Item = itemRepository.FindById(dod.ItemCode);
            //    stockMovementRepository.Save(sm);
            //}

            throw new NotImplementedException();
        }

        public void SaveDOFileToDeliveryOrder(HttpPostedFileBase file)
        {
            throw new NotImplementedException();
            //if (file.ContentLength > 0)
            //{
            //    var fileName = Path.GetFileName(file.FileName);
            //    var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/DoFiles"), fileName);
            //    file.SaveAs(path);
            //}
        }

         public void SaveInvoiceFileToDeliveryOrder(string Filepath)
        {
            throw new NotImplementedException();
          //  deliveryOrderRepository.SaveInvoiceFileToDeliveryOrder(Filepath);
            //Need to send to Finance department
        }

    }
}
