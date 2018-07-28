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
        ItemPriceRepository itemPriceRepository;
        ApplicationDbContext context;

        public PurchaseOrderService(ApplicationDbContext context)
        {
            this.context = context;
            purchaseOrderRepository = new PurchaseOrderRepository(context);
            purchaseOrderDetailRepository = new PurchaseOrderDetailRepository(context);
            statusRepository = new StatusRepository(context);
            itemPriceRepository = new ItemPriceRepository(context);
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


        public void SavePurchaseOrderDetails(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetailRepository.Save(purchaseOrderDetail);
        }


        public List<PurchaseOrder> CreatePOForEachSupplier(List<Item> items)
        {
            List<Supplier> supList = new List<Supplier>();
            List<PurchaseOrder> poList = new List<PurchaseOrder>();

            foreach (Item i in items)
            {
                ItemPrice ip = i.ItemPrices.Where(x => x.PrioritySequence == 1).First();
                int num = 1;

                if (!supList.Contains(ip.Supplier))
                {
                    supList.Add(ip.Supplier);

                    PurchaseOrder p = new PurchaseOrder();
                    p.PurchaseOrderNo = "PONO" + num;
                    p.Supplier = ip.Supplier;
                    p.CreatedDateTime = DateTime.Now;

                    poList.Add(p);

                } 
            }

            return poList;
        }


        public List<PurchaseOrder> AddItemsToPurchaseOrders(List<OrderItem> orderItems,List<PurchaseOrder> poList)
        {
           
            foreach(PurchaseOrder po in poList)
            {
                po.PurchaseOrderDetails=new List<PurchaseOrderDetail>();
                foreach(OrderItem orderItem in orderItems)
                {
                    ItemPrice ip = orderItem.Item.ItemPrices.Where(x => x.PrioritySequence == 1).First();

                    if(po.Supplier.SupplierCode == ip.Supplier.SupplierCode)
                    {
                        PurchaseOrderDetail pd = new PurchaseOrderDetail();
                        pd.PurchaseOrderNo = po.PurchaseOrderNo;
                        pd.Item = orderItem.Item;
                        pd.Quantity = orderItem.Quantity;

                        po.PurchaseOrderDetails.Add(pd);
                        
                    }
                }
            }

                return poList;
        }


        public bool IsPurchaseOrderCreated(Item item, List<PurchaseOrder> poList)
        {
            //if(poList!=null && poList.Where(x => x.PurchaseOrderDetails.ForEach(y=>y.)!=null))
            //{ 

            //}
            foreach(PurchaseOrder po in poList)
            {
               //bool result=purchaseOrderDetailRepository.ExistsById(po.PurchaseOrderNo, item.ItemCode);
               bool result2 = po.PurchaseOrderDetails.Count(x => x.Item.ItemCode == item.ItemCode) > 0;
                    
                    if (result2)
                {
                    return true;
                }
            }

            return false;
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

        


    }
}