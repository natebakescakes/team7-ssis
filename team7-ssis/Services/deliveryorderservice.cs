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
        PurchaseOrderService purchaseOrderService;
 

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
            this.purchaseOrderService = new PurchaseOrderService(context);
        }

        public List<DeliveryOrder> FindAllDeliveryOrders()
        {
            return deliveryOrderRepository.FindAll().ToList();
        }

        public DeliveryOrder FindDeliveryOrderById(string deliveryOrderNo)
        {
            return deliveryOrderRepository.FindById(deliveryOrderNo);
        }

        
        public List<DeliveryOrder> FindDeliveryOrderByPurchaseOrderNo(string purchaseOrderNo)
        {
            return deliveryOrderRepository.FindDeliveryOrderByPurchaseOrderNo(purchaseOrderNo).ToList();
        }

        public List<DeliveryOrder> FindDeliveryOrderBySupplier(string supplierCode)
        {
            return deliveryOrderRepository.FindDeliveryOrderBySupplier(supplierCode).ToList();
        }

        public DeliveryOrder Save(DeliveryOrder deliveryOrder)
        {
            return deliveryOrderRepository.Save(deliveryOrder);
        }

        public void CheckSave(DeliveryOrder deliveryOrder)
        {
            foreach (DeliveryOrderDetail dod in deliveryOrder.DeliveryOrderDetails)
            {

                PurchaseOrderDetail purchaseOrderDetail = purchaseOrderService.FindPurchaseOrderDetailbyIdItem(deliveryOrder.PurchaseOrder.PurchaseOrderNo, dod.ItemCode);

                if (dod.ActualQuantity >= dod.PlanQuantity)

                    purchaseOrderDetail.Status = statusRepository.FindById(13);

                else

                    purchaseOrderDetail.Status = statusRepository.FindById(12);


                purchaseOrderDetailsRepository.Save(purchaseOrderDetail);

                deliveryOrderDetailRepository.Save(dod);

                SaveInventory(itemRepository.FindById(dod.ItemCode), dod.ActualQuantity);

                SaveStockMovement(dod);
            }
            
            deliveryOrderRepository.Save(deliveryOrder);


            PurchaseOrder po = deliveryOrder.PurchaseOrder;
            int flag=0;

            foreach (PurchaseOrderDetail pod in po.PurchaseOrderDetails)
            {

                if ((pod.Status.StatusId == 12) || (pod.Status.StatusId == 11))
                {
                    po.Status = statusRepository.FindById(12);

                    purchaseOrderRepository.Save(po);

                    flag = 1;

                    break;
                }
            }


            if (flag!=1)
            {
                po.Status = statusRepository.FindById(13);
                purchaseOrderRepository.Save(po);
            }
        }


        public Inventory SaveInventory(Item item, int receivedQuantity)
        {
            Inventory iv = inventoryRepository.FindById(item.ItemCode);
            iv.Quantity = iv.Quantity+receivedQuantity;
            return inventoryRepository.Save(iv);
        }

        public StockMovement SaveStockMovement(DeliveryOrderDetail deliveryOrderDetail)
        {
            StockMovement sm = new StockMovement();
            sm.DeliveryOrderDetail = deliveryOrderDetail;
            sm.DeliveryOrderNo = deliveryOrderDetail.DeliveryOrderNo;
            sm.DeliveryOrderDetailItemCode = deliveryOrderDetail.ItemCode;
            sm.Item = deliveryOrderDetail.Item;
            sm.OriginalQuantity = inventoryRepository.FindById(deliveryOrderDetail.ItemCode).Quantity;
            sm.AfterQuantity = sm.OriginalQuantity + deliveryOrderDetail.ActualQuantity;
            sm.CreatedDateTime = DateTime.Now;
            sm.StockMovementId = IdService.GetNewStockMovementId(context);
            return stockMovementRepository.Save(sm);
        }

        public DeliveryOrderDetail SaveDeliveryOrderDetails(DeliveryOrderDetail deliveryOrderDetail)
        {
            return deliveryOrderDetailRepository.Save(deliveryOrderDetail);
        }
    }
}
