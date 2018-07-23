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
        DeliveryOrderDetailRepository deliveryOrderDetailRepository;
        StatusRepository statusRepository;
        InventoryRepository inventoryRepository;
        PurchaseOrderRepository purchaseOrderRepository;
        PurchaseOrderDetailRepository purchaseOrderDetailsRepository;
        ItemRepository itemRepository;
        StockMovementRepository stockMovementRepository;

        public DeliveryOrderService(ApplicationDbContext context)
        {
            this.context = context;
            this.deliveryOrderRepository = new DeliveryOrderRepository(context);
            this.statusRepository = new StatusRepository(context);
            this.inventoryRepository = new InventoryRepository(context);
            this.purchaseOrderRepository = new PurchaseOrderRepository(context);
            this.stockMovementRepository = new StockMovementRepository(context);
            this.purchaseOrderDetailsRepository= new PurchaseOrderDetailRepository(context) ;
            this.itemRepository = new ItemRepository(context);
            this.deliveryOrderDetailRepository = new DeliveryOrderDetailRepository(context);
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
  
        public DeliveryOrder Save(DeliveryOrder deliveryOrder)
        {
            deliveryOrderRepository.Save(deliveryOrder);
            foreach (DeliveryOrderDetail dod in deliveryOrder.DeliveryOrderDetails)
            {
                if (dod.ActualQuantity == dod.PlanQuantity)
                    dod.Status = statusRepository.FindById(13);
                else
                    dod.Status = statusRepository.FindById(12);
                deliveryOrderDetailRepository.Save(dod);
                SaveInventory(itemRepository.FindById(dod.ItemCode), dod.ActualQuantity);
                SaveStockMovement(dod, itemRepository.FindById(dod.ItemCode), dod.ActualQuantity);
            }
             PurchaseOrder po = deliveryOrder.PurchaseOrder;
            foreach (DeliveryOrderDetail dod in deliveryOrder.DeliveryOrderDetails)
            {
                if(dod.Status.StatusId==12)
                {
                    itemRepository.FindById(dod.ItemCode);
                    po.Status = statusRepository.FindById(12);
                    purchaseOrderRepository.Save(po);
                    break;
                }        
            }

            if (po.Status.StatusId == 11)
            {
                po.Status = statusRepository.FindById(13);
                purchaseOrderRepository.Save(po);
            }
            
            purchaseOrderRepository.Save(deliveryOrder.PurchaseOrder);
            return deliveryOrder;
        }

        public Inventory SaveInventory(Item item, int quantity)
        {
            Inventory iv = inventoryRepository.FindById(item.ItemCode);
            iv.Quantity = iv.Quantity+quantity;
            return inventoryRepository.Save(iv);
        }

        public StockMovement SaveStockMovement(DeliveryOrderDetail deliveryOrderDetail, Item item, int quantity)
        {
            StockMovement sm = new StockMovement();
            sm.StockMovementId = IdService.GetNewStockMovementId(context);
            sm.DeliveryOrderDetail = deliveryOrderDetail;
            sm.Item = item;
            sm.OriginalQuantity = inventoryRepository.FindById(item.ItemCode).Quantity; 
            sm.AfterQuantity = sm.OriginalQuantity + quantity;
            sm.CreatedDateTime = DateTime.Now;
            return stockMovementRepository.Save(sm);
        }


        public int UploadDeliveryOrderFile(HttpPostedFileBase file)
        { 
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = System.Web.HttpContext.Current.Server.MapPath("~/DOFiles");

                file.SaveAs(path + fileName);
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public void SaveInvoiceFileToDeliveryOrder(string Filepath)
        {
            throw new NotImplementedException();
          //  deliveryOrderRepository.SaveInvoiceFileToDeliveryOrder(Filepath);
            //Need to send to Finance department
        } 
    }
}
