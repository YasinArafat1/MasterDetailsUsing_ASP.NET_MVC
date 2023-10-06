using MasterDetailes.Models;
using MasterDetailes.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterDetailes.Controllers
{
    public class ClientsController : Controller
    {
        TravelDbContext db = new TravelDbContext();
        public ActionResult Index()
        {
            var clients = db.Clients.Include(x => x.BookingEntries.Select(b => b.Spot)).OrderByDescending(x => x.ClientId).ToList();

            return View(clients);
        }





        public ActionResult AddNewSpot(int? id)
        {
            ViewBag.spots = new SelectList(db.Spots.ToList(), "SpotId", "SpotName", (id != null) ? id.ToString() : "");
            return PartialView("_addNewSpot");

        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(ClientVM clientVM, int[] spotId)
        {
            if (ModelState.IsValid)
            {
                Client client = new Client()
                {
                    ClientName = clientVM.ClientName,
                    BirthDate = clientVM.BirthDate,
                    Age = clientVM.Age,
                    MaritalStatus = clientVM.MaritalStatus,
                };

                HttpPostedFileBase file = clientVM.PictureFile;
                if (file != null)
                {
                    string filePath = Path.Combine("/Images/", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));

                    file.SaveAs(Server.MapPath(filePath));

                    client.Picture = filePath;
                }

                //save all spot from SpotId
                foreach (var item in spotId)
                {
                    BookingEntry bookingEntry = new BookingEntry()
                    {

                        Client = client,
                        ClientId = client.ClientId,
                        SpotId = item,

                    };
                    db.BookingEntries.Add(bookingEntry);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }

            }
            return View();

        }


        public ActionResult Edit(int? id)
        {
            Client client = db.Clients.First(x => x.ClientId == id);


            var clientSpots = db.BookingEntries.Where(x => x.ClientId == id).ToList();

            ClientVM clientVM = new ClientVM()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                Age = client.Age,
                BirthDate = client.BirthDate,
                Picture = client.Picture,
                MaritalStatus = client.MaritalStatus,

            };
            if (clientSpots.Count > 0)
            {
                foreach (var item in clientSpots)
                {
                    clientVM.SpotList.Add(item.SpotId);
                }
            }
            return View(clientVM);

        }

        [HttpPost, ValidateAntiForgeryToken]

        public ActionResult Edit(ClientVM clientVM, int[] spotId)
        {
            if (ModelState.IsValid)
            {
                //Retrieve The existing client from database
                Client existingClient = db.Clients.Find(clientVM.ClientId);

                if (existingClient == null)
                {
                    return HttpNotFound();
                }

                existingClient.ClientName = clientVM.ClientName;
                existingClient.BirthDate = clientVM.BirthDate;
                existingClient.Age = clientVM.Age;
                existingClient.MaritalStatus = clientVM.MaritalStatus;

                HttpPostedFileBase file = clientVM.PictureFile;
                if (file != null)
                {
                    string filePath = Path.Combine("/Images/", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    existingClient.Picture = filePath;
                }

                //below codes for spot from SpotId

                var existsSpotEntry = db.BookingEntries.Where(x => x.ClientId == existingClient.ClientId).ToList();

                //below code for remove spot from selected spot while editing 

                foreach (var bookingEntry in existsSpotEntry)
                {
                    db.BookingEntries.Remove(bookingEntry);
                }


                //user can add a spot while editing and also delete a spot while editing using remove method

                foreach (var item in spotId)
                {
                    BookingEntry bookingEntry = new BookingEntry()
                    {
                        ClientId = existingClient.ClientId,
                        SpotId = item
                    };
                    db.BookingEntries.Add(bookingEntry);
                }

                db.Entry(existingClient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View();


        }

        public ActionResult Delete(int? id)
        {
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            var entries = db.BookingEntries.Where(x => x.ClientId == id).ToList();

            ClientVM clientVM = new ClientVM()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                BirthDate = client.BirthDate,
                Age = client.Age,
                MaritalStatus = client.MaritalStatus,
            };

            if (entries.Count > 0)
            {
                foreach (var item in entries)
                {
                    clientVM.SpotList.Add(item.SpotId);
                }
            }
            return View(clientVM);

        }

        [HttpPost, ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DoDelete(int id)
        {
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            db.Entry(client).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
           
        }




    }
}