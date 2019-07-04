using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ImageWebController : Controller
    {
        static ImageWeb ImageViewInfo = new ImageWeb();
        public ImageWebController()
        {
        }

        // GET: ImageWeb
        public ActionResult WebImageFront()
        {
            ImageViewInfo.CheckConnection();
            ViewBag.Status= ImageViewInfo.Status;
            ViewBag.Count = ImageViewInfo.NumberOfImages();
            return View(ImageViewInfo);
        }
    }
}