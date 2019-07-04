using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class PhotoController : Controller
    {
        private PhotoAlbum photos = new PhotoAlbum();
        private static string currentImagePath;
        private static string currentThumbPath;

        // GET: Photo
        public ActionResult PhotoView()
        {
            return View(photos.PhotosAlbum);
        }
        public ActionResult DeletePhoto(string name, string thumbPath, string photoPath, string photoRelThumb)
        {
            currentImagePath = photoPath;
            currentThumbPath = thumbPath;
            ViewBag.path = photoRelThumb;
            ViewBag.name = name;
            return View();
        }
        public ActionResult RemovePhoto()
        {
            System.IO.File.Delete(currentThumbPath);
            System.IO.File.Delete(currentImagePath);
            Thread.Sleep(2000);
            return RedirectToAction("PhotoView");
        }
        public ActionResult ViewerPhoto(string photoRelPath, string name, string year, string month, string thumbPath, string photoPath)
        {
            currentImagePath = photoPath;
            currentThumbPath = thumbPath;
            ViewBag.path = photoRelPath;
            ViewBag.name = name;
            ViewBag.year = year;
            ViewBag.month = month;
            return View();
        }
    }
}