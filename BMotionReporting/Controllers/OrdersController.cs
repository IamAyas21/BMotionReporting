using AForge.Video.DirectShow;
using BMotionReporting.Entity;
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

        [HttpPost]
        public ActionResult StrukBBM(string orderNo)
        {
            StruckModels struk = new StruckModels();
            StruckOrderDetails orderDetail = new StruckOrderDetails();
            FuelModels fuelModels = new FuelModels();
            List<StruckModels> listModel = new List<StruckModels>();
            List<StruckOrderDetails> listOrder = new List<StruckOrderDetails>();
            List<FuelModels> fuelList = new List<FuelModels>();
            try
            {
                var list = db.sp_StrukPengambilanBBM(orderNo).ToList();
                if(list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        if (struk.OutletNo == null)
                        {
                            struk = new StruckModels();
                            struk.OrderNo = item.OrderNo;
                            struk.OutletNo = item.OutletNo;
                            struk.OutletName = item.OutletName;
                        }

                        orderDetail = new StruckOrderDetails();
                        orderDetail.FuelName = item.FuelName;
                        orderDetail.Liter = item.Liter;
                        orderDetail.CreatedDate = item.CreatedDate.Value.ToString("MM/dd/yyyy HH:mm");

                        listOrder.Add(orderDetail);
                    }

                    struk.StruckOrderDetails = listOrder;
                    listModel.Add(struk);
                }
                else
                {
                    var listFuel = db.Fuels.ToList();
                    foreach(var item in listFuel)
                    {
                        fuelModels = new FuelModels();
                        fuelModels.FuelId = item.FuilId;
                        fuelModels.FuelName = item.Name;
                        fuelList.Add(fuelModels);
                    }
                    return Json(fuelList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
            }
           
            return Json(listModel, JsonRequestBehavior.AllowGet);   
        }
    }
}