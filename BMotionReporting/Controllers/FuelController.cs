using BMotionReporting.App_Start;
using BMotionReporting.Entity;
using BMotionReporting.Logic;
using BMotionReporting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BMotionReporting.Controllers
{
    public class FuelController : Controller
    {
        // GET: Fuel
        BMotionDBEntities db = new BMotionDBEntities();

        [CheckAuthorizeAttribute()]
        public ActionResult Index()
        {
            List<Entity.Fuel> fuel = FuelLogic.getInstance().getAllFuel();
            List<FuelModels> Data = new List<FuelModels>();
            for(int i = 0; i < fuel.Count; i++)
            {
                FuelModels item = new FuelModels();
                item.FuilId = fuel[i].FuilId;
                item.Name = fuel[i].Name;
                item.Price = fuel[i].Price;
                item.IsSubsidy = fuel[i].IsSubsidy == "Y"?"Subsidi":"Non Subsidi";
                item.BackgroundColor = fuel[i].BackgroundColor;
                item.TextColor = fuel[i].TextColor;
                item.CreatedDate = fuel[i].CreatedDate;
                item.CreatedBy = fuel[i].CreatedBy;
                item.UpdatedDate = fuel[i].UpdatedDate;
                item.UpdatedBy = fuel[i].UpdatedBy;

                Data.Add(item);
            }
            PagedList<FuelModels> page = new PagedList<FuelModels>();
            page.Content = Data;
            return View(page);
        }

        [CheckAuthorizeAttribute()]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(FuelModels model)
        {
            try
            {
                if (FuelLogic.getInstance().CheckExistingData(model))
                {
                    FuelLogic.getInstance().Add(model);
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
        public ActionResult Edit(int ID)
        {
            FuelModels model = new FuelModels();
            var data = db.Fuels.Where(f => f.FuilId == ID).ToList();
            if(data.Count() > 0)
            {
                model.FuilId = data.FirstOrDefault().FuilId;
                model.Name = data.FirstOrDefault().Name;
                model.Price = data.FirstOrDefault().Price;
                model.IsSubsidy = data.FirstOrDefault().IsSubsidy;
                model.BackgroundColor = data.FirstOrDefault().BackgroundColor;
                model.TextColor = data.FirstOrDefault().TextColor;
                model.CreatedDate = data.FirstOrDefault().CreatedDate;
                model.CreatedBy = data.FirstOrDefault().CreatedBy;
                model.UpdatedDate = data.FirstOrDefault().UpdatedDate;
                model.UpdatedBy = data.FirstOrDefault().UpdatedBy;
            }
            else
            {

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(FuelModels model)
        {
            try
            {
                FuelLogic.getInstance().Update(model);
                TempData["Success"] = "Success saving Data for " + model.Name;
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
            }

            return RedirectToAction("Index");
        }

        //[HttpPost]
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            FuelLogic.getInstance().Remove(id);
            TempData["Success"] = "Delete Success";
            return RedirectToAction("Index");
        }
    }
}