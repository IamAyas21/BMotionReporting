using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMotionReporting.Models
{
    public class Orders
    {
        string orderNo;
        public String OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }

        string nip;
        public String NIP
        {
            get { return nip; }
            set { nip = value; }
        }

        string isVerify;
        public String IsVerify
        {
            get { return isVerify; }
            set { isVerify = value; }
        }

        string createdDate;
        public String CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        List<OrderDetails> orderDetails;
        public List<OrderDetails> OrderDetails
        {
            get { return orderDetails; }
            set { orderDetails = value; }
        }
    }

    public class HistoryOrder
    {
        public string No { get; set; }
        public string TanggalTransaksi { get; set; }
        public string NIP { get; set; }
        public string Pengguna { get; set; }
        public string Fuel { get; set; }
        public string Qty { get; set; }
    }
}