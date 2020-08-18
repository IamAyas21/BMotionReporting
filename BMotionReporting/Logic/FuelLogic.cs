using BMotionReporting.Entity;
using BMotionReporting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMotionReporting.Logic
{
    public class FuelLogic
    {
        BMotionDBEntities db = new BMotionDBEntities();

        static FuelLogic _FuelLogic = null;

        public static FuelLogic getInstance()
        {
            if (_FuelLogic == null)
            {
                _FuelLogic = new FuelLogic();
                return _FuelLogic;
            }
            else
            {
                return _FuelLogic;
            }
        }

        public List<Fuel> getAllFuel()
        {
            return db.Fuels.ToList();
        }

        public bool CheckExistingData(FuelModels model)
        {
            if (db.Fuels.Where(fuel => fuel.Name.Equals(model.Name)).Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Add(FuelModels model)
        {
            try
            {
                string dateTimeDayNow = DateTime.Now.ToString("ddMMyyyyHHmmss");
                string dateDayNow = DateTime.Now.ToString("ddMMyyyy");

                db = new BMotionDBEntities();
                Fuel fuelEntity = new Fuel();
                fuelEntity.FuilId = model.FuilId;
                fuelEntity.Name = model.Name;
                fuelEntity.Price = model.Price;
                fuelEntity.IsSubsidy = model.IsSubsidy;
                fuelEntity.BackgroundColor = model.BackgroundColor;
                fuelEntity.TextColor = model.TextColor;
                fuelEntity.CreatedDate = DateTime.Now;
                fuelEntity.CreatedBy = SessionManager.NIP();
                db.Fuels.Add(fuelEntity);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(FuelModels model)
        {
            try
            {
                db = new BMotionDBEntities();
                Fuel fuel = (from u in db.Fuels
                            where u.FuilId.Equals(model.FuilId)
                            select u).First();
                fuel.Name = model.Name;
                fuel.Price = model.Price;
                fuel.IsSubsidy = model.IsSubsidy;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public void Remove(int FuelId)
        {
            try
            {
                db = new BMotionDBEntities();
                Fuel fuel = (from u in db.Fuels
                             where u.FuilId.Equals(FuelId)
                             select u).First();
                db.Fuels.Remove(fuel);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}