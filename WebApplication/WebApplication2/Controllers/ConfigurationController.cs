using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ConfigurationController : Controller
    {
        static Configuration configuration = Configuration.GetInstance;        // GET: Configuration
        static string handlerToRemove = string.Empty;
        public ConfigurationController()
        {
            //configuration.Changed -= Changed;
            // configuration.Changed += Changed;
        }
        public ActionResult ConfigurationView()
        {
            return View(configuration);
        }

        public ActionResult ConfirmView(string handler)
        {
            handlerToRemove = handler;
            ViewBag.name = handler;
            return View("ConfirmView");
        }
        public ActionResult RemoveHandler()
        {
            configuration.RemoveHandler(handlerToRemove);
            Thread.Sleep(3000);
            return RedirectToAction("ConfigurationView");
        }
    }
}