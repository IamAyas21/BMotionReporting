using BMotionReporting.App_Start;
using BMotionReporting.Entity;
using BMotionReporting.Logic;
using BMotionReporting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMotionReporting.Controllers
{
    public class InfoController : Controller
    {
        BMotionDBEntities db = new BMotionDBEntities();
        // GET: Info
        [CheckAuthorizeAttribute()]
        public ActionResult Index()
        {
            //InfoLogic.SendNotificationFromFirebaseCloud();
            List<sp_Notification_Result> Notif = InfoLogic.getInstance().getAllInfo();
            PagedList<sp_Notification_Result> page = new PagedList<sp_Notification_Result>();
            page.Content = Notif;
            return View(page);
        }

        [CheckAuthorizeAttribute()]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(sp_Notification_Result model)
        {
            try
            {
                InfoLogic.getInstance().Add(model);
                TempData["Success"] = "Info " + model.Title + " telah dipublikasikan.";
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
            }

            return RedirectToAction("Index");
        }
    }
}