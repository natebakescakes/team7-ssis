using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Services;
using team7_ssis.Models;

namespace team7_ssis.Controllers
{
    public class DeliveryOrderController : Controller
    {
        public static ApplicationDbContext context = new ApplicationDbContext();
        DeliveryOrderService deliveryOrderService = new DeliveryOrderService(context);
        PurchaseOrderService purchaseOrderService = new PurchaseOrderService(context);
        PurchaseOrderService purchaseOrderDetailService = new PurchaseOrderService(context);

        // GET: DeliveryOrder
        public ActionResult Index()
        {
            return View("ReceiveGoods");
        }

        public ActionResult OutstandingItems()
        {
            int[] myIntArray = { 11,12 };

            List<PurchaseOrder> list = purchaseOrderService.FindPurchaseOrderByStatus(myIntArray);
            List<PurchaseOrderDetail> dlist=new List <PurchaseOrderDetail>();

            foreach (PurchaseOrder po in list)
            {
                foreach (PurchaseOrderDetail pod in po.PurchaseOrderDetails)
                {
                    dlist.Add(pod);
                }
            }

            var v = dlist.Select(x => new
                    {
                        ItemCode = x.ItemCode,

                        Description =x.Item.Description,

                        OutstandingQty=x.Quantity
                    });
            return View("OutstandingItems");

            //  return Json(new { data = v }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]

        public ActionResult ImageUpload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    int i = deliveryOrderService.UploadDeliveryOrderFile(file);
                    if (i == 1)
                    {
                        ViewBag.Message = "File uploaded successfully";
                    }

                    else
                    {
                        ViewBag.Message = "File uploading Unsuccessful";
                    }
                }

                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }

            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            return RedirectToAction("Index");
        }

    }
}