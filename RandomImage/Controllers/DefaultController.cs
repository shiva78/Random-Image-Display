using RandomImage.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RandomImage.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            ViewBag.Images = getImages();
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(string answer, Preference model)
        {
            User_PreferencesEntities db = new User_PreferencesEntities();
            string username = model.UserName;
            if (answer == "Display History")
            {
                var historyData = db.Preferences.Where(x => x.UserName.Contains(username)).ToList();
                if (historyData.Count > 0)
                {
                    ViewBag.HistoryData = historyData;
                }
                else
                {
                    ViewBag.NoHistory = "Sorry you don't have any history to display.";
                }
            }
            else
            {
                string image = model.Image;
                db.Preferences.Add(new Preference
                {
                    Id = model.Id,
                    UserName = model.UserName,
                    User_Preferences = answer,
                    Image = model.Image

                });
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    e.ToString();
                }
            }
            ViewBag.Images = getImages();
            return View();
        }

        private dynamic getImages()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "Images";
            var filenames = Directory
                .EnumerateFiles(path, "*", SearchOption.AllDirectories)
                .Select(Path.GetFileName).ToList();
            return filenames; 
        }
    }
}