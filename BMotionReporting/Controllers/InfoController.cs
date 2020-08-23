using BMotionReporting.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMotionReporting.Controllers
{
    public class InfoController : Controller
    {
        // GET: Info
        public ActionResult Index()
        {
            InfoLogic.SendNotificationFromFirebaseCloud();
            return View();
        }
    }
}