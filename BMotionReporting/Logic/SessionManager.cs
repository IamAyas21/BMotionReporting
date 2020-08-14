using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMotionReporting.Logic
{
    public class SessionManager
    {
        // GET: SessionManager
        public static string NIP()
        {
            string value = string.Empty;
            try
            {
                value = HttpContext.Current.Session["nip"].ToString();
            }
            catch { }

            return value;
        }
    }
}