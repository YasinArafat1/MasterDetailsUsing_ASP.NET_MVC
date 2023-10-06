using MasterDetailes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterDetailes.Controllers
{
    public class SpotsController : Controller
    {
      TravelDbContext db = new TravelDbContext();
        public ActionResult Index()
        {
            return View(db.Spots.ToList());
        }

        public ActionResult Create()
        {

            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Spot spot)
        {
            if (ModelState.IsValid)
            {
                db.Spots.Add(spot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        return View(spot);
        }

        public ActionResult Edit(int? id )
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Spot spot = db.Spots.Find(id);
            if (spot==null)
            {
               return HttpNotFound();
            }

            return View(spot);

        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Spot spots)
        {
            if (ModelState.IsValid)
            {
                db.Entry(spots).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(); 
        }

        public ActionResult Delete(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Spot spot = db.Spots.Find(id);
            if (spot == null)
            {
                return HttpNotFound();
            }



            return View(spot);
        }



    }

    }
