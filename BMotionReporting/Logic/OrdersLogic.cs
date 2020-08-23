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
        public Orders Add(Orders order)
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
                db.Orders.Add(orderEntity);
                db.SaveChanges();

                foreach (var item in order.OrderDetails)
                {
                    db = new BMotionDBEntities();
                    OrderDetail orderDetailEntity = new OrderDetail();
                    orderDetailEntity.OrderNo = orderEntity.OrderNo;
                    orderDetailEntity.FuelId = item.FuelId;
                    orderDetailEntity.Liter = item.Liter;
                    orderDetailEntity.CreatedDate = DateTime.Now;
                    orderDetailEntity.CreatedBy = orderEntity.NIP;
                    db.OrderDetails.Add(orderDetailEntity);
                    db.SaveChanges();
                }

                order.OrderNo = guidId.ToString();
                order.IsVerify = "N";

                return order;
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
                db = new BMotionDBEntities();
                OrderDetail dtl = (from d in db.OrderDetails
                                    where d.OrderDetailId.Equals(model.OrderDetailId)
                                    select d).First();
                dtl.IsVerify = "Y";
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}