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
        ApplicationDbContext context;

        public PurchaseOrderService(ApplicationDbContext context)
        {
            this.context = context;
            purchaseOrderRepository = new PurchaseOrderRepository(context);
            purchaseOrderDetailRepository = new PurchaseOrderDetailRepository(context);
            statusRepository = new StatusRepository(context);
            supplierRepository = new SupplierRepository(context);
            itemPriceRepository = new ItemPriceRepository(context);

        }

        public void DeleteItemFromPurchaseOrder(PurchaseOrder purchaseOrder, params string[] itemCodes)
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

        public void DeletePurchaseOrderDetailsFromPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail)
        {
                        purchaseOrderDetailRepository.Delete(purchaseOrderDetail);
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


   

        public bool IsPurchaseOrderCreated(Item item, List<PurchaseOrder> poList)
        {
           
            foreach(PurchaseOrder po in poList)
            {
               
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