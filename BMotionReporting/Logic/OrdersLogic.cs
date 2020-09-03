using BMotionReporting.Entity;
using BMotionReporting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMotionReporting.Logic
{
    public class OrdersLogic
    {
        BMotionDBEntities db = new BMotionDBEntities();
        static OrdersLogic orderLgc = null;
        public static OrdersLogic getInstance()
        {
            if (orderLgc == null)
            {
                orderLgc = new OrdersLogic();
                return orderLgc;
            }
            else
            {
                return orderLgc;
            }
        }

        public List<sp_OrderMonitoring_Result> GetAllActivity()
        {
            var list = db.sp_OrderMonitoring("").ToList();
            return list;
        }

        public List<sp_RiwayatPengambilanBBM_Result> GetAllHistory()
        {
            var list = db.sp_RiwayatPengambilanBBM(SessionManager.NIP()).ToList();
            return list;
        }

        public List<sp_RiwayatPengambilanBBMExport_Result> GetAllHistoryExport()
        {
            var list = db.sp_RiwayatPengambilanBBMExport(SessionManager.NIP()).ToList();
            return list;
        }

        public string Add(OrderParameter order)
        {
            try
            {
                Guid guidId = Guid.NewGuid();
                db = new BMotionDBEntities();
                Order orderEntity = new Order();
                orderEntity.OrderNo = guidId.ToString();
                orderEntity.NIP = order.NIP;
                orderEntity.IsVerify = "N";
                orderEntity.CreatedDate = DateTime.Now;
                orderEntity.CreatedBy = orderEntity.NIP;
                orderEntity.ExpiredDate = DateTime.Now.AddHours(4);
                db.Orders.Add(orderEntity);
                db.SaveChanges();

                foreach (var item in order.Orders)
                {
                    db = new BMotionDBEntities();
                    int n;
                    bool isNumeric = int.TryParse(item.Liter, out n);
                    if(isNumeric && n != 0)
                    {
                        OrderDetail orderDetailEntity = new OrderDetail();
                        orderDetailEntity.OrderNo = orderEntity.OrderNo;
                        orderDetailEntity.FuelId = Convert.ToInt32(item.FuelId);
                        orderDetailEntity.Liter = n;
                        orderDetailEntity.CreatedDate = DateTime.Now;
                        orderDetailEntity.CreatedBy = orderEntity.NIP;
                        db.OrderDetails.Add(orderDetailEntity);
                        db.SaveChanges();
                    }
                }
                return guidId.ToString();
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
                throw e;
            }
        }

        public void VerifyOrder(OrderDetails model)
        {
            try
            {
                Nullable<int> literSubsidy = 0;
                db = new BMotionDBEntities();
                OrderDetail dtl = (from d in db.OrderDetails
                                    where d.OrderDetailId.Equals(model.OrderDetailId)
                                    select d).First();
                dtl.IsVerify = "Y";
                dtl.UpdatedDate = DateTime.Now;
                db.SaveChanges();

                db = new BMotionDBEntities();
                Fuel fl = (from f in db.Fuels
                           where f.FuilId == dtl.FuelId
                           select f).First();
                db = new BMotionDBEntities();
                if (fl.IsSubsidy.ToLower()=="y")
                {
                    literSubsidy = dtl.Liter;
                }
                db = new BMotionDBEntities();
                Order or = (from o in db.Orders
                             where o.OrderNo.Equals(dtl.OrderNo)
                             select o).First();
                db = new BMotionDBEntities();
                Document dc = (from d in db.Documents
                               where d.NIP.Equals(or.NIP) && d.IsVerify.Equals("Y")
                               select d).First();

                dc.Quota = dc.Quota - literSubsidy;
                db.SaveChanges();

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}