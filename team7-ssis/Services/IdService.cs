using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public static class IdService
    {
        public static int GetNewCollectionPointId(ApplicationDbContext context)
        {
            return context.CollectionPoint
                .OrderByDescending(x => x.CollectionPointId)
                .FirstOrDefault()
                .CollectionPointId + 1;
        }

        public static int GetNewDelegationId(ApplicationDbContext context)
        {
            return context.Delegation
                .OrderByDescending(x => x.DelegationId)
                .FirstOrDefault()
                .DelegationId + 1;
        }

        public static string GetNewDeliveryOrderNo(ApplicationDbContext context)
        {
            string prefix = "DO";
            int serialNo = new DeliveryOrderRepository(context)
                .FindByCreatedDateTime(DateTime.Now.Date, DateTime.Now.Date.AddDays(1))
                .Count() >= 0 ?
                new DeliveryOrderRepository(context)
                    .FindByCreatedDateTime(DateTime.Now.Date, DateTime.Now.Date.AddDays(1))
                    .AsEnumerable()
                    .Select(x => Int32.Parse(x.DeliveryOrderNo.Substring(x.DeliveryOrderNo.Length - 3)))
                    .OrderByDescending(x => x)
                    .FirstOrDefault() + 1 : 1;

            return $"{prefix}-{DateTime.Now.Year}{DateTime.Now.Month:00}-{serialNo:000}";
        }

        public static string GetNewDisbursementId(ApplicationDbContext context)
        {
            string prefix = "DSB";
            int serialNo = new DisbursementRepository(context)
                .FindByCreatedDateTime(DateTime.Now.Date, DateTime.Now.Date.AddDays(1))
                .Count() >= 0 ?
                new DisbursementRepository(context)
                    .FindByCreatedDateTime(DateTime.Now.Date, DateTime.Now.Date.AddDays(1))
                    .AsEnumerable()
                    .Select(x => Int32.Parse(x.DisbursementId.Substring(x.DisbursementId.Length - 3)))
                    .OrderByDescending(x => x)
                    .FirstOrDefault() + 1 : 1;

            return $"{prefix}-{DateTime.Now.Year}{DateTime.Now.Month:00}-{serialNo:000}";
        }

        public static string GetNewItemCode(ApplicationDbContext context, ItemCategory itemCategory)
        {
            // Unsure if can be programmatically generated
            throw new NotImplementedException();
        }
        
        public static int GetNewItemCategoryId(ApplicationDbContext context)
        {
            return context.ItemCategory
                .OrderByDescending(x => x.ItemCategoryId)
                .FirstOrDefault()
                .ItemCategoryId + 1;
        }

        public static int GetNewNotificationId(ApplicationDbContext context)
        {
            return context.Notification
                .OrderByDescending(x => x.NotificationId)
                .FirstOrDefault()
                .NotificationId + 1;
        } 

        public static string GetNewPurchaseOrderNo(ApplicationDbContext context)
        {
            string prefix = "PO";
            int serialNo = new PurchaseOrderRepository(context)
                .FindByCreatedDateTime(DateTime.Now.Date, DateTime.Now.Date.AddDays(1))
                .Count() >= 0 ?
                new PurchaseOrderRepository(context)
                    .FindByCreatedDateTime(DateTime.Now.Date, DateTime.Now.Date.AddDays(1))
                    .AsEnumerable()
                    .Select(x => Int32.Parse(x.PurchaseOrderNo.Substring(x.PurchaseOrderNo.Length - 3)))
                    .OrderByDescending(x => x)
                    .FirstOrDefault() + 1 : 1;

            return $"{prefix}-{DateTime.Now.Year}{DateTime.Now.Month:00}-{serialNo:000}";
        }

        public static string GetNewRequisitionId(ApplicationDbContext context)
        {
            string prefix = "REQ";
            int serialNo = new RequisitionRepository(context)
                .FindByCreatedDateTime(DateTime.Now.Date, DateTime.Now.Date.AddDays(1))
                .Count() >= 0 ?
                new RequisitionRepository(context)
                    .FindByCreatedDateTime(DateTime.Now.Date, DateTime.Now.Date.AddDays(1))
                    .AsEnumerable()
                    .Select(x => Int32.Parse(x.RequisitionId.Substring(x.RequisitionId.Length - 3)))
                    .OrderByDescending(x => x)
                    .FirstOrDefault() + 1 : 1;

            return $"{prefix}-{DateTime.Now.Year}{DateTime.Now.Month:00}-{serialNo:000}";
        }

        public static string GetNewRetrievalId(ApplicationDbContext context)
        {
            string prefix = "RET";
            int serialNo = new RetrievalRepository(context)
                .FindByCreatedDateTime(DateTime.Now.Date, DateTime.Now.Date.AddDays(1))
                .Count() >= 0 ?
                new RetrievalRepository(context)
                    .FindByCreatedDateTime(DateTime.Now.Date, DateTime.Now.Date.AddDays(1))
                    .AsEnumerable()
                    .Select(x => Int32.Parse(x.RetrievalId.Substring(x.RetrievalId.Length - 3)))
                    .OrderByDescending(x => x)
                    .FirstOrDefault() + 1 : 1;

            return $"{prefix}-{DateTime.Now.Year}{DateTime.Now.Month:00}-{serialNo:000}";
        }

        public static string GetNewStockAdjustmentId(ApplicationDbContext context)
        {
            string prefix = "ADJ";
            int serialNo = new StockAdjustmentRepository(context)
                .FindByCreatedDateTime(DateTime.Now.Date, DateTime.Now.Date.AddDays(1))
                .Count() >= 0 ?
                new StockAdjustmentRepository(context)
                    .FindByCreatedDateTime(DateTime.Now.Date, DateTime.Now.Date.AddDays(1))
                    .AsEnumerable()
                    .Select(x => Int32.Parse(x.StockAdjustmentId.Substring(x.StockAdjustmentId.Length - 3)))
                    .OrderByDescending(x => x)
                    .FirstOrDefault() + 1 : 1;

            return $"{prefix}-{DateTime.Now.Year}{DateTime.Now.Month:00}-{serialNo:000}";
        }

        public static int GetNewStockMovementId(ApplicationDbContext context)
        {
            return context.StockMovement
                .OrderByDescending(x => x.StockMovementId)
                .FirstOrDefault()
                .StockMovementId + 1;
        }
    }
}