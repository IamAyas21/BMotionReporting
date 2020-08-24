using AForge.Video.DirectShow;
using BMotionReporting.App_Start;
using BMotionReporting.Entity;
using BMotionReporting.Logic;
using BMotionReporting.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMotionReporting.Controllers
{
    public class OrdersController : Controller
    {
        BMotionDBEntities db = new BMotionDBEntities();
        // GET: Orders
        public ActionResult Index()
        {
           return View();
        }

        [CheckAuthorizeAttribute()]
        public ActionResult Activity()
        {
            var page = new PagedList<sp_OrderMonitoring_Result>();
            try
            {
                var model = OrdersLogic.getInstance().GetAllActivity().ToList();
                page.Content = model;
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
            }
            return View(page);
        }

        [HttpPost]
        public ActionResult StrukBBM(string orderNo)
        {
            StruckModels struk = new StruckModels();
            //StruckOrderDetails orderDetail = new StruckOrderDetails();
            FuelModels fuelModels = new FuelModels();
            List<StruckModels> listModel = new List<StruckModels>();
            List<StruckOrderDetails> listOrder = new List<StruckOrderDetails>();
            List<FuelModels> fuelList = new List<FuelModels>();
            try
            {
                int n;
                bool isNumeric = int.TryParse(orderNo, out n);
                var list = db.sp_StrukPengambilanBBM(orderNo).ToList();
                if (isNumeric)
                {
                    // Confirmation Struck
                    if (db.OrderDetails.AsEnumerable().Where(dtl => dtl.OrderDetailId == Int32.Parse(orderNo) && dtl.IsVerify == null).Count() > 0)
                    {
                        OrderDetails model = new OrderDetails();
                        model.OrderDetailId = Convert.ToInt32(orderNo);
                        OrdersLogic.getInstance().VerifyOrder(model);
                        return Json("OK", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
                // Get Struk
                else if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        struk = new StruckModels();
                        struk.OrderNo = item.OrderNo;
                        struk.OutletNo = item.OutletNo;
                        struk.OutletName = item.OutletName;

                        struk.FuelName = item.FuelName;
                        struk.Liter = item.Liter;
                        struk.OrderDetailId = item.OrderDetailId;
                        struk.CreatedDate = item.CreatedDate.Value.ToString("MM/dd/yyyy HH:mm");
                        listModel.Add(struk);

                        //orderDetail = new StruckOrderDetails();
                        //orderDetail.OrderDetailId = item.OrderDetailId;
                        //orderDetail.FuelName = item.FuelName;
                        //orderDetail.Liter = item.Liter;
                        //orderDetail.CreatedDate = item.CreatedDate.Value.ToString("MM/dd/yyyy HH:mm");
                        //listOrder.Add(orderDetail);
                    }

                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }


                //if (db.OrderDetails.AsEnumerable().Where(dtl => dtl.OrderDetailId == Int32.Parse(orderNo)).Count() > 0)
                //{
                //    OrderDetails model = new OrderDetails();
                //    model.OrderDetailId = Convert.ToInt32(orderNo);
                //    OrdersLogic.getInstance().VerifyOrder(model);

                //}
                //else
                //{
                //    var listFuel = db.Fuels.ToList();
                //    foreach(var item in listFuel)
                //    {
                //        fuelModels = new FuelModels();
                //        fuelModels.NIP = orderNo;
                //        fuelModels.FuelId = item.FuilId;
                //        fuelModels.FuelName = item.Name;
                //        fuelList.Add(fuelModels);
                //    }
                //    return Json(fuelList, JsonRequestBehavior.AllowGet);
                //}

            } 
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
            }

            return Json(listModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OrderFuel(string[] arrValue)
        {
            try
            {
                Orders order = new Models.Orders();
                OrderDetails detail = new OrderDetails();
                Orders orderDetails = new Orders();

                List<OrderDetails> listDetail = new List<OrderDetails>();
                
                if (arrValue != null)
                {
                    string[] arrNIP = arrValue[0].Split(',');
                    string[] arrFuelId = arrValue[1].Split(',');
                    string[] arrLiter = arrValue[2].Split(',');

                    if (order.NIP == null)
                    {
                        order = new Models.Orders();
                        order.NIP = arrNIP[0];
                    }

                    for (int i = 0; i < arrFuelId.Length; i++)
                    {
                        detail = new OrderDetails();
                        detail.FuelId = Convert.ToInt32(arrFuelId[i]);
                        detail.Liter = Convert.ToInt32(arrLiter[i]);
                        listDetail.Add(detail);
                    }

                    order.OrderDetails = listDetail;
                    orderDetails = OrdersLogic.getInstance().Add(order);
                }

                return Json(orderDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}