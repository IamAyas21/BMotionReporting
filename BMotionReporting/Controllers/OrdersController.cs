using AForge.Video.DirectShow;
using BMotionReporting.App_Start;
using BMotionReporting.Entity;
using BMotionReporting.Logic;
using BMotionReporting.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BMotionReporting.Controllers
{
    public class OrdersController : Controller
    {
        BMotionDBEntities db = new BMotionDBEntities();
        // GET: Orders
        [CheckAuthorizeAttribute()]
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

        [CheckAuthorizeAttribute()]
        public ActionResult History()
        {
            var page = new PagedList<sp_RiwayatPengambilanBBM_Result>();
            try
            {
                var model = OrdersLogic.getInstance().GetAllHistory().ToList();
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
                        //OrderDetails model = new OrderDetails();
                        //model.OrderDetailId = Convert.ToInt32(orderNo);
                        //OrdersLogic.getInstance().VerifyOrder(model);
                        
                        var verify = db.sp_OrderDetailVerify(orderNo);
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
                        struk.Name = item.Name;

                        struk.FuelName = item.FuelName;
                        struk.Liter = item.Liter;
                        struk.OrderDetailId = item.OrderDetailId;
                        struk.CreatedDate = item.CreatedDate.Value.ToString("MM/dd/yyyy HH:mm:ss");
                        listModel.Add(struk);
                    }

                }
                else
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }

            } 
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
            }

            return Json(listModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OrderFuel(string NIP)
        {
            try
            {
                NewOrder order = new Models.NewOrder();
                if (db.Users.Where(u => u.NIP.Equals(NIP)).Count() != 0)
                {
                    var user = UserLogic.getInstance().GetUserById(NIP);
                    if (user.IsVerify.ToLower() == "y")
                    {

                        var quota = db.sp_UserQuota(NIP).FirstOrDefault();
                        order.NIP = user.NIP;
                        order.Nama = user.Name;
                        order.Phone = user.Telp;
                        order.Email = user.Email;
                        order.Quota = quota.Quota;

                        FuelOrder fuel = new FuelOrder();
                        List<FuelItemOrder> subsidi = new List<FuelItemOrder>();
                        List<FuelItemOrder> nonSubsidi = new List<FuelItemOrder>();

                        List<Fuel> fuelList = FuelLogic.getInstance().getAllFuel();
                        for (int i = 0; i < fuelList.Count; i++)
                        {
                            FuelItemOrder item = new FuelItemOrder();

                            item.FuelId = fuelList[i].FuilId.ToString();
                            item.Fuel = fuelList[i].Name;

                            if (fuelList[i].IsSubsidy.ToLower() == "y")
                            {
                                subsidi.Add(item);
                            }
                            else
                            {
                                nonSubsidi.Add(item);
                            }
                        }

                        fuel.Subsidi = subsidi;
                        fuel.NonSubsidi = nonSubsidi;

                        order.Fuel = fuel;

                        return Json(order, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }
                else {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
                //OrderDetails detail = new OrderDetails();
                //Orders orderDetails = new Orders();

                //List<OrderDetails> listDetail = new List<OrderDetails>();
                
                //if (arrValue != null)
                //{
                //    string[] arrNIP = arrValue[0].Split(',');
                //    string[] arrFuelId = arrValue[1].Split(',');
                //    string[] arrLiter = arrValue[2].Split(',');

                //    if (order.NIP == null)
                //    {
                //        order = new Models.Orders();
                //        order.NIP = arrNIP[0];
                //    }

                //    for (int i = 0; i < arrFuelId.Length; i++)
                //    {
                //        detail = new OrderDetails();
                //        detail.FuelId = Convert.ToInt32(arrFuelId[i]);
                //        detail.Liter = Convert.ToInt32(arrLiter[i]);
                //        listDetail.Add(detail);
                //    }

                //    order.OrderDetails = listDetail;
                //    orderDetails = OrdersLogic.getInstance().Add(order);
                //}

                //return Json(orderDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult OrderAdd(OrderParameter param)
        {
            try
            {
                return Json(OrdersLogic.getInstance().Add(param), JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [CheckAuthorizeAttribute()]
        public ActionResult ExportToExcel()
        {
            try
            {
                ExcelPackage Package = new ExcelPackage();
                ExcelWorksheet ws = Package.Workbook.Worksheets.Add("Data");
                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#00b0f0");
                OfficeOpenXml.Style.ExcelBorderStyle DefaultBorder = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                ws.Cells["A1"].LoadFromText("No");
                ws.Cells["A1"].Style.Font.SetFromFont(new Font("Cambria", 10));
                ws.Cells["A1"].Style.Font.Color.SetColor(Color.White);
                ws.Cells["A1"].Style.Font.Bold = true;
                ws.Cells["A1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["A1"].Style.Fill.BackgroundColor.SetColor(colFromHex);
                ws.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["A1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells["A1"].Style.Border.Bottom.Style = DefaultBorder;
                ws.Cells["A1"].Style.Border.Left.Style = DefaultBorder;
                ws.Cells["A1"].Style.Border.Top.Style = DefaultBorder;
                ws.Cells["A1"].Style.Border.Right.Style = DefaultBorder;

                ws.Cells["B1"].LoadFromText("Tanggal Transaksi");
                ws.Cells["B1"].Style.Font.SetFromFont(new Font("Cambria", 10));
                ws.Cells["B1"].Style.Font.Color.SetColor(Color.White);
                ws.Cells["B1"].Style.Font.Bold = true;
                ws.Cells["B1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["B1"].Style.Fill.BackgroundColor.SetColor(colFromHex);
                ws.Cells["B1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["B1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells["B1"].Style.Border.Bottom.Style = DefaultBorder;
                ws.Cells["B1"].Style.Border.Left.Style = DefaultBorder;
                ws.Cells["B1"].Style.Border.Top.Style = DefaultBorder;
                ws.Cells["B1"].Style.Border.Right.Style = DefaultBorder;

                ws.Cells["C1"].LoadFromText("NIP");
                ws.Cells["C1"].Style.Font.SetFromFont(new Font("Cambria", 10));
                ws.Cells["C1"].Style.Font.Color.SetColor(Color.White);
                ws.Cells["C1"].Style.Font.Bold = true;
                ws.Cells["C1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["C1"].Style.Fill.BackgroundColor.SetColor(colFromHex);
                ws.Cells["C1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["C1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells["C1"].Style.Border.Bottom.Style = DefaultBorder;
                ws.Cells["C1"].Style.Border.Left.Style = DefaultBorder;
                ws.Cells["C1"].Style.Border.Top.Style = DefaultBorder;
                ws.Cells["C1"].Style.Border.Right.Style = DefaultBorder;

                ws.Cells["D1"].LoadFromText("Pengguna");
                ws.Cells["D1"].Style.Font.SetFromFont(new Font("Cambria", 10));
                ws.Cells["D1"].Style.Font.Color.SetColor(Color.White);
                ws.Cells["D1"].Style.Font.Bold = true;
                ws.Cells["D1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["D1"].Style.Fill.BackgroundColor.SetColor(colFromHex);
                ws.Cells["D1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["D1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells["D1"].Style.Border.Bottom.Style = DefaultBorder;
                ws.Cells["D1"].Style.Border.Left.Style = DefaultBorder;
                ws.Cells["D1"].Style.Border.Top.Style = DefaultBorder;
                ws.Cells["D1"].Style.Border.Right.Style = DefaultBorder;

                ws.Cells["E1"].LoadFromText("Fuel");
                ws.Cells["E1"].Style.Font.SetFromFont(new Font("Cambria", 10));
                ws.Cells["E1"].Style.Font.Color.SetColor(Color.White);
                ws.Cells["E1"].Style.Font.Bold = true;
                ws.Cells["E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["E1"].Style.Fill.BackgroundColor.SetColor(colFromHex);
                ws.Cells["E1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["E1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells["E1"].Style.Border.Bottom.Style = DefaultBorder;
                ws.Cells["E1"].Style.Border.Left.Style = DefaultBorder;
                ws.Cells["E1"].Style.Border.Top.Style = DefaultBorder;
                ws.Cells["E1"].Style.Border.Right.Style = DefaultBorder;

                ws.Cells["F1"].LoadFromText("Qty");
                ws.Cells["F1"].Style.Font.SetFromFont(new Font("Cambria", 10));
                ws.Cells["F1"].Style.Font.Color.SetColor(Color.White);
                ws.Cells["F1"].Style.Font.Bold = true;
                ws.Cells["F1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["F1"].Style.Fill.BackgroundColor.SetColor(colFromHex);
                ws.Cells["F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells["F1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ws.Cells["F1"].Style.Border.Bottom.Style = DefaultBorder;
                ws.Cells["F1"].Style.Border.Left.Style = DefaultBorder;
                ws.Cells["F1"].Style.Border.Top.Style = DefaultBorder;
                ws.Cells["F1"].Style.Border.Right.Style = DefaultBorder;

                //HistoryOrder historyOrders = new HistoryOrder();
                //List<HistoryOrder> historyOrdersList = new List<HistoryOrder>();
                int idx = 1;
                foreach (var item in OrdersLogic.getInstance().GetAllHistoryExport().ToList())
                {
                    //historyOrders = new HistoryOrder();
                    //historyOrders.No = item.No.ToString();
                    //historyOrders.TanggalTransaksi = item.CreateDate;
                    //historyOrders.NIP = item.NIP;
                    //historyOrders.Pengguna = item.Name;
                    //historyOrders.Fuel = item.Fuel;
                    //historyOrders.Qty = string.Format("{0} Liter", item.Liter.ToString());
                    //historyOrdersList.Add(historyOrders);

                    idx++;
                    ws.Cells["A" + idx.ToString()].LoadFromText(item.No.ToString());
                    ws.Cells["A" + idx.ToString()].Style.Font.SetFromFont(new Font("Cambria", 10));
                    ws.Cells["A" + idx.ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A" + idx.ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells["A" + idx.ToString()].Style.Border.Bottom.Style = DefaultBorder;
                    ws.Cells["A" + idx.ToString()].Style.Border.Left.Style = DefaultBorder;
                    ws.Cells["A" + idx.ToString()].Style.Border.Top.Style = DefaultBorder;
                    ws.Cells["A" + idx.ToString()].Style.Border.Right.Style = DefaultBorder;

                    DateTime? createdDate = item.CreateDate;
                    string strCreatedDate = createdDate.Value.ToString("dd MMM yy HH:mm");
                    ws.Cells["B" + idx.ToString()].LoadFromText(strCreatedDate);
                    ws.Cells["B" + idx.ToString()].Style.Font.SetFromFont(new Font("Cambria", 10));
                    ws.Cells["B" + idx.ToString()].Style.Numberformat.Format = "dd MMM yy HH:mm";
                    ws.Cells["B" + idx.ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["B" + idx.ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells["B" + idx.ToString()].Style.Border.Bottom.Style = DefaultBorder;
                    ws.Cells["B" + idx.ToString()].Style.Border.Left.Style = DefaultBorder;
                    ws.Cells["B" + idx.ToString()].Style.Border.Top.Style = DefaultBorder;
                    ws.Cells["B" + idx.ToString()].Style.Border.Right.Style = DefaultBorder;

                    ws.Cells["C" + idx.ToString()].LoadFromText(item.NIP);
                    ws.Cells["C" + idx.ToString()].Style.Numberformat.Format = "##0";
                    ws.Cells["C" + idx.ToString()].Style.Font.SetFromFont(new Font("Cambria", 10));
                    ws.Cells["C" + idx.ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["C" + idx.ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells["C" + idx.ToString()].Style.Border.Bottom.Style = DefaultBorder;
                    ws.Cells["C" + idx.ToString()].Style.Border.Left.Style = DefaultBorder;
                    ws.Cells["C" + idx.ToString()].Style.Border.Top.Style = DefaultBorder;
                    ws.Cells["C" + idx.ToString()].Style.Border.Right.Style = DefaultBorder;

                    ws.Cells["D" + idx.ToString()].LoadFromText(item.Name);
                    ws.Cells["D" + idx.ToString()].Style.Font.SetFromFont(new Font("Cambria", 10));
                    ws.Cells["D" + idx.ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["D" + idx.ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells["D" + idx.ToString()].Style.Border.Bottom.Style = DefaultBorder;
                    ws.Cells["D" + idx.ToString()].Style.Border.Left.Style = DefaultBorder;
                    ws.Cells["D" + idx.ToString()].Style.Border.Top.Style = DefaultBorder;
                    ws.Cells["D" + idx.ToString()].Style.Border.Right.Style = DefaultBorder;

                    ws.Cells["E" + idx.ToString()].LoadFromText(item.Fuel);
                    ws.Cells["E" + idx.ToString()].Style.Font.SetFromFont(new Font("Cambria", 10));
                    ws.Cells["E" + idx.ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["E" + idx.ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells["E" + idx.ToString()].Style.Border.Bottom.Style = DefaultBorder;
                    ws.Cells["E" + idx.ToString()].Style.Border.Left.Style = DefaultBorder;
                    ws.Cells["E" + idx.ToString()].Style.Border.Top.Style = DefaultBorder;
                    ws.Cells["E" + idx.ToString()].Style.Border.Right.Style = DefaultBorder;

                    ws.Cells["F" + idx.ToString()].LoadFromText(item.Liter.ToString());
                    ws.Cells["F" + idx.ToString()].Style.Font.SetFromFont(new Font("Cambria", 10));
                    ws.Cells["F" + idx.ToString()].Style.Numberformat.Format = "#,#0 Liter";
                    ws.Cells["F" + idx.ToString()].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["F" + idx.ToString()].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells["F" + idx.ToString()].Style.Border.Bottom.Style = DefaultBorder;
                    ws.Cells["F" + idx.ToString()].Style.Border.Left.Style = DefaultBorder;
                    ws.Cells["F" + idx.ToString()].Style.Border.Top.Style = DefaultBorder;
                    ws.Cells["F" + idx.ToString()].Style.Border.Right.Style = DefaultBorder;
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=RiwayatPengambilanBBM_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx");
                Response.BinaryWrite(Package.GetAsByteArray());
                Response.End();

                //var gv = new GridView();
                //gv.DataSource = historyOrdersList;
                //gv.DataBind();
                //Response.ClearContent();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", "attachment; filename=RiwayatPengambilanBBM_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls");
                //Response.ContentType = "application/ms-excel";
                //Response.Charset = "";
                //StringWriter objStringWriter = new StringWriter();
                //HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                //gv.RenderControl(objHtmlTextWriter);
                //Response.Output.Write(objStringWriter.ToString());
                //Response.Flush();
                //Response.End();
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
            }

            return View("Index");
        }
    }
}