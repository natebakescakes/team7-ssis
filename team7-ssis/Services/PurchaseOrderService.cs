using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.ViewModels;



namespace team7_ssis.Services
{
    public class PurchaseOrderService
    {
        PurchaseOrderRepository purchaseOrderRepository;
        PurchaseOrderDetailRepository purchaseOrderDetailRepository;
        StatusRepository statusRepository;
        SupplierRepository supplierRepository;
        ItemPriceRepository itemPriceRepository;
        ItemService itemService;
        RequisitionService requisitionService;
        ApplicationDbContext context;

        public PurchaseOrderService(ApplicationDbContext context)
        {
            this.context = context;
            purchaseOrderRepository = new PurchaseOrderRepository(context);
            purchaseOrderDetailRepository = new PurchaseOrderDetailRepository(context);
            statusRepository = new StatusRepository(context);
            supplierRepository = new SupplierRepository(context);
            itemPriceRepository = new ItemPriceRepository(context);
            requisitionService = new RequisitionService(context);
            itemService = new ItemService(context);

        }

        public void DeleteItemFromPurchaseOrder(PurchaseOrder purchaseOrder,params string[] itemCodes)
        {
            if (purchaseOrder != null && itemCodes.Length != 0)
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
          
        }

        public void CancelItemFromPurchaseOrder(string purchaseOrderNum , string itemCode)
        {
            if (purchaseOrderNum !=""  && purchaseOrderNum !=null && itemCode!=null && itemCode!="" )
            {
                PurchaseOrderDetail pod = purchaseOrderDetailRepository.FindById(purchaseOrderNum, itemCode);

                if (pod.Status.StatusId == 11)
                {
                    pod.Status = statusRepository.FindById(2);
                    purchaseOrderDetailRepository.Save(pod);
                }
            }
 
        }

        public List<PurchaseOrder> FindAllPurchaseOrders()
        {
            if (purchaseOrderRepository.FindAll().ToList() != null)
            {
                return purchaseOrderRepository.FindAll().ToList();
            }

            return null;   
        }

        public PurchaseOrder FindPurchaseOrderById(string purchaseOrderNo)
        {
            if (purchaseOrderRepository.ExistsById(purchaseOrderNo))
            {
                return purchaseOrderRepository.FindById(purchaseOrderNo);
            }
            else
                return null;
        }

        


        public List<PurchaseOrder> FindPurchaseOrderBySupplier(Supplier supplier)
        {
            if (supplierRepository.ExistsById(supplier.SupplierCode))
            {
                return purchaseOrderRepository.FindBySupplier(supplier.SupplierCode).ToList();
            }

            return null;
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
                return purchaseOrderRepository.Save(purchaseOrder);
            
        }


        public List<PurchaseOrder> CreatePOForEachSupplier(List<Supplier> suppliers)
        {
            
            List<PurchaseOrder> poList = new List<PurchaseOrder>();

            foreach(Supplier supplier in suppliers)
            {
                PurchaseOrder p = new PurchaseOrder();
                p.PurchaseOrderNo = IdService.GetNewPurchaseOrderNo(context);
                p.Supplier = supplier;
                p.CreatedDateTime = DateTime.Now;
                p.Status = statusRepository.FindById(11);
                p.Status.StatusId = 11;
                purchaseOrderRepository.Save(p);

                poList.Add(p);
            }

            return poList;
        }

        public List<Item> FindInventoryShortfallItems()
        {
            List<Item> items = itemService.FindAllItems();
            List<Item> itemsToDelete = new List<Item>();
            foreach (Item i in items)
            {
                if (IsPurchaseOrderCreated(i))
                {
                    itemsToDelete.Add(i);
                }
            }

            foreach (Item item in itemsToDelete)
            {
            //    foreach (Item i in items)
            //    {
            //        if (i.ItemCode == item.ItemCode)
            //        {
                        items.Remove(item);
            //            break;
            //        }
            //    }
            }


            //items.RemoveAll(IsPurchaseOrderCreated);

            return items;
        }




        public bool IsPurchaseOrderCreated(Item item)
        {
            bool isPurchaseOrderCreated = false;
            int[] statusId = { 11, 12 };
            int remainingPOQuantity = 0;
            int amountToReorder = 0;

            List<PurchaseOrder> awaitingPO = FindPurchaseOrderByStatus(statusId);

            foreach (PurchaseOrder po in awaitingPO)
            {
                foreach (PurchaseOrderDetail pod in po.PurchaseOrderDetails)
                {
                    if (pod.ItemCode == item.ItemCode)
                    {
                        if (pod.Status.StatusId == 11 || pod.Status.StatusId == 12)
                        {
                            remainingPOQuantity = remainingPOQuantity + FindRemainingQuantity(pod);
                        }
                    }

                }
            }

            if(item.Inventory.Quantity - requisitionService.FindUnfulfilledQuantityRequested(item) < item.ReorderLevel)
            {
                amountToReorder = Math.Max(requisitionService.FindUnfulfilledQuantityRequested(item) + item.ReorderQuantity, item.ReorderLevel - item.Inventory.Quantity + requisitionService.FindUnfulfilledQuantityRequested(item));
            }
            else
            {
                amountToReorder = 0;
            }

            if (remainingPOQuantity >= amountToReorder)
            {
                isPurchaseOrderCreated = true;
            }

            return isPurchaseOrderCreated;

        }

        public decimal FindUnitPriceByPurchaseOrderDetail(PurchaseOrderDetail pod)
        {
            List<ItemPrice> itemPriceList=itemPriceRepository.FindBySupplierCode(pod.PurchaseOrder.Supplier.SupplierCode).ToList();
            return itemPriceList.Where(x => x.Item.ItemCode == pod.Item.ItemCode).First().Price;
        }


        public decimal FindTotalAmountByPurchaseOrderDetail(PurchaseOrderDetail pod)
        {
            decimal unitPrice = FindUnitPriceByPurchaseOrderDetail(pod);
            return unitPrice * pod.Quantity;
        }


        public int FindReceivedQuantityByPurchaseOrderDetail(PurchaseOrderDetail pod)
        {
            int receivedQuantity = 0;

            if (pod.PurchaseOrder.Status.StatusId != 11)
            {
                foreach (DeliveryOrder delOrder in pod.PurchaseOrder.DeliveryOrders)
                {
                    foreach (DeliveryOrderDetail delOrderDetail in delOrder.DeliveryOrderDetails)
                    {
                        if (delOrderDetail.Item.ItemCode == pod.ItemCode)
                        {
                            receivedQuantity = receivedQuantity + delOrderDetail.ActualQuantity;
                        }
                    }
                }

            }

           return receivedQuantity;
           
        }

        public int FindRemainingQuantity(PurchaseOrderDetail pod)
        {
            int receivedQuantity = FindReceivedQuantityByPurchaseOrderDetail(pod);
            return pod.Quantity - receivedQuantity;
        }

        public PurchaseOrderDetail SavePurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail)
        {
            return purchaseOrderDetailRepository.Save(purchaseOrderDetail);
        }


        public PurchaseOrderDetail FindPurchaseOrderDetailbyIdItem(string id1, string id2)
        {
            return purchaseOrderDetailRepository.FindById(id1, id2);
        }

        public List<PurchaseOrderDetail> FindPurchaseOrderDetailByIdStatus(string id,params int[] statusId)
        {
            return purchaseOrderRepository.FindPurchaseOrderDetailByIdStatus(id,statusId).ToList();
        }

    }
}