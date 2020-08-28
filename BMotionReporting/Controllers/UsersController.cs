using BMotionReporting.App_Start;
using BMotionReporting.Entity;
using BMotionReporting.Logic;
using BMotionReporting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRCoder;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace BMotionReporting.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        //[CheckAuthorizeAttribute(Roles = "Users")]
        [CheckAuthorizeAttribute()]
        public ActionResult Index()
        {
            var page = new PagedList<sp_UserPengguna_Result>();
            try
            {
                
                var model = UserLogic.getInstance().GetAllUsers().ToList();
                page.Content = model;
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
            }
            return View(page);
        }

        [CheckAuthorizeAttribute()]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserModels model)
        {
            try
            {
                if(UserLogic.getInstance().CheckExistingUser(model))
                {
                    UserLogic.getInstance().Add(model);
                    TempData["Success"] = "Success saving Data for " + model.Name;
                }
                else
                {
                    TempData["Error"] = "User " + model.Name + " is exist";
                }
            }
            catch (Exception e)
            { 
                Logging.Log.getInstance().CreateLogError(e);
            }
            
            return RedirectToAction("Index");
        }

        [CheckAuthorizeAttribute()]
        public ActionResult Detail(string id)
        {
            UserModels model = new UserModels();
            try
            {
                model = UserLogic.getInstance().GetUserById(id);
                model.DocumentList = Document(id);
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Detail(UserModels model)
        {
            try
            {
                UserLogic.getInstance().Verification(model);
                TempData["Success"] = "Success verification for " + model.NIP;
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error verification for " + model.NIP;
                Logging.Log.getInstance().CreateLogError(e);
            }

            return RedirectToAction("Detail", "Users", new { id = model.NIP });
        }

        private PagedList<DocumentModels> Document(string id)
        { 
            List<DocumentModels> list = DocumentLogic.getInstance().getAllDocumentByUser(id);
            var page = new PagedList<DocumentModels>();
            try
            {
                page.Content = list;
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
            }
            return page;
        }
       

        [HttpPost]
        public ActionResult Verification(string[] arrValue)
        {
            try
            {
                DocumentLogic.getInstance().Verification(arrValue[0]);
                TempData["Success"] = "Success verification for " + arrValue[0];
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error verification for " + arrValue[0];
                Logging.Log.getInstance().CreateLogError(e);
            }

            return RedirectToAction("Detail", "Users", new { id = arrValue[1] });
        }

        public ActionResult NewPrintCards(string nip)
        {
            try
            {
                UserModels user = UserLogic.getInstance().GetUserById(nip);
                return View(user);//Json(user, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("error" + e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult PrintCards(string nip)
        {
            try
            {
               
                UserModels user = UserLogic.getInstance().GetUserById(nip);
                return Json(user, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("error" + e.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}