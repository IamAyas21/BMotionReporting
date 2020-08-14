using BMotionReporting.App_Start;
using BMotionReporting.Logic;
using BMotionReporting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMotionReporting.Controllers
{
    public class DocumentController : Controller
    {
        // GET: Document
        public ActionResult Index()
        {
            return View();
        }
        [CheckAuthorizeAttribute()]
        public ActionResult Upload(string id)
        {
            DocumentModels model = new DocumentModels();
            model.NIP = id;
            return View(model);
        }
        [HttpPost]
        public ActionResult Upload(DocumentModels model)
        {
            try
            {
                if(DocumentLogic.getInstance().CheckExistingDocument(model))
                {
                    DocumentLogic.getInstance().Add(model);
                    TempData["Success"] = "Success saving Data for " + model.NIP;
                }
                else
                {
                    TempData["Error"] = "Document " + model.DocumentNo +" is exist";
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error saving Data for " + model.NIP;
                Logging.Log.getInstance().CreateLogError(e);
            }

            return RedirectToAction("Detail","Users",new { id = model.NIP });
        }
       
    }
}